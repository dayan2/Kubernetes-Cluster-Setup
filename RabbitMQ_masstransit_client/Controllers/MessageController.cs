using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ_masstransit.Models.TodoConsumerModel;

namespace RabbitMQ_masstransit_client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IRequestClient<MessageRequest> _requestClient;

        public MessageController(IRequestClient<MessageRequest> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            MessageRequest req = new() { Id = 1, Name = "s" };
            using (var request = _requestClient.Create(req))
            {
                try
                {

                    var response = await request.GetResponse<MessageResponse>();
                    return response.Message.ResponseMessage;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
        }
    }
}