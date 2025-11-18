using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Vu_Assign3_Blog.Core;
using Vu_Assign3_Blog.Core.DTOs;
using Vu_Assign3_Blog.Core.Interfaces;
using Vu_Assign3_Blog.Core.Models;

namespace Vu_Assign3_Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _repository;
    public PostsController(IPostRepository repository)
    {
        _repository = repository;
    }
    // api/posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await _repository.GetAllAsync();
        return Ok(posts);
    }

    // GET: api/posts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        return Ok(post);
    }
    // POST: api/posts
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateDto
    postDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var post = new Post
        {
            Title = postDto.Title,
            Content = postDto.Content
        };
        var createdPost = await _repository.CreateAsync(post);
        return CreatedAtAction(
        nameof(GetPost),
        new { id = createdPost.Id },
        createdPost
        );
    }

}
