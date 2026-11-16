using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Abs;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IServiceBusPublisher _serviceBusPublisher;
        public OrderController( IServiceBusPublisher serviceBusPublisher)
        {
            _serviceBusPublisher = serviceBusPublisher;
        }

        

        //[HttpGet(Name = "getorder")]
        //[HttpGet("{id}")]
        //[HttpGet("getorder/{id:int}")]

        [HttpGet(Name = "getorder")]
        public IActionResult GetOrder()
        {
            int id = 23232;
            var ordercreatedEvent = new OrderCreatedEvent()
            {
                OrderId = id,
                Status = "Created",
                Amount = 4000
            };

            //await _serviceBusPublisher.PublishOrderCreatedEventAsync(ordercreatedEvent);

            var objEntity = new OrderEntity
            {
                OrderId = id,
                Status = "Created",
                Amount = 5000
            };

            return Ok(objEntity);
        }
    }
}
