using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Planor.Domain.Entities;

namespace Planor.Application.Common.Interfaces;

public interface IBlobService
{
    Task<Blob> UploadAsync(IFormFile file, string name);
    Task DeleteIfExistsAsync(int? blobId);
    Task DeleteIfExistsAsync(Blob? blob);
    Task DeleteAsync(int blobId);
    Task DeleteAsync(Blob blob);
}