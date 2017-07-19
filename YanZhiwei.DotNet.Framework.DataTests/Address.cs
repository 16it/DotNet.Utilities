using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YanZhiwei.DotNet.Framework.Contract;

[Table("Person.Address")]
[Auditable]
[Serializable]
public class Address : ModelBase<int>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("AddressID")]
    [Required(ErrorMessage = "Address ID is required")]
    public override int ID
    {
        get;    // int, not null
        set;
    }
    
    [MaxLength(60)]
    [Required(ErrorMessage = "Address Line1 is required")]
    public string AddressLine1
    {
        get;    // nvarchar(60), not null
        set;
    }
    
    [MaxLength(60)]
    public string AddressLine2
    {
        get;    // nvarchar(60), null
        set;
    }
    
    [MaxLength(30)]
    [Required(ErrorMessage = "City is required")]
    public string City
    {
        get;    // nvarchar(30), not null
        set;
    }
    
    [Required(ErrorMessage = "State Province ID is required")]
    public int StateProvinceID
    {
        get;    // int, not null
        set;
    }
    
    [MaxLength(15)]
    [Required(ErrorMessage = "Postal Code is required")]
    public string PostalCode
    {
        get;    // nvarchar(15), not null
        set;
    }
    
    [Required(ErrorMessage = "rowguid is required")]
    public Guid rowguid
    {
        get;    // uniqueidentifier, not null
        set;
    }
    
    [Required(ErrorMessage = "Modified Date is required")]
    [Column("ModifiedDate")]
    public override DateTime CreateTime
    {
        get;    // datetime, not null
        set;
    }
}