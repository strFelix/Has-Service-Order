using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;
using OsDsII.api.Repository.Comments;
using OsDsII.api.Repository.ServiceOrders;

namespace OsDsII.api.Controllers
{

    [ApiController]
    [Route("ServiceOrders/{id}/comment")]
    public class CommentController : ControllerBase
    {
        private readonly  IServiceOrderRepository _serviceOrderRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentController(IServiceOrderRepository serviceOrderRepository, ICommentRepository commentRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentsAsync(int serviceOrderId)
        {
            try
            {   //GetCommentByServiceOrderIdAsync
                ServiceOrder serviceOrderWithComments = await _serviceOrderRepository.GetCommentByServiceOrderIdAsync(serviceOrderId);
                return Ok(serviceOrderWithComments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment(int serviceOrderId, Comment comment)
        {
            try
            {
                //
                var os = await _serviceOrderRepository.GetServiceOrderCustomerAsync(serviceOrderId);

                if (os == null)
                {
                    return NotFound("ServiceOrder not found.");
                }

                Comment commentExists = HandleCommentObject(serviceOrderId, comment.Description);

                _commentRepository.AddCommentAsync(commentExists);

                return Ok(commentExists);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private Comment HandleCommentObject(int id, string description)
        {
            Comment comment = new Comment
            {
                Description = description,
                ServiceOrderId = id
            };
            return comment;
        }
    }
}

