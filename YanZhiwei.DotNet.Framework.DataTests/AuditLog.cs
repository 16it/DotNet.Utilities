using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YanZhiwei.DotNet.Framework.Contract;

[Table("AuditLog")]
public class AuditLog : ModelBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required(ErrorMessage = "ID is required")]
    public override int ID
    {
        get;    // int, not null
        set;
    }
    
    [MaxLength(100)]
    public string UserName
    {
        get;    // nvarchar(100), null
        set;
    }
    
    [MaxLength(100)]
    public string ModuleName
    {
        get;    // nvarchar(100), null
        set;
    }
    
    [MaxLength(100)]
    public string TableName
    {
        get;    // nvarchar(100), null
        set;
    }
    
    public int? ModelId
    {
        get;    // int, null
        set;
    }
    
    [MaxLength(50)]
    public string EventType
    {
        get;    // nvarchar(50), null
        set;
    }
    
    [MaxLength]
    public string NewValues
    {
        get;    // nvarchar(max), null
        set;
    }
    
    public override DateTime CreateTime
    {
        get;    // datetime, null
        set;
    }
}