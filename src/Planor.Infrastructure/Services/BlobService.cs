using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Services;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly IApplicationDbContext _context;

    public BlobService(BlobServiceClient blobServiceClient, IApplicationDbContext context)
    {
        _blobContainerClient = blobServiceClient.GetBlobContainerClient("planor-images");
        _context = context;
    }

    public async Task<Blob> UploadAsync(IFormFile file, string name)
    {
        //get file extension
        var fileInfo = new FileInfo(file.FileName);
        var fileName = $"{name}-{Guid.NewGuid()}{fileInfo.Extension}";

        using var stream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(stream);
        var newFile = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = file.Headers
        };
        
        var blobClient = _blobContainerClient.GetBlobClient(newFile.FileName);

        await using var data = newFile.OpenReadStream();
        await blobClient.UploadAsync(data);

        return new Blob(blobClient.Name, blobClient.Uri.AbsoluteUri);
    }

    public async Task DeleteIfExistsAsync(int? blobId)
    {
        if (blobId is null) return;
        var blob = _context.Blobs.FirstOrDefault(x => x.Id == blobId);

        await DeleteIfExistsAsync(blob);
    }

    public async Task DeleteIfExistsAsync(Blob? blob)
    {
        if (blob is null) return;
        
        var blobClient = _blobContainerClient.GetBlobClient(blob.Name);
        await blobClient.DeleteIfExistsAsync();
        
        _context.Blobs.Remove(blob);
        await _context.SaveChangesAsync(default);
    }

    public async Task DeleteAsync(int blobId)
    {
        var blob = _context.Blobs.FirstOrDefault(x => x.Id == blobId);
        if (blob is null) return;

        await DeleteAsync(blob);
    }

    public async Task DeleteAsync(Blob blob)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blob.Name);
        await blobClient.DeleteAsync();
        
        _context.Blobs.Remove(blob);
        await _context.SaveChangesAsync(default);
    }
}