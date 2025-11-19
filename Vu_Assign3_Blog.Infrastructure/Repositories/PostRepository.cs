using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vu_Assign3_Blog.Core.Interfaces;
using Vu_Assign3_Blog.Core.Models;
using Vu_Assign3_Blog.Infrastructure.Data;

namespace Vu_Assign3_Blog.Infrastructure.Repositories
{
    public class PostRepository: IPostRepository
    {
        private readonly Assign3DbContext _context;
        public PostRepository(Assign3DbContext context)
        {
            _context = context;
        }
        public async Task<Post> CreateAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return false;
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Posts.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts.Include(p=>p.Comments).FirstOrDefaultAsync(p=> p.Id == id);
        }

        public async Task<Post?> UpdateAsync(Post post)
        {
            var existingPost = await _context.Posts.FindAsync(post.Id);

            if (existingPost == null)
            {
                return null;
            }
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.Author = post.Author;
            existingPost.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingPost;
        }
        
    }
}