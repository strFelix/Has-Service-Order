using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Dtos;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.CommentsRepository;
using OsDsII.api.Repository.ServiceOrderRepository;

namespace OsDsII.api.Services.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IMapper _mapper;

        public CommentsService(ICommentsRepository commentsRepository, IMapper mapper, IServiceOrderRepository serviceOrderRepository)
        {
            _commentsRepository = commentsRepository;
            _serviceOrderRepository = serviceOrderRepository;
            _mapper = mapper;
        }

        public async Task<Comment> AddCommentAsync(int serviceOrderId, CommentDto comment)
        {

            var commentMapped = _mapper.Map<Comment>(comment);
            var os = await _serviceOrderRepository.GetByIdAsync(serviceOrderId);

            if (os == null)
            {
                throw new NotFoundException("ServiceOrder not found.");
            }

            Comment commentExists = HandleCommentObject(os.Id, commentMapped.Description);

            return commentExists;
        }

        private Comment HandleCommentObject(int id, string description)
        {
            return new Comment
            {
                Description = description,
                ServiceOrderId = id
            };
        }
    }
}
