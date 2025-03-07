﻿using FinSharkMarket.utils;

namespace FinSharkMarket.Dtos.comments;

public class ResponseCommentDto
{
    
    public Guid Id { get; set; }
    public String Title { get; set; } = String.Empty;
    public String Content { get; set; } = String.Empty;
    // foreign key (stock id)
    public Guid StockId { get; set; }
    public String CrateBy { get; set; } = String.Empty;
    // date fields
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}