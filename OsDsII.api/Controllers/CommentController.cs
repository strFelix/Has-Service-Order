using Microsoft.AspNetCore.Mvc;
using OsDsII.api.Dtos;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.CommentsRepository;
using OsDsII.api.Repository.ServiceOrderRepository;
using OsDsII.api.Services.Comments;
using OsDsII.api.Services.ServiceOrders;

namespace OsDsII.api.Controllers
{

    [ApiController]
    [Route("ServiceOrders/{id}/comment")]
    public class CommentController : ControllerBase
    {

        private ICommentsService _commentsService;
        private IServiceOrderService _serviceOrderService;

        public CommentController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentsAsync(int serviceOrderId)
        {
            try
            {
                ServiceOrderDto serviceOrderWithComments = await _serviceOrderService.GetServiceOrderWithComments(serviceOrderId);
                return Ok(serviceOrderWithComments);
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment(CommentDto comment, int serviceOrderId)
        {
            try
            {
                ServiceOrderDto os = await _serviceOrderService.GetServiceOrderFromUserAsync(serviceOrderId);
                Comment commentExists = await _commentsService.AddCommentAsync(serviceOrderId, comment);

                return Ok(commentExists);
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }
        }
    }
}

