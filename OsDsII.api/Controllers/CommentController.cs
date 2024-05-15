using Microsoft.AspNetCore.Mvc;
using OsDsII.api.Models;
using OsDsII.api.Repository.CommentsRepository;
using OsDsII.api.Repository.ServiceOrderRepository;

namespace OsDsII.api.Controllers
{

    [ApiController]
    [Route("ServiceOrders/{id}/comment")]
    public class CommentController : ControllerBase
    {
        //private readonly DataContext _context;
        private readonly IServiceOrderRepository _serviceOrderRepository; // IOC (INVERSION OF CONTROL)
        private readonly ICommentsRepository _commentsRepository;


        public CommentController(IServiceOrderRepository serviceOrderRepository, ICommentsRepository commentsRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _commentsRepository = commentsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsAsync(int serviceOrderId)
        {
            ServiceOrder serviceOrderWithComments = await _serviceOrderRepository.GetServiceOrderWithComments(serviceOrderId);
            return Ok(serviceOrderWithComments);

        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int serviceOrderId, Comment comment)
        {
            try
            {
                ServiceOrder os = await _serviceOrderRepository.GetServiceOrderFromUser(serviceOrderId);

                if (os == null)
                {
                    throw new Exception("Service Order not found.");
                }

                Comment commentExists = HandleCommentObject(serviceOrderId, comment.Description);

                await _commentsRepository.AddCommentAsync(commentExists); // This line adds the comment to the context

                return Ok(commentExists);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

