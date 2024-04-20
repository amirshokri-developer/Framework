using ASh.Framework.EventBus.Abstractions;
using Meeting.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace Meeting.API.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class PublishController : ControllerBase
    {

        private readonly ILogger<PublishController> _logger;
        private readonly IProducer<ProductAdded> _productAddedProducer;

        public PublishController(ILogger<PublishController> logger,
            IProducer<ProductAdded> productAddedProducer)
        {
            _logger = logger;
            _productAddedProducer = productAddedProducer;
        }

        [HttpPost]
        public IActionResult ProductAdded()
        {
            _productAddedProducer.Produce(new ProductAdded(1, "product-1"));
            return Ok();
        }
    }
}
