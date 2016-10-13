using System;

public class Product
{
    public Product()
    {
        this.MakeFlag = true;
        this.FinishedGoodsFlag = true;
        this.rowguid = Guid.NewGuid();
        this.ModifiedDate = DateTime.Now;
    }
    
    public int ProductID
    {
        get;    // int, not null
        set;
    }
    
    public string Name
    {
        get;    // nvarchar(50), not null
        set;
    }
    
    public string ProductNumber
    {
        get;    // nvarchar(25), not null
        set;
    }
    
    public bool MakeFlag
    {
        get;    // bit, not null
        set;
    }
    
    public bool FinishedGoodsFlag
    {
        get;    // bit, not null
        set;
    }
    
    public string Color
    {
        get;    // nvarchar(15), null
        set;
    }
    
    public short SafetyStockLevel
    {
        get;    // smallint, not null
        set;
    }
    
    public short ReorderPoint
    {
        get;    // smallint, not null
        set;
    }
    
    public decimal StandardCost
    {
        get;    // money, not null
        set;
    }
    
    public decimal ListPrice
    {
        get;    // money, not null
        set;
    }
    
    public string Size
    {
        get;    // nvarchar(5), null
        set;
    }
    
    public string SizeUnitMeasureCode
    {
        get;    // nchar(3), null
        set;
    }
    
    public string WeightUnitMeasureCode
    {
        get;    // nchar(3), null
        set;
    }
    
    public decimal? Weight
    {
        get;    // decimal(8,2), null
        set;
    }
    
    public int DaysToManufacture
    {
        get;    // int, not null
        set;
    }
    
    public string ProductLine
    {
        get;    // nchar(2), null
        set;
    }
    
    public string Class
    {
        get;    // nchar(2), null
        set;
    }
    
    public string Style
    {
        get;    // nchar(2), null
        set;
    }
    
    public int? ProductSubcategoryID
    {
        get;    // int, null
        set;
    }
    
    public int? ProductModelID
    {
        get;    // int, null
        set;
    }
    
    public DateTime SellStartDate
    {
        get;    // datetime, not null
        set;
    }
    
    public DateTime? SellEndDate
    {
        get;    // datetime, null
        set;
    }
    
    public DateTime? DiscontinuedDate
    {
        get;    // datetime, null
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
