using System;

public class Address
{
    public Address()
    {
        this.rowguid = Guid.NewGuid();
        this.ModifiedDate = DateTime.Now;
    }
    
    public int AddressID
    {
        get;    // int, not null
        set;
    }
    
    public string AddressLine1
    {
        get;    // nvarchar(60), not null
        set;
    }
    
    public string AddressLine2
    {
        get;    // nvarchar(60), null
        set;
    }
    
    public string City
    {
        get;    // nvarchar(30), not null
        set;
    }
    
    public int StateProvinceID
    {
        get;    // int, not null
        set;
    }
    
    public string PostalCode
    {
        get;    // nvarchar(15), not null
        set;
    }
    
    public Guid rowguid
    {
        get;    // uniqueidentifier, not null
        set;
    }
    
    public DateTime ModifiedDate
    {
        get;    // datetime, not null
        set;
    }
}