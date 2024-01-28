using Planor.Domain.Entities;

namespace Planor.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<string> CreateUserAsync(string userName, string password);

    Task<bool> DeleteUserAsync(string userId);
    
    Task<string> GenerateToken(User user);
    
    Task<string> CreateMailConfirmationToken(User user);
    
    Task<bool> ConfirmMailConfirmationToken(User user, string token);

}