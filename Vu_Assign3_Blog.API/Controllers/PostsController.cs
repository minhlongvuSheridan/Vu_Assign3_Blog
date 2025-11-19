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
    public async Task<ActionResult<List<PostReturnDto>>> GetAllPosts()
    {
        var posts = await _postRepository.GetAllAsync();

        List<PostReturnDto> postReturnDto = new List<PostReturnDto>();
        foreach(Post post in posts)
        {
            postReturnDto.Add(new PostReturnDto
            {
               Id = post.Id,
               Title = post.Title,
               Content = post.Content,
               Author = post.Author,
               CreatedDate = post.CreatedDate,
               UpdatedDate = post.UpdatedDate, 
            });
        }
        return Ok(postReturnDto);
    }

    // GET: api/posts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PostReturnDto>> GetPost(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        PostReturnDto postReturnDto = new PostReturnDto
            {
               Id = post.Id,
               Title = post.Title,
               Content = post.Content,
               Author = post.Author,
               CreatedDate = post.CreatedDate,
               UpdatedDate = post.UpdatedDate, 
            };

        return Ok(postReturnDto);
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
            Content = postUpdateDto.Content,
            Author = postUpdateDto.Author

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
    public async Task<ActionResult<List<CommentReturnDto>>> GetCommentsByPostId(int postId)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {postId} not found" });
        }
        List<CommentReturnDto> commentReturnDto = new List<CommentReturnDto>();
        foreach (Comment comment in post.Comments)
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
    // POST: api/posts/{postId}/comments
    [HttpPost("{postId}/comments")]
    public async Task<ActionResult<CommentReturnDto>> AddCommentToPostId(int postId,
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
            PostId = postId
        };
        await _commentRepository.CreateAsync(comment);

        CommentReturnDto commentReturnDto = new CommentReturnDto
        {
            Name = comment.Name,
            Email = comment.Email,
            Content = comment.Content,
            PostId = comment.PostId,
            Id = comment.Id
        };
        return CreatedAtAction(
        nameof(GetPost),
        new { id = commentReturnDto.Id },
        commentReturnDto
        );
    }

}
