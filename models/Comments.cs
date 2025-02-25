using System.ComponentModel.DataAnnotations.Schema;

namespace FinSharkMarket.models;

public class Comments
{
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }
    public String Title { get; set; } = String.Empty;
    public String Content { get; set; } = String.Empty;
    // foreign key (stock id)
    [Column(TypeName = "uuid")]
    public Guid StockId { get; set; }
    // navigation property
    public Stocks? Stock { get; set; }
    // date fields
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
}