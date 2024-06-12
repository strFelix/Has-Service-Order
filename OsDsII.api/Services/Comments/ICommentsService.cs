using OsDsII.api.Dtos;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Models;

namespace OsDsII.api.Services.Comments
{
    public interface ICommentsService
    {
        public Task<Comment> AddCommentAsync(int serviceOrderId, CommentDto comment);
    }
}
