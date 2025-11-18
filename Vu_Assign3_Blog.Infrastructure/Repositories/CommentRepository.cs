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
    public class CommentRepository: ICommentRepository
    {
        private readonly Assign3DbContext _context;
        public CommentRepository(Assign3DbContext context)
        {
            _context = context;
        }
        public async Task<Comment> CreateAsync(Comment comment)
        {
             _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Comments.AnyAsync(t => t.Id == id);
        }
        

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment?> UpdateAsync(Comment comment)
        {
            var existingComment = await _context.Comments.FindAsync(comment.Id);

            if (existingComment == null)
            {
                return null;
            }
            existingComment.PostId = comment.PostId;
            existingComment.Name = comment.Name;
            existingComment.Content = comment.Content;
            existingComment.Email = comment.Email;
        

            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}