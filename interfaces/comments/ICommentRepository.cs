using FinSharkMarket.models;

namespace FinSharkMarket.interfaces.comments;

public interface ICommentRepository
{
    Task<List<Comments>> GetAllComments();
    Task<Comments?> GetCommentById(Guid id);
}