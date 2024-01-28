using System.Linq.Expressions;
using Planor.Domain.Entities;

namespace Planor.Application.Common.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Deletes job from Hangfire with given job Id list
    /// Verilen jobId listesi ile tüm jobları siler
    /// </summary>
    /// <param name="jobIdList">List of Hangfire job Id</param>
    /// <returns>false if one failed, true if all succeed</returns>
    bool RemoveRange(IEnumerable<string> jobIdList);
    
    /// <summary>
    /// Deletes job from Hangfire with given job Id
    /// </summary>
    /// <param name="jobId">Hangfire job Id</param>
    /// <returns>false if failed, true if succeed</returns>
    bool Remove(string jobId);

    /// <summary>
    /// Creates a job that will be run by Hangfire when the time comes by delay.
    /// </summary>
    /// <param name="notificationType">Must be in NotificationType class</param>
    /// <param name="data">anonymous object data for Handlebars template</param>
    /// <param name="user">User</param>
    /// <param name="delay">When will the job start</param>
    /// <returns></returns>
    string? Schedule(string notificationType, object data, User user, DateTimeOffset delay);

    /// <summary>
    /// Creates a job that will work with rety policy by Hangfire. If the job to be run receives an error, it is tried again.
    /// </summary>
    /// <param name="notificationType">Must be in NotificationType class</param>
    /// <param name="data">anonymous object data for Handlebars template</param>
    /// <param name="user">User</param>
    void Send(string notificationType, object data, User user);
    
}