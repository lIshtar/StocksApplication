using StocksApplication.Dtos.Comment;
using StocksApplication.Models;

namespace StocksApplication.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllAsync();
        public Task<Comment?> GetByIdAsync(int id);
        public Task<Comment> CreateAsync(Comment commentModel);
        public Task<Comment?> UpdateAsync(UpdateCommentRequestDto commentDto, int id);
        public Task<Comment?> DeleteAsync(int id);
    }
}
