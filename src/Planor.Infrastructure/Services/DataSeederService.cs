using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Apps.Commands;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Constants;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Services;

public class DataSeederService : IDataSeedService
{
    private readonly IApplicationDbContext _context;

    public DataSeederService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(string tenantId)
    {

        var app = new App
        {
            Name = tenantId,
            Code = GenerateCodeFromName(tenantId),
            TenantId = tenantId
        };

        _context.Apps.Add(app);
        
        var tags = new Tag[]
        {
            new Tag
            {
                Name = "Reklam ajansı",
                TenantId = tenantId,
                Slug = nameof(Customer)
            },
            new Tag
            {
                Name = "Yardım dernekleri",
                TenantId = tenantId,
                Slug = nameof(Customer)
            },
            new Tag
            {
                Name = "Yayınevi",
                TenantId = tenantId,
                Slug = nameof(Customer)
            },
            new Tag
            {
                Name = "Tekstil",
                TenantId = tenantId,
                Slug = nameof(Customer)
            },
            new Tag
            {
                Name = "Yazılım",
                TenantId = tenantId,
                Slug = nameof(Customer)
            }
        };

        _context.Tags.AddRange(tags);
        
        var mailTemplates = new MailTemplate[]
        {
            new MailTemplate
            {
                Title = "Hesabını onayla, Planor ile şirketini yönetmeye başla!",
                Slug = NotificationType.MailConfirmation,
                TenantId = tenantId,
                Body =
                    @"<!doctype html> <html lang=""tr""> <head> <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/> <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/> <title>{{Title}}</title> <style>body,table td{font-size:14px}.body,body{background-color:#f6f6f6}.container,.content{display:block;max-width:580px;padding:10px}body,h1,h2,h3,h4{line-height:1.4;font-family:sans-serif}body,h1,h2,h3,h4,ol,p,table td,ul{font-family:sans-serif}.btn,.btn .link,.content,.wrapper{box-sizing:border-box}.align-center,.btn table td,.footer,h1{text-align:center}.clear,.footer{clear:both}.btn .link,.powered-by .link{text-decoration:none}body{-webkit-font-smoothing:antialiased;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}table{border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%}table td{vertical-align:top}.body{width:100%}.container{margin:0 auto!important;width:580px}.btn,.footer,.main{width:100%}.content{margin:0 auto}.main{background:#fff;border-radius:3px}.wrapper{padding:20px}.content-block{padding-bottom:10px;padding-top:10px}.footer{margin-top:10px}.footer a,.footer p,.footer span,.footer td{color:#999;font-size:12px;text-align:center}h1,h2,h3,h4{color:#000;font-weight:400;margin:0 0 30px}h1{font-size:35px;font-weight:300;text-transform:capitalize}ol,p,ul{font-size:14px;font-weight:400;margin:0 0 15px}ol li,p li,ul li{list-style-position:inside;margin-left:5px}.link{color:#1e1b4b;text-decoration:underline}.btn>tbody>tr>td{padding-bottom:15px}.btn table{width:auto}.btn table td{background-color:#fff;border-radius:5px}.btn .link{background-color:#fff;border:1px solid #1e1b4b;border-radius:5px;color:#1e1b4b;cursor:pointer;display:inline-block;font-size:14px;font-weight:700;margin:0;padding:12px 25px}.btn-primary .link,.btn-primary table td{background-color:#1e1b4b}.btn-primary .link{border-color:#1e1b4b;color:#fff}.last,.mb0{margin-bottom:0}.first,.mt0{margin-top:0}.align-right{text-align:right}.align-left{text-align:left}.preheader{color:transparent;display:none;height:0;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;visibility:hidden;width:0}hr{border:0;border-bottom:1px solid #f6f6f6;margin:20px 0}@media only screen and (max-width:620px){table.body h1{font-size:28px!important;margin-bottom:10px!important}table.body .link,table.body ol,table.body p,table.body span,table.body td,table.body ul{font-size:16px!important}table.body .article,table.body .wrapper{padding:10px!important}table.body .content{padding:0!important}table.body .container{padding:0!important;width:100%!important}table.body .main{border-left-width:0!important;border-radius:0!important;border-right-width:0!important}table.body .btn .link,table.body .btn table{width:100%!important}table.body .img-responsive{height:auto!important;max-width:100%!important;width:auto!important}}@media all{.ExternalClass{width:100%}.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td{line-height:100%}.apple-link .link{color:inherit!important;font-family:inherit!important;font-size:inherit!important;font-weight:inherit!important;line-height:inherit!important;text-decoration:none!important}#MessageViewBody .link{color:inherit;text-decoration:none;font-size:inherit;font-family:inherit;font-weight:inherit;line-height:inherit}.btn-primary table td:hover{background-color:#34495e!important}}</style> </head> <body> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""> <tr> <td>&nbsp;</td><td class=""container""> <div class=""content""> <table role=""presentation"" class=""main""> <tr> <td class=""wrapper""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td> <p>Merhaba {{User}},</p><p>{{Message}}</p><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""> <tbody> <tr> <td align=""left""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tbody> <tr> <td> <a class=""link"">{{Code}}</a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></table> </td></tr></table> <div class=""footer""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td class=""content-block""> <span class=""apple-link"">Planor, Karmaşık Süreçlerden Kurtul</span> <br>Don't like these emails? <a href=""http://i.imgur.com/CScmqnj.gif"">Unsubscribe</a>. </td></tr></table> </div></div></td><td>&nbsp;</td></tr></table> </body> </html>"
            },
            new MailTemplate
            {
                Title = "Planor uygulamasına davet edildin.",
                Slug = NotificationType.InviteUser,
                TenantId = tenantId,
                Body =
                    @"<!DOCTYPE html><html lang=""tr""><head><meta name=""viewport"" content=""width=device-width,initial-scale=1""><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""><title>{{Title}}</title><style>body,table td{font-size:14px}.body,body{background-color:#f6f6f6}.container,.content{display:block;max-width:580px;padding:10px}body,h1,h2,h3,h4{line-height:1.4;font-family:sans-serif}body,h1,h2,h3,h4,ol,p,table td,ul{font-family:sans-serif}.btn,.btn .link,.content,.wrapper{box-sizing:border-box}.align-center,.btn table td,.footer,h1{text-align:center}.clear,.footer{clear:both}.btn .link,.powered-by .link{text-decoration:none}body{-webkit-font-smoothing:antialiased;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}table{border-collapse:separate;mso-table-lspace:0;mso-table-rspace:0;width:100%}table td{vertical-align:top}.body{width:100%}.container{margin:0 auto!important;width:580px}.btn,.footer,.main{width:100%}.content{margin:0 auto}.main{background:#fff;border-radius:3px}.wrapper{padding:20px}.content-block{padding-bottom:10px;padding-top:10px}.footer{margin-top:10px}.footer a,.footer p,.footer span,.footer td{color:#999;font-size:12px;text-align:center}h1,h2,h3,h4{color:#000;font-weight:400;margin:0 0 30px}h1{font-size:35px;font-weight:300;text-transform:capitalize}ol,p,ul{font-size:14px;font-weight:400;margin:0 0 15px}ol li,p li,ul li{list-style-position:inside;margin-left:5px}.link{color:#1e1b4b;text-decoration:underline}.btn>tbody>tr>td{padding-bottom:15px}.btn table{width:auto}.btn table td{background-color:#fff;border-radius:5px}.btn .link{background-color:#fff;border:1px solid #1e1b4b;border-radius:5px;color:#1e1b4b;cursor:pointer;display:inline-block;font-size:14px;font-weight:700;margin:0;padding:12px 25px}.btn-primary .link,.btn-primary table td{background-color:#1e1b4b}.btn-primary .link{border-color:#1e1b4b;color:#fff}.last,.mb0{margin-bottom:0}.first,.mt0{margin-top:0}.align-right{text-align:right}.align-left{text-align:left}.preheader{color:transparent;display:none;height:0;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;visibility:hidden;width:0}hr{border:0;border-bottom:1px solid #f6f6f6;margin:20px 0}@media only screen and (max-width:620px){table.body h1{font-size:28px!important;margin-bottom:10px!important}table.body .link,table.body ol,table.body p,table.body span,table.body td,table.body ul{font-size:16px!important}table.body .article,table.body .wrapper{padding:10px!important}table.body .content{padding:0!important}table.body .container{padding:0!important;width:100%!important}table.body .main{border-left-width:0!important;border-radius:0!important;border-right-width:0!important}table.body .btn .link,table.body .btn table{width:100%!important}table.body .img-responsive{height:auto!important;max-width:100%!important;width:auto!important}}@media all{.ExternalClass{width:100%}.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td{line-height:100%}.apple-link .link{color:inherit!important;font-family:inherit!important;font-size:inherit!important;font-weight:inherit!important;line-height:inherit!important;text-decoration:none!important}#MessageViewBody .link{color:inherit;text-decoration:none;font-size:inherit;font-family:inherit;font-weight:inherit;line-height:inherit}.btn-primary table td:hover{background-color:#34495e!important}}</style></head><body><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""><tr><td>&nbsp;</td><td class=""container""><div class=""content""><table role=""presentation"" class=""main""><tr><td class=""wrapper""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td><p>Merhaba {{Name}},</p><p>{{Message}}</p><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""><tbody><tr><td align=""left""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tbody><tr><td><p class=""link"">{{ Email }}</p><p class=""link"">{{ Password }}</p><a href=""{{ Link }}"" target=""_blank"" class=""link"">Giriş yap</a></td></tr></tbody></table></td></tr></tbody></table></td></tr></table></td></tr></table><div class=""footer""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td class=""content-block""><span class=""apple-link"">Planor, Karmaşık Süreçlerden Kurtul</span><br>Don't like these emails?<a href=""http://i.imgur.com/CScmqnj.gif"">Unsubscribe</a>.</td></tr></table></div></div></td><td>&nbsp;</td></tr></table></body></html>"
            },
            new MailTemplate
            {
                Title = "Planor şifreni sıfırla",
                Slug = NotificationType.ForgotMail,
                TenantId = tenantId,
                Body =
                    @"<!doctype html><html lang=""tr""> <head> <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/> <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/> <title>{{Title}}</title> <style>body, table td{font-size: 14px}.body, body{background-color: #f6f6f6}.container, .content{display: block; max-width: 580px; padding: 10px}body, h1, h2, h3, h4{line-height: 1.4; font-family: sans-serif}body, h1, h2, h3, h4, ol, p, table td, ul{font-family: sans-serif}.btn, .btn .link, .content, .wrapper{box-sizing: border-box}.align-center, .btn table td, .footer, h1{text-align: center}.clear, .footer{clear: both}.btn .link, .powered-by .link{text-decoration: none}body{-webkit-font-smoothing: antialiased; margin: 0; padding: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%}table{border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%}table td{vertical-align: top}.body{width: 100%}.container{margin: 0 auto !important; width: 580px}.btn, .footer, .main{width: 100%}.content{margin: 0 auto}.main{background: #fff; border-radius: 3px}.wrapper{padding: 20px}.content-block{padding-bottom: 10px; padding-top: 10px}.footer{margin-top: 10px}.footer a, .footer p, .footer span, .footer td{color: #999; font-size: 12px; text-align: center}h1, h2, h3, h4{color: #000; font-weight: 400; margin: 0 0 30px}h1{font-size: 35px; font-weight: 300; text-transform: capitalize}ol, p, ul{font-size: 14px; font-weight: 400; margin: 0 0 15px}ol li, p li, ul li{list-style-position: inside; margin-left: 5px}.link{color: #1e1b4b; text-decoration: underline}.btn>tbody>tr>td{padding-bottom: 15px}.btn table{width: auto}.btn table td{background-color: #fff; border-radius: 5px}.btn .link{background-color: #fff; border: 1px solid #1e1b4b; border-radius: 5px; color: #1e1b4b; cursor: pointer; display: inline-block; font-size: 14px; font-weight: 700; margin: 0; padding: 12px 25px}.btn-primary .link, .btn-primary table td{background-color: #1e1b4b}.btn-primary .link{border-color: #1e1b4b; color: #fff}.last, .mb0{margin-bottom: 0}.first, .mt0{margin-top: 0}.align-right{text-align: right}.align-left{text-align: left}.preheader{color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0}hr{border: 0; border-bottom: 1px solid #f6f6f6; margin: 20px 0}@media only screen and (max-width:620px){table.body h1{font-size: 28px !important; margin-bottom: 10px !important}table.body .link, table.body ol, table.body p, table.body span, table.body td, table.body ul{font-size: 16px !important}table.body .article, table.body .wrapper{padding: 10px !important}table.body .content{padding: 0 !important}table.body .container{padding: 0 !important; width: 100% !important}table.body .main{border-left-width: 0 !important; border-radius: 0 !important; border-right-width: 0 !important}table.body .btn .link, table.body .btn table{width: 100% !important}table.body .img-responsive{height: auto !important; max-width: 100% !important; width: auto !important}}@media all{.ExternalClass{width: 100%}.ExternalClass, .ExternalClass div, .ExternalClass font, .ExternalClass p, .ExternalClass span, .ExternalClass td{line-height: 100%}.apple-link .link{color: inherit !important; font-family: inherit !important; font-size: inherit !important; font-weight: inherit !important; line-height: inherit !important; text-decoration: none !important}#MessageViewBody .link{color: inherit; text-decoration: none; font-size: inherit; font-family: inherit; font-weight: inherit; line-height: inherit}.btn-primary table td:hover{background-color: #34495e !important}}</style> </head> <body> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""> <tr> <td>&nbsp;</td><td class=""container""> <div class=""content""> <table role=""presentation"" class=""main""> <tr> <td class=""wrapper""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td> <p>Merhaba {{User}},</p><p>{{Message}}</p><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""> <tbody> <tr> <td align=""left""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tbody> <tr> <td><a class=""link"" href=""{{Link}}"" target=""_blank"">Şifreni sıfırla</a></td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></table> </td></tr></table> <div class=""footer""> <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td class=""content-block""><span class=""apple-link"">Planor, Fatih İstanbul</span><br>Don't like these emails? <a href=""http://i.imgur.com/CScmqnj.gif"">Unsubscribe</a>. </td></tr></table> </div></div></td><td>&nbsp;</td></tr></table> </body></html>"
            },
            new MailTemplate
            {
                Title = "Etkinlik yaklaşıyor",
                Slug = NotificationType.EventTime,
                TenantId = tenantId,
                Body = @"<!DOCTYPE html><html lang=""tr""><head><meta name=""viewport"" content=""width=device-width,initial-scale=1""><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""><title>{{Title}}</title><style>body,table td{font-size:14px}.body,body{background-color:#f6f6f6}.container,.content{display:block;max-width:580px;padding:10px}body,h1,h2,h3,h4{line-height:1.4;font-family:sans-serif}body,h1,h2,h3,h4,ol,p,table td,ul{font-family:sans-serif}.btn,.btn .link,.content,.wrapper{box-sizing:border-box}.align-center,.btn table td,.footer,h1{text-align:center}.clear,.footer{clear:both}.btn .link,.powered-by .link{text-decoration:none}body{-webkit-font-smoothing:antialiased;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}table{border-collapse:separate;mso-table-lspace:0;mso-table-rspace:0;width:100%}table td{vertical-align:top}.body{width:100%}.container{margin:0 auto!important;width:580px}.btn,.footer,.main{width:100%}.content{margin:0 auto}.main{background:#fff;border-radius:3px}.wrapper{padding:20px}.content-block{padding-bottom:10px;padding-top:10px}.footer{margin-top:10px}.footer a,.footer p,.footer span,.footer td{color:#999;font-size:12px;text-align:center}h1,h2,h3,h4{color:#000;font-weight:400;margin:0 0 30px}h1{font-size:35px;font-weight:300;text-transform:capitalize}ol,p,ul{font-size:14px;font-weight:400;margin:0 0 15px}ol li,p li,ul li{list-style-position:inside;margin-left:5px}.link{color:#1e1b4b;text-decoration:underline}.btn>tbody>tr>td{padding-bottom:15px}.btn table{width:auto}.btn table td{background-color:#fff;border-radius:5px}.btn .link{background-color:#fff;border:1px solid #1e1b4b;border-radius:5px;color:#1e1b4b;cursor:pointer;display:inline-block;font-size:14px;font-weight:700;margin:0;padding:12px 25px}.btn-primary .link,.btn-primary table td{background-color:#1e1b4b}.btn-primary .link{border-color:#1e1b4b;color:#fff}.last,.mb0{margin-bottom:0}.first,.mt0{margin-top:0}.align-right{text-align:right}.align-left{text-align:left}.preheader{color:transparent;display:none;height:0;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;visibility:hidden;width:0}hr{border:0;border-bottom:1px solid #f6f6f6;margin:20px 0}@media only screen and (max-width:620px){table.body h1{font-size:28px!important;margin-bottom:10px!important}table.body .link,table.body ol,table.body p,table.body span,table.body td,table.body ul{font-size:16px!important}table.body .article,table.body .wrapper{padding:10px!important}table.body .content{padding:0!important}table.body .container{padding:0!important;width:100%!important}table.body .main{border-left-width:0!important;border-radius:0!important;border-right-width:0!important}table.body .btn .link,table.body .btn table{width:100%!important}table.body .img-responsive{height:auto!important;max-width:100%!important;width:auto!important}}@media all{.ExternalClass{width:100%}.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td{line-height:100%}.apple-link .link{color:inherit!important;font-family:inherit!important;font-size:inherit!important;font-weight:inherit!important;line-height:inherit!important;text-decoration:none!important}#MessageViewBody .link{color:inherit;text-decoration:none;font-size:inherit;font-family:inherit;font-weight:inherit;line-height:inherit}.btn-primary table td:hover{background-color:#34495e!important}}</style></head><body><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""><tr><td>&nbsp;</td><td class=""container""><div class=""content""><table role=""presentation"" class=""main""><tr><td class=""wrapper""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td><p>Merhaba {{Name}},</p><p>{{Message}}</p><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""><tbody><tr><td align=""left""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tbody><tr><td><p class=""link"">Etkinliğe göz atmak için</p><a href=""{{ Link }}"" target=""_blank"" class=""link"">Giriş yap</a></td></tr></tbody></table></td></tr></tbody></table></td></tr></table></td></tr></table><div class=""footer""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td class=""content-block""><span class=""apple-link"">Planor, Karmaşık Süreçlerden Kurtul</span><br>Don't like these emails?<a href=""http://i.imgur.com/CScmqnj.gif"">Unsubscribe</a>.</td></tr></table></div></div></td><td>&nbsp;</td></tr></table></body></html>"
            }

        };
        
        _context.MailTemplates.AddRange(mailTemplates);
        
        var currencies = new Currency[]
        {
            new Currency
            {
                Code = "TRY",
                Symbol = "₺",
                Rate = 0.037,
                TenantId = tenantId,
                IsDefault = true
            },
            new Currency
            {
                Code = "USD",
                Symbol = "$",
                Rate = 1,
                TenantId = tenantId
            },

            new Currency
            {
                Code = "EUR",
                Symbol = "€",
                Rate = 1.10,
                TenantId = tenantId
            }
        };
        _context.Currencies.AddRange(currencies);
        
        var customers = new Customer[]
        {
            new Customer
            {
                Name = "Deneme şirketi",
                IsCompany = true,
                Address = "Karagümrük mah.",
                City = "İstanbul",
                District = "Fatih",
                PostCode = "34000",
                Country = "Türkiye",
                GovernmentId = "1111111111",
                IsPotantial = true,
                Currency = currencies[1],
                TenantId = tenantId
            }
        };
        _context.Customers.AddRange(customers);
        
        var projects = new Project[]
        {
            new Project
            {
                Title = "Deneme projesi",
                IsCompleted = false,
                Customer = customers[0],
                Description = "Uzun bir açıklama",
                Price = 200.40,
                TenantId = tenantId
            }
        };
        _context.Projects.AddRange(projects);
        
        var dutySizes = new DutySize[]
        {
            new DutySize
            {
                Name = "Minik",
                Score = 1,
                TenantId = tenantId
            },
            new DutySize
            {
                Name = "Küçük",
                Score = 2,
                TenantId = tenantId
            },
            new DutySize
            {
                Name = "Orta",
                Score = 3,
                TenantId = tenantId
            },
            new DutySize
            {
                Name = "Büyük",
                Score = 4,
                TenantId = tenantId
            },
            new DutySize
            {
                Name = "Çok büyük",
                Score = 5,
                TenantId = tenantId
            },

        };
        _context.DutySizes.AddRange(dutySizes);
        
        var dutyPriorities = new DutyPriority[]
        {
            new DutyPriority
            {
                Name = "Düşük",
                Score = 1,
                TenantId = tenantId
            },
            new DutyPriority
            {
                Name = "Orta",
                Score = 2,
                TenantId = tenantId
            },
            new DutyPriority
            {
                Name = "Yüksek",
                Score = 3,
                TenantId = tenantId
            },
            new DutyPriority
            {
                Name = "Kritik",
                Score = 4,
                TenantId = tenantId
            }
        };
        _context.DutyPriorities.AddRange(dutyPriorities);
        
        var dutyCategories = new DutyCategory[]
        {
            new DutyCategory
            {
                Title = "İş Girişi",
                TenantId = tenantId
            },
            new DutyCategory
            {
                Title = "Yapılıyor",
                TenantId = tenantId
            },
            new DutyCategory
            {
                Title = "Test",
                TenantId = tenantId
            },
            new DutyCategory
            {
                Title = "Tamamlandı 💯",
                TenantId = tenantId
            }
        };
        _context.DutyCategories.AddRange(dutyCategories);
        
        var duties = new Duty[]
        {
            new Duty
            {
                Title = "Deneme görevi",
                Description = "Deneme açıklaması",
                Size = dutySizes[0],
                Priority = dutyPriorities[1],
                Category = dutyCategories[1],
                Project = projects[0],
                TenantId = tenantId,
                Order = 1,
                Todos = new List<DutyTodo>
                {
                    new DutyTodo
                    {
                        Content = "Bu görev yapılcak",
                        IsCompleted = true
                    },
                    new DutyTodo
                    {
                        Content = "Şu görev yapılcak",
                        IsCompleted = false
                    }
                }
            }
        };
        _context.Duties.AddRange(duties);

        
        var financeCategories = new FinanceCategory[]
        {
          new FinanceCategory()
          {
              Name = "Gelir",
              TenantId = tenantId,
          },
          new FinanceCategory()
          {
              Name = "Gider",
              TenantId = tenantId,
          },
          new FinanceCategory()
          {
              Name = "Fatura",
              TenantId = tenantId,
          },
          new FinanceCategory()
          {
              Name = "Maaş",
              TenantId = tenantId,
          },
          new FinanceCategory()
          {
              Name = "Kira",
              TenantId = tenantId,
          },
        };
        _context.FinanceCategories.AddRange(financeCategories);
        
        await _context.SaveChangesAsync(default);
    }
    
    private string GenerateCodeFromName(string name)
    {
        string result;
        
        if (name.Length == 3)
        {
            result = name;
        }
        else if (name.Length < 3)
        {
            result = "PLN";
        }
        else
        {
            result = name.Substring(0, 3);
        }

        var nonAlphaNumericRegex = new Regex("[^a-zA-Z0-9 -]");
        result = nonAlphaNumericRegex.Replace(result, "");
        
        return result.ToUpper();
    }

}