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
    
    public async Task<List<Comments>> GetAllCommentsAsync()
    {
        var comments = await _context.Comments.ToListAsync();
        return comments;
    }

    public async Task<Comments?> GetCommentByIdAsync(Guid id)
    {
        var comment = await _context.Comments.FindAsync(id);
        return comment;
    }

    public async Task<Comments> CreateCommentAsync(Comments comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comments?> UpdateCommentAsync(Comments comment, Guid id)
    {
        var existingComment = await _context.Comments.FindAsync(id);
        if (existingComment == null)
        {
            return null;
        }
        
        existingComment.Title = comment.Title;
        existingComment.Content = comment.Content;
        
        _context.Comments.Update(existingComment);
        await _context.SaveChangesAsync();
        
        return existingComment;
    }

    public async Task<Comments?> DeleteCommentAsync(Guid id)
    {
        var existingComment = await _context.Comments.FindAsync(id);
        if (existingComment == null)
        {
            return null;
        }
        
        _context.Comments.Remove(existingComment);
        await _context.SaveChangesAsync();
        
        return existingComment;
    }
}