using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vu_Assign3_Blog.Core.Models;

namespace Vu_Assign3_Blog.Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(Comment comment);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}