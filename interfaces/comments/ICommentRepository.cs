﻿using FinSharkMarket.models;

namespace FinSharkMarket.interfaces.comments;

public interface ICommentRepository
{
    Task<List<Comments>> GetAllCommentsAsync();
    Task<Comments?> GetCommentByIdAsync(Guid id);
    Task<Comments> CreateCommentAsync(Comments comment);
    Task<Comments?> UpdateCommentAsync(Comments comment, Guid id);
    Task<Comments?>  DeleteCommentAsync(Guid id);
}