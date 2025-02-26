using FinSharkMarket.data;
using FinSharkMarket.interfaces.comments;
using FinSharkMarket.models;
using Microsoft.EntityFrameworkCore;

namespace FinSharkMarket.Repository.comments;

public class CommentRepository: ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Comments>> GetAllComments()
    {
        var comments = await _context.Comments.ToListAsync();
        return comments;
    }

    public async Task<Comments?> GetCommentById(Guid id)
    {
        var comment = await _context.Comments.FindAsync(id);
        
        return comment;
    }
}