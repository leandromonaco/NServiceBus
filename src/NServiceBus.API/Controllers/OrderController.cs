using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus.API.Queue;
using System.Threading.Tasks;

namespace NServiceBusAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IPublisher _publisher;

        public OrderController(ILogger<OrderController> logger, IPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        [HttpPost]
        public async Task Post(OrderSubmitted orderSubmitted)
        {
            await _publisher.PublishAsync(orderSubmitted);
            _logger.LogInformation("Order has been published.");
        }
    }
}
