using FinSharkMarket.Dtos.comments;
using FinSharkMarket.models;

namespace FinSharkMarket.Mappers.comments;

public static class CommentMapper
{
    // Response Type
    public static ResponseCommentDto ToResponseCommentDto(this Comments comment)
    {
        return new ResponseCommentDto
        {
            Id = comment.Id,
            Title = comment.Title,
            Content = comment.Content,
            StockId = comment.StockId,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt
        };
    }
    
    // Request Type
    public static Comments ToComment(this RequestCommentDto commentDto, Guid stockId)
    {
        return new Comments
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId
        };
    }
}