using Microsoft.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Vu_Assign3_Blog.Core;
using Vu_Assign3_Blog.Core.Interfaces;
using Vu_Assign3_Blog.Core.Models;

namespace Vu_Assign3_Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController: ControllerBase
{
    private readonly ICommentRepository _repository;
    public CommentsController(ICommentRepository repository)
    {
        _repository = repository;
    }
    // GET: api/comments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
    {
        var comments = await _repository.GetAllAsync();
        return Ok(comments);
    }
    // GET: api/comments/{id}

    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        var comment = await _repository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        return Ok(comment);
    }



}
