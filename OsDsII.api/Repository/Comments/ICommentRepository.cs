using OsDsII.api.Models;

namespace OsDsII.api.Repository.Comments
{
    public interface ICommentRepository
    {
        public Task AddCommentAsync(Comment commentExists);
    }
}
