using FinSharkMarket.Dtos.comments;
using FinSharkMarket.interfaces.comments;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.Mappers.comments;
using Microsoft.AspNetCore.Mvc;

namespace FinSharkMarket.Controllers;

[Route("api/comments")]
[ApiController]
public class CommentController: ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    
    public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var comments = await _commentRepository.GetAllCommentsAsync();
        var response = comments.Select(c => c.ToResponseCommentDto());
        return Ok(response);
    }
    
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var comment = await _commentRepository.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound("Comment not found");
        }
        var response = comment.ToResponseCommentDto();
        return Ok(response);
    }

    [HttpPost("{stockId:Guid}")]
    public async Task<IActionResult> CreateComment([FromRoute] Guid stockId, RequestCommentDto commentDto)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        bool stockExists = await _stockRepository.StockExists(stockId);
        if (!stockExists)
        {
            return BadRequest("Stock not found");
        }
        
        var comment = commentDto.ToComment(stockId);
        await _commentRepository.CreateCommentAsync(comment);
        
        return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment.ToResponseCommentDto());
    }

    [HttpPut("{commentId:Guid}")]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid commentId, UpdateRequestCommentDto updateDto)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var updatedComment = await _commentRepository.UpdateCommentAsync(updateDto.ToUpdateComment(), commentId);
        
        if (updatedComment == null)
        {
            return NotFound("Comment not found");
        }
        
        return Ok(updatedComment.ToResponseCommentDto());
    }
    
    [HttpDelete("{commentId:Guid}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var comment = await _commentRepository.DeleteCommentAsync(commentId);
        if (comment == null)
        {
            return NotFound("Comment not found");
        }
        
        return NoContent();
    }
}