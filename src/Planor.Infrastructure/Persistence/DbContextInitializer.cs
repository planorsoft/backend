using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Infrastructure.Persistence;

public static class InitialiserExtensions
{
    public static void InitialiseHangfireDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var connection = new MySqlConnection(connectionString);  
        var command = new MySqlCommand();  
        command.Connection = connection;  
        command.CommandText = "CREATE DATABASE IF NOT EXISTS hangfire";  
        connection.Open();  
        command.ExecuteNonQuery();  
        connection.Close();  
    }
    
    public static void InitialiseDatabase(this WebApplication app)
    {
        app.InitialiseDatabaseAsync().Wait();
    }

    private static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<DbContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }
}

public class DbContextInitializer
{
    private readonly ILogger<DbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbContextInitializer(ILogger<DbContextInitializer> logger,
        ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Tenants.Any())
        {
            _context.Tenants.Add(new Tenant
            {
                Name = "planor"
            });

            await _context.SaveChangesAsync(default);
        }

        await SeedIdentity();

        if (!_context.MailTemplates.Any())
        {
            await SeedMailTemplates("planor", true);
        }
    }

    private async Task SeedIdentity()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Admin);

        if (!await _roleManager.RoleExistsAsync(Roles.Admin))
        {
            await _roleManager.CreateAsync(administratorRole);
            await _roleManager.CreateAsync(new IdentityRole(Roles.Manager));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Customer));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Employee));
        }

        if (!await _roleManager.RoleExistsAsync(Roles.TagCreate))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.TagCreate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.TagDelete));
            await _roleManager.CreateAsync(new IdentityRole(Roles.TagRead));
            await _roleManager.CreateAsync(new IdentityRole(Roles.TagUpdate));
        }

        if (!await _roleManager.RoleExistsAsync(Roles.MailTemplateCreate))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.MailTemplateCreate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.MailTemplateDelete));
            await _roleManager.CreateAsync(new IdentityRole(Roles.MailTemplateRead));
            await _roleManager.CreateAsync(new IdentityRole(Roles.MailTemplateUpdate));
        }

        if (!await _roleManager.RoleExistsAsync(Roles.CurrencyCreate))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.CurrencyCreate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.CurrencyUpdate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.CurrencyDelete));
            await _roleManager.CreateAsync(new IdentityRole(Roles.CurrencyRead));
        }

        if (!await _roleManager.RoleExistsAsync(Roles.CustomerCreate))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.CustomerCreate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.CustomerUpdate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.CustomerDelete));
            await _roleManager.CreateAsync(new IdentityRole(Roles.CustomerRead));
        }

        if (!await _roleManager.RoleExistsAsync(Roles.ProjectCreate))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.ProjectCreate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.ProjectRead));
            await _roleManager.CreateAsync(new IdentityRole(Roles.ProjectDelete));
            await _roleManager.CreateAsync(new IdentityRole(Roles.ProjectUpdate));
        }

        if (!await _roleManager.RoleExistsAsync(Roles.DutyRead))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.DutyRead));
            await _roleManager.CreateAsync(new IdentityRole(Roles.DutyCreate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.DutyDelete));
            await _roleManager.CreateAsync(new IdentityRole(Roles.DutyUpdate));
        }

        if (!await _roleManager.RoleExistsAsync(Roles.EventRead))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.EventRead));
            await _roleManager.CreateAsync(new IdentityRole(Roles.EventCreate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.EventDelete));
            await _roleManager.CreateAsync(new IdentityRole(Roles.EventUpdate));
        }
        
        if (!await _roleManager.RoleExistsAsync(Roles.FinanceRead))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.FinanceRead));
            await _roleManager.CreateAsync(new IdentityRole(Roles.FinanceCreate));
            await _roleManager.CreateAsync(new IdentityRole(Roles.FinanceDelete));
            await _roleManager.CreateAsync(new IdentityRole(Roles.FinanceUpdate));
        }
    }


    public async Task SeedMailTemplates(string tenantId, bool saveChanges)
    {
        _context.MailTemplates.Add(new MailTemplate
        {
            Title = "Hesabını onayla, Planor ile plan yapmaya başla!",
            Slug = NotificationType.MailConfirmation,
            TenantId = tenantId,
            Body =
                @"<!doctype html> <html lang=""tr""> <head> <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/> <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/> <title>{{Title}}</title> <style>body,table td{font-size:14px}.body,body{background-color:#f6f6f6}.container,.content{display:block;max-width:580px;padding:10px}body,h1,h2,h3,h4{line-height:1.4;font-family:sans-serif}body,h1,h2,h3,h4,ol,p,table td,ul{font-family:sans-serif}.btn,.btn .link,.content,.wrapper{box-sizing:border-box}.align-center,.btn table td,.footer,h1{text-align:center}.clear,.footer{clear:both}.btn .link,.powered-by .link{text-decoration:none}body{-webkit-font-smoothing:antialiased;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}table{border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%}table td{vertical-align:top}.body{width:100%}.container{margin:0 auto!important;width:580px}.btn,.footer,.main{width:100%}.content{margin:0 auto}.main{background:#fff;border-radius:3px}.wrapper{padding:20px}.content-block{padding-bottom:10px;padding-top:10px}.footer{margin-top:10px}.footer a,.footer p,.footer span,.footer td{color:#999;font-size:12px;text-align:center}h1,h2,h3,h4{color:#000;font-weight:400;margin:0 0 30px}h1{font-size:35px;font-weight:300;text-transform:capitalize}ol,p,ul{font-size:14px;font-weight:400;margin:0 0 15px}ol li,p li,ul li{list-style-position:inside;margin-left:5px}.link{color:#1e1b4b;text-decoration:underline}.btn>tbody>tr>td{padding-bottom:15px}.btn table{width:auto}.btn table td{background-color:#fff;border-radius:5px}.btn .link{background-color:#fff;border:1px solid #1e1b4b;border-radius:5px;color:#1e1b4b;cursor:pointer;display:inline-block;font-size:14px;font-weight:700;margin:0;padding:12px 25px}.btn-primary .link,.btn-primary table td{background-color:#1e1b4b}.btn-primary .link{border-color:#1e1b4b;color:#fff}.last,.mb0{margin-bottom:0}.first,.mt0{margin-top:0}.align-right{text-align:right}.align-left{text-align:left}.preheader{color:transparent;display:none;height:0;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;visibility:hidden;width:0}hr{border:0;border-bottom:1px solid #f6f6f6;margin:20px 0}@media only screen and (max-width:620px){table.body h1{font-size:28px!important;margin-bottom:10px!important}table.body .link,table.body ol,table.body p,table.body span,table.body td,table.body ul{font-size:16px!important}table.body .article,table.body .wrapper{padding:10px!important}table.body .content{padding:0!important}table.body .container{padding:0!important;width:100%!important}table.body .main{border-left-width:0!important;border-radius:0!important;border-right-width:0!important}table.body .btn .link,table.body .btn table{width:100%!important}table.body .img-responsive{height:auto!important;max-width:100%!important;width:auto!important}}@media all{.ExternalClass{width:100%}.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td{line-height:100%}.apple-link .link{color:inherit!important;font-family:inherit!important;font-size:inherit!important;font-weight:inherit!important;line-height:inherit!important;text-decoration:none!important}#MessageViewBody .link{color:inherit;text-decoration:none;font-size:inherit;font-family:inherit;font-weight:inherit;line-height:inherit}.btn-primary table td:hover{background-color:#34495e!important}}</style> </head> <body> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""> <tr> <td>&nbsp;</td><td class=""container""> <div class=""content""> <table role=""presentation"" class=""main""> <tr> <td class=""wrapper""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td> <p>Merhaba {{User}},</p><p>{{Message}}</p><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""> <tbody> <tr> <td align=""left""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tbody> <tr> <td> <a class=""link"">{{Code}}</a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></table> </td></tr></table> <div class=""footer""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td class=""content-block""> <span class=""apple-link"">Planor, Fatih İstanbul</span> <br>Don't like these emails? <a href=""http://i.imgur.com/CScmqnj.gif"">Unsubscribe</a>. </td></tr></table> </div></div></td><td>&nbsp;</td></tr></table> </body> </html>"
        });

        _context.MailTemplates.Add(new MailTemplate
        {
            Title = "Planor uygulamasına davet edildin.",
            Slug = NotificationType.InviteUser,
            TenantId = tenantId,
            Body =
                @"<!doctype html> <html lang=""tr""> <head> <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/> <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/> <title>{{Title}}</title> <style>body,table td{font-size:14px}.body,body{background-color:#f6f6f6}.container,.content{display:block;max-width:580px;padding:10px}body,h1,h2,h3,h4{line-height:1.4;font-family:sans-serif}body,h1,h2,h3,h4,ol,p,table td,ul{font-family:sans-serif}.btn,.btn .link,.content,.wrapper{box-sizing:border-box}.align-center,.btn table td,.footer,h1{text-align:center}.clear,.footer{clear:both}.btn .link,.powered-by .link{text-decoration:none}body{-webkit-font-smoothing:antialiased;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}table{border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%}table td{vertical-align:top}.body{width:100%}.container{margin:0 auto!important;width:580px}.btn,.footer,.main{width:100%}.content{margin:0 auto}.main{background:#fff;border-radius:3px}.wrapper{padding:20px}.content-block{padding-bottom:10px;padding-top:10px}.footer{margin-top:10px}.footer a,.footer p,.footer span,.footer td{color:#999;font-size:12px;text-align:center}h1,h2,h3,h4{color:#000;font-weight:400;margin:0 0 30px}h1{font-size:35px;font-weight:300;text-transform:capitalize}ol,p,ul{font-size:14px;font-weight:400;margin:0 0 15px}ol li,p li,ul li{list-style-position:inside;margin-left:5px}.link{color:#1e1b4b;text-decoration:underline}.btn>tbody>tr>td{padding-bottom:15px}.btn table{width:auto}.btn table td{background-color:#fff;border-radius:5px}.btn .link{background-color:#fff;border:1px solid #1e1b4b;border-radius:5px;color:#1e1b4b;cursor:pointer;display:inline-block;font-size:14px;font-weight:700;margin:0;padding:12px 25px}.btn-primary .link,.btn-primary table td{background-color:#1e1b4b}.btn-primary .link{border-color:#1e1b4b;color:#fff}.last,.mb0{margin-bottom:0}.first,.mt0{margin-top:0}.align-right{text-align:right}.align-left{text-align:left}.preheader{color:transparent;display:none;height:0;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;visibility:hidden;width:0}hr{border:0;border-bottom:1px solid #f6f6f6;margin:20px 0}@media only screen and (max-width:620px){table.body h1{font-size:28px!important;margin-bottom:10px!important}table.body .link,table.body ol,table.body p,table.body span,table.body td,table.body ul{font-size:16px!important}table.body .article,table.body .wrapper{padding:10px!important}table.body .content{padding:0!important}table.body .container{padding:0!important;width:100%!important}table.body .main{border-left-width:0!important;border-radius:0!important;border-right-width:0!important}table.body .btn .link,table.body .btn table{width:100%!important}table.body .img-responsive{height:auto!important;max-width:100%!important;width:auto!important}}@media all{.ExternalClass{width:100%}.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td{line-height:100%}.apple-link .link{color:inherit!important;font-family:inherit!important;font-size:inherit!important;font-weight:inherit!important;line-height:inherit!important;text-decoration:none!important}#MessageViewBody .link{color:inherit;text-decoration:none;font-size:inherit;font-family:inherit;font-weight:inherit;line-height:inherit}.btn-primary table td:hover{background-color:#34495e!important}}</style> </head> <body> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""> <tr> <td>&nbsp;</td><td class=""container""> <div class=""content""> <table role=""presentation"" class=""main""> <tr> <td class=""wrapper""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td> <p>Merhaba {{.Email }},</p><p>{{Message}}</p><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""> <tbody> <tr> <td align=""left""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tbody> <tr> <td> <p class=""link"">{{.Email }}</p><p class=""link"">{{ Password }}</p></td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></table> </td></tr></table> <div class=""footer""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td class=""content-block""> <span class=""apple-link"">Planor, Fatih İstanbul</span> <br>Don't like these emails? <a href=""http://i.imgur.com/CScmqnj.gif"">Unsubscribe</a>. </td></tr></table> </div></div></td><td>&nbsp;</td></tr></table> </body> </html>"
        });

        _context.MailTemplates.Add(new MailTemplate
        {
            Title = "Planor şifreni sıfırla",
            Slug = NotificationType.ForgotMail,
            TenantId = tenantId,
            Body =
                @"<!doctype html><html lang=""tr""> <head> <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/> <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/> <title>{{Title}}</title> <style>body, table td{font-size: 14px}.body, body{background-color: #f6f6f6}.container, .content{display: block; max-width: 580px; padding: 10px}body, h1, h2, h3, h4{line-height: 1.4; font-family: sans-serif}body, h1, h2, h3, h4, ol, p, table td, ul{font-family: sans-serif}.btn, .btn .link, .content, .wrapper{box-sizing: border-box}.align-center, .btn table td, .footer, h1{text-align: center}.clear, .footer{clear: both}.btn .link, .powered-by .link{text-decoration: none}body{-webkit-font-smoothing: antialiased; margin: 0; padding: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%}table{border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%}table td{vertical-align: top}.body{width: 100%}.container{margin: 0 auto !important; width: 580px}.btn, .footer, .main{width: 100%}.content{margin: 0 auto}.main{background: #fff; border-radius: 3px}.wrapper{padding: 20px}.content-block{padding-bottom: 10px; padding-top: 10px}.footer{margin-top: 10px}.footer a, .footer p, .footer span, .footer td{color: #999; font-size: 12px; text-align: center}h1, h2, h3, h4{color: #000; font-weight: 400; margin: 0 0 30px}h1{font-size: 35px; font-weight: 300; text-transform: capitalize}ol, p, ul{font-size: 14px; font-weight: 400; margin: 0 0 15px}ol li, p li, ul li{list-style-position: inside; margin-left: 5px}.link{color: #1e1b4b; text-decoration: underline}.btn>tbody>tr>td{padding-bottom: 15px}.btn table{width: auto}.btn table td{background-color: #fff; border-radius: 5px}.btn .link{background-color: #fff; border: 1px solid #1e1b4b; border-radius: 5px; color: #1e1b4b; cursor: pointer; display: inline-block; font-size: 14px; font-weight: 700; margin: 0; padding: 12px 25px}.btn-primary .link, .btn-primary table td{background-color: #1e1b4b}.btn-primary .link{border-color: #1e1b4b; color: #fff}.last, .mb0{margin-bottom: 0}.first, .mt0{margin-top: 0}.align-right{text-align: right}.align-left{text-align: left}.preheader{color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0}hr{border: 0; border-bottom: 1px solid #f6f6f6; margin: 20px 0}@media only screen and (max-width:620px){table.body h1{font-size: 28px !important; margin-bottom: 10px !important}table.body .link, table.body ol, table.body p, table.body span, table.body td, table.body ul{font-size: 16px !important}table.body .article, table.body .wrapper{padding: 10px !important}table.body .content{padding: 0 !important}table.body .container{padding: 0 !important; width: 100% !important}table.body .main{border-left-width: 0 !important; border-radius: 0 !important; border-right-width: 0 !important}table.body .btn .link, table.body .btn table{width: 100% !important}table.body .img-responsive{height: auto !important; max-width: 100% !important; width: auto !important}}@media all{.ExternalClass{width: 100%}.ExternalClass, .ExternalClass div, .ExternalClass font, .ExternalClass p, .ExternalClass span, .ExternalClass td{line-height: 100%}.apple-link .link{color: inherit !important; font-family: inherit !important; font-size: inherit !important; font-weight: inherit !important; line-height: inherit !important; text-decoration: none !important}#MessageViewBody .link{color: inherit; text-decoration: none; font-size: inherit; font-family: inherit; font-weight: inherit; line-height: inherit}.btn-primary table td:hover{background-color: #34495e !important}}</style> </head> <body> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""> <tr> <td>&nbsp;</td><td class=""container""> <div class=""content""> <table role=""presentation"" class=""main""> <tr> <td class=""wrapper""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td> <p>Merhaba{{User}},</p><p>{{Message}}</p><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""> <tbody> <tr> <td align=""left""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tbody> <tr> <td><a class=""link"" href=""{{Link}}"" target=""_blank"">Şifreni sıfırla</a></td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></table> </td></tr></table> <div class=""footer""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td class=""content-block""><span class=""apple-link"">Planor, Fatih İstanbul</span><br>Don't like these emails? <a href=""http://i.imgur.com/CScmqnj.gif"">Unsubscribe</a>. </td></tr></table> </div></div></td><td>&nbsp;</td></tr></table> </body></html>"
        });

        _context.MailTemplates.Add(new MailTemplate
        {
            Title = "Etkinlik yaklaşıyor",
            Slug = NotificationType.EventTime,
            TenantId = tenantId,
            Body =
                @"<!DOCTYPE html><html lang=""tr""><head><meta name=""viewport"" content=""width=device-width,initial-scale=1""><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""><title>{{Title}}</title><style>body,table td{font-size:14px}.body,body{background-color:#f6f6f6}.container,.content{display:block;max-width:580px;padding:10px}body,h1,h2,h3,h4{line-height:1.4;font-family:sans-serif}body,h1,h2,h3,h4,ol,p,table td,ul{font-family:sans-serif}.btn,.btn .link,.content,.wrapper{box-sizing:border-box}.align-center,.btn table td,.footer,h1{text-align:center}.clear,.footer{clear:both}.btn .link,.powered-by .link{text-decoration:none}body{-webkit-font-smoothing:antialiased;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}table{border-collapse:separate;mso-table-lspace:0;mso-table-rspace:0;width:100%}table td{vertical-align:top}.body{width:100%}.container{margin:0 auto!important;width:580px}.btn,.footer,.main{width:100%}.content{margin:0 auto}.main{background:#fff;border-radius:3px}.wrapper{padding:20px}.content-block{padding-bottom:10px;padding-top:10px}.footer{margin-top:10px}.footer a,.footer p,.footer span,.footer td{color:#999;font-size:12px;text-align:center}h1,h2,h3,h4{color:#000;font-weight:400;margin:0 0 30px}h1{font-size:35px;font-weight:300;text-transform:capitalize}ol,p,ul{font-size:14px;font-weight:400;margin:0 0 15px}ol li,p li,ul li{list-style-position:inside;margin-left:5px}.link{color:#1e1b4b;text-decoration:underline}.btn>tbody>tr>td{padding-bottom:15px}.btn table{width:auto}.btn table td{background-color:#fff;border-radius:5px}.btn .link{background-color:#fff;border:1px solid #1e1b4b;border-radius:5px;color:#1e1b4b;cursor:pointer;display:inline-block;font-size:14px;font-weight:700;margin:0;padding:12px 25px}.btn-primary .link,.btn-primary table td{background-color:#1e1b4b}.btn-primary .link{border-color:#1e1b4b;color:#fff}.last,.mb0{margin-bottom:0}.first,.mt0{margin-top:0}.align-right{text-align:right}.align-left{text-align:left}.preheader{color:transparent;display:none;height:0;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;visibility:hidden;width:0}hr{border:0;border-bottom:1px solid #f6f6f6;margin:20px 0}@media only screen and (max-width:620px){table.body h1{font-size:28px!important;margin-bottom:10px!important}table.body .link,table.body ol,table.body p,table.body span,table.body td,table.body ul{font-size:16px!important}table.body .article,table.body .wrapper{padding:10px!important}table.body .content{padding:0!important}table.body .container{padding:0!important;width:100%!important}table.body .main{border-left-width:0!important;border-radius:0!important;border-right-width:0!important}table.body .btn .link,table.body .btn table{width:100%!important}table.body .img-responsive{height:auto!important;max-width:100%!important;width:auto!important}}@media all{.ExternalClass{width:100%}.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td{line-height:100%}.apple-link .link{color:inherit!important;font-family:inherit!important;font-size:inherit!important;font-weight:inherit!important;line-height:inherit!important;text-decoration:none!important}#MessageViewBody .link{color:inherit;text-decoration:none;font-size:inherit;font-family:inherit;font-weight:inherit;line-height:inherit}.btn-primary table td:hover{background-color:#34495e!important}}</style></head><body><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""><tr><td>&nbsp;</td><td class=""container""><div class=""content""><table role=""presentation"" class=""main""><tr><td class=""wrapper""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td><p>Merhaba {{Name}},</p><p>{{Message}}</p><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""><tbody><tr><td align=""left""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tbody><tr><td><p class=""link"">Etkinliğe göz atmak için</p><a href=""{{ Link }}"" target=""_blank"" class=""link"">Giriş yap</a></td></tr></tbody></table></td></tr></tbody></table></td></tr></table></td></tr></table><div class=""footer""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td class=""content-block""><span class=""apple-link"">Planor, Karmaşık Süreçlerden Kurtul</span><br>Don't like these emails?<a href=""http://i.imgur.com/CScmqnj.gif"">Unsubscribe</a>.</td></tr></table></div></div></td><td>&nbsp;</td></tr></table></body></html>"
        });

        if (saveChanges)
        {
            await _context.SaveChangesAsync(default);
        }
    }
}