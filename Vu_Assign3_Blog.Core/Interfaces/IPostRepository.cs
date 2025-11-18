using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vu_Assign3_Blog.Core.Models;

namespace Vu_Assign3_Blog.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> CreateAsync(Post post);
        Task<Post?> UpdateAsync(Post post);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}