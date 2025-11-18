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
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    public PostsController(IPostRepository postRepository,
                        ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    // api/posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await _postRepository.GetAllAsync();
        return Ok(posts);
    }

    // GET: api/posts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
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
        var createdPost = await _postRepository.CreateAsync(post);
        return CreatedAtAction(
        nameof(GetPost),
        new { id = createdPost.Id },
        createdPost
        );
    }

    // PUT: api/posts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] PostUpdateDto
    postUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var exists = await _postRepository.ExistsAsync(id);
        if (!exists)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        var post = new Post
        {
            Id = id,
            Title = postUpdateDto.Title,

        };
        post.UpdatedDate = DateTime.Now;
        var updatedPost = await _postRepository.UpdateAsync(post);
        return Ok(updatedPost);
    }
    // PATCH: api/posts/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchPost(int id, [FromBody]
JsonPatchDocument<Post> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest(new { message = "Patch document is null" });
        }
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        patchDoc.ApplyTo(post, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        post.UpdatedDate = DateTime.UtcNow;
        await _postRepository.UpdateAsync(post);
        return Ok(post);
    }

    // DELETE: api/posts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var deleted = await _postRepository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        return NoContent();
    }

    // GET: api/posts/{postId}/comments
    [HttpGet("{postId}/comments")]
    public async Task<ActionResult<List<Comment>>> GetCommentsByPostId(int postId)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {postId} not found" });
        }
        return Ok(post.Comments);
    }
    // POST: api/posts/{postId}/comments
    [HttpPost("{postId}/comments")]
    public async Task<ActionResult<Comment>> AddCommentToPostId(int postId,
                                    [FromBody] CommentCreateDto commentCreateDto)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {postId} not found" });
        }
        Comment comment = new Comment
        {
            Name = commentCreateDto.Name,
            Email = commentCreateDto.Email,
            Content = commentCreateDto.Content,
            PostId = post.Id
        };
        await _postRepository.UpdateAsync(post);
        await _commentRepository.UpdateAsync(comment);
        return Ok(comment);
    }

}
