using System.ComponentModel.DataAnnotations.Schema;

namespace FinSharkMarket.models;

[Table("PortFolio")]
public class PortFolio
{
    // appUser Id is in the AppUser table and the type is Text
    public String AppUserId { get; set; }
    
    [Column(TypeName = "uuid")]
    public Guid StockId { get; set; }
    
    public AppUser AppUser { get; set; }
    public Stocks Stock { get; set; }
}