namespace Planor.Domain.Constants;

public abstract class Roles
{
    /// <summary>
    /// Temel roller.
    /// </summary>
    public static readonly string[] List = new[]
    {
        Admin, Manager, Customer, Employee, 
    };
    
    /// <summary>
    /// Endpoint üzerinden role ekleme sırasında validasyon için kullanılır.
    /// </summary>
    public static readonly string[] ExtendedList = new[]
    {
        TagCreate, TagRead, TagUpdate, TagDelete,
        MailTemplateRead, MailTemplateUpdate,
        CurrencyCreate, CurrencyDelete, CurrencyRead, CurrencyUpdate,
        CustomerCreate, CustomerRead, CustomerUpdate, CustomerDelete,
        ProjectCreate, ProjectDelete, ProjectRead, ProjectUpdate,
        DutyCreate, DutyDelete, DutyRead, DutyUpdate,
        EventCreate, EventDelete, EventRead, EventUpdate,
        FinanceCreate, FinanceDelete, FinanceRead, FinanceUpdate,
    };
    
    /// <summary>
    /// Manager kullanıcısı oluştururken otomatik atanan roller.
    /// </summary>
    public static readonly string[] ManagerRoles = new[]
    {
        Manager,
        TagCreate, TagRead, TagUpdate, TagDelete,
        MailTemplateRead, MailTemplateUpdate,
        CurrencyCreate, CurrencyDelete, CurrencyRead, CurrencyUpdate,
        CustomerCreate, CustomerRead, CustomerUpdate, CustomerDelete,
        ProjectCreate, ProjectDelete, ProjectRead, ProjectUpdate,
        DutyCreate, DutyDelete, DutyRead, DutyUpdate,
        EventCreate, EventDelete, EventRead, EventUpdate,
        FinanceCreate, FinanceDelete, FinanceRead, FinanceUpdate,
    };

    /// <summary>
    /// Customer kullanıcısı oluştururken otomatik atanan roller.
    /// </summary>
    public static readonly string[] CustomerRoles = new[]
    {
        Customer,
        TagRead, CurrencyRead, CustomerRead, ProjectRead, DutyRead, EventRead, FinanceRead
    };

    public static readonly string[] EmployeeRoles = new[]
    {
        Employee,
        TagCreate, TagRead, TagUpdate, TagDelete,
        MailTemplateRead, MailTemplateUpdate,
        CurrencyCreate, CurrencyDelete, CurrencyRead, CurrencyUpdate,
        CustomerCreate, CustomerRead, CustomerUpdate, CustomerDelete,
        ProjectCreate, ProjectDelete, ProjectRead, ProjectUpdate,
        DutyCreate, DutyDelete, DutyRead, DutyUpdate,
        EventCreate, EventDelete, EventRead, EventUpdate,
        FinanceCreate, FinanceDelete, FinanceRead, FinanceUpdate,
    };
    
    public const string Admin = nameof(Admin);
    public const string Manager = nameof(Manager);
    public const string Customer = nameof(Customer);
    public const string Employee = nameof(Employee);

    public const string TagCreate = nameof(TagCreate);
    public const string TagRead = nameof(TagRead);
    public const string TagUpdate = nameof(TagUpdate);
    public const string TagDelete = nameof(TagDelete);
    
    public const string MailTemplateCreate = nameof(MailTemplateCreate);
    public const string MailTemplateRead = nameof(MailTemplateRead);
    public const string MailTemplateUpdate = nameof(MailTemplateUpdate);
    public const string MailTemplateDelete = nameof(MailTemplateDelete);

    public const string CurrencyCreate = nameof(CurrencyCreate);
    public const string CurrencyRead = nameof(CurrencyRead);
    public const string CurrencyUpdate = nameof(CurrencyUpdate);
    public const string CurrencyDelete = nameof(CurrencyDelete);

    public const string CustomerCreate = nameof(CustomerCreate);
    public const string CustomerRead = nameof(CustomerRead);
    public const string CustomerUpdate = nameof(CustomerUpdate);
    public const string CustomerDelete = nameof(CustomerDelete);

    public const string ProjectCreate = nameof(ProjectCreate);
    public const string ProjectRead = nameof(ProjectRead);
    public const string ProjectUpdate = nameof(ProjectUpdate);
    public const string ProjectDelete = nameof(ProjectDelete);
    
    public const string DutyCreate = nameof(DutyCreate);
    public const string DutyRead = nameof(DutyRead);
    public const string DutyUpdate = nameof(DutyUpdate);
    public const string DutyDelete = nameof(DutyDelete);
    
    public const string EventCreate = nameof(EventCreate);
    public const string EventRead = nameof(EventRead);
    public const string EventUpdate = nameof(EventUpdate);
    public const string EventDelete = nameof(EventDelete);
    
    public const string FinanceCreate = nameof(FinanceCreate);
    public const string FinanceRead = nameof(FinanceRead);
    public const string FinanceUpdate = nameof(FinanceUpdate);
    public const string FinanceDelete = nameof(FinanceDelete);
}