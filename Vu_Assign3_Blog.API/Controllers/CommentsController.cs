using Microsoft.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Vu_Assign3_Blog.Core;
using Vu_Assign3_Blog.Core.DTOs;
using Vu_Assign3_Blog.Core.Interfaces;
using Vu_Assign3_Blog.Core.Models;

namespace Vu_Assign3_Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _repository;
    public CommentsController(ICommentRepository repository)
    {
        _repository = repository;
    }
    // GET: api/comments
    [HttpGet]
    public async Task<ActionResult<List<CommentReturnDto>>> GetAllComments()
    {
        var comments = await _repository.GetAllAsync();
        List<CommentReturnDto> commentReturnDto = new List<CommentReturnDto>();
        foreach (Comment comment in comments)
        {
            commentReturnDto.Add(new CommentReturnDto
            {
                Name = comment.Name,
                Email = comment.Email,
                Content = comment.Content,
                PostId = comment.PostId,
                Id = comment.Id
            });
        }

        return Ok(commentReturnDto);
    }
    // GET: api/comments/{id}

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentReturnDto>> GetComment(int id)
    {
        var comment = await _repository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        CommentReturnDto commentReturnDto = new CommentReturnDto
        {
            Name = comment.Name,
            Email = comment.Email,
            Content = comment.Content,
            PostId = comment.PostId,
            Id = comment.Id
        };
        return Ok(commentReturnDto);
    }

    // PUT: api/comments/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateDto
    commentUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var exists = await _repository.ExistsAsync(id);
        if (!exists)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        var comment = new Comment
        {
            Id = id,
            PostId = commentUpdateDto.PostId,
            Name = commentUpdateDto.Name,
            Email = commentUpdateDto.Email,
            Content = commentUpdateDto.Content

        };
        var updatedComment = await _repository.UpdateAsync(comment);
        CommentReturnDto? commentReturnDto = null;
        if(updatedComment != null)
        {
            commentReturnDto = new CommentReturnDto
        {
            Name = updatedComment.Name,
            Email = updatedComment.Email,
            Content = updatedComment.Content,
            PostId = updatedComment.PostId,
            Id = updatedComment.Id
        };
        }
        

        return Ok(commentReturnDto);
    }
    // PATCH: api/comments/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchComment(int id, [FromBody]
JsonPatchDocument<Comment> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest(new { message = "Patch document is null" });
        }
        var comment = await _repository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        patchDoc.ApplyTo(comment, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var patchedComment = await _repository.UpdateAsync(comment);
        CommentReturnDto? commentReturnDto = null;
        if(patchedComment != null)
        {
            commentReturnDto = new CommentReturnDto
        {
            Name = patchedComment.Name,
            Email = patchedComment.Email,
            Content = patchedComment.Content,
            PostId = patchedComment.PostId,
            Id = patchedComment.Id
        };
        }
        return Ok(commentReturnDto);
    }
    // DELETE: api/comments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        return NoContent();
    }

}
