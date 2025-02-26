﻿using System.ComponentModel.DataAnnotations.Schema;
using FinSharkMarket.utils;

namespace FinSharkMarket.models;

public class Comments
{
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }
    [Column(TypeName = "varchar(255)")]
    public String Title { get; set; } = String.Empty;
    [Column(TypeName = "varchar(255)")]
    public String Content { get; set; } = String.Empty;
    // foreign key (stock id)
    [Column(TypeName = "uuid")]
    public Guid StockId { get; set; }
    // navigation property
    public Stocks? Stock { get; set; }
    // date fields
    public DateTime CreatedAt { get; set; } = DateTimeUtils.ToUtc(DateTime.Now);
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.ToUtc(DateTime.Now);
    
}