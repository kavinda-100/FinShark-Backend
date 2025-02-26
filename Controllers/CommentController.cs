using FinSharkMarket.interfaces.comments;
using FinSharkMarket.Mappers.comments;
using Microsoft.AspNetCore.Mvc;

namespace FinSharkMarket.Controllers;

[Route("api/comments")]
[ApiController]
public class CommentController: ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    
    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var comments = await _commentRepository.GetAllComments();
        var response = comments.Select(c => c.ToResponseCommentDto());
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id)
    {
        var comment = await _commentRepository.GetCommentById(id);
        if (comment == null)
        {
            return NotFound("Comment not found");
        }
        var response = comment.ToResponseCommentDto();
        return Ok(response);
    }
}