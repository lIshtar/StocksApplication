using Microsoft.EntityFrameworkCore;
using StocksApplication.Data;
using StocksApplication.Dtos.Comment;
using StocksApplication.Interfaces;
using StocksApplication.Mappers;
using StocksApplication.Models;
using System.Runtime.CompilerServices;

namespace StocksApplication.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbContext.Comments
                .Include(a => a.AppUser)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContext.Comments
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _dbContext.Comments.AddAsync(commentModel);
            await _dbContext.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> UpdateAsync(UpdateCommentRequestDto commentDto, int id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null)
            {
                return null;
            }

            comment = commentDto.ToCommentFromUpdateDto(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null)
            {
                return null;
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }
    }
}
