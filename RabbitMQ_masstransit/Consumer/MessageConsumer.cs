using DAL;
using MassTransit;
using RabbitMQ_masstransit.Models.TodoConsumerModel;

namespace RabbitMQ_masstransit.Consumer
{
    public class MessageConsumer : IConsumer<MessageRequest>
    {
        private readonly IMessageRepository _IMessageRepository;

        public MessageConsumer(IMessageRepository iMessageRepository)
        {
            _IMessageRepository = iMessageRepository;
        }
        public async Task Consume(ConsumeContext<MessageRequest> context)
        {
            MessageResponse res = _IMessageRepository.GetMessages();
            await context.RespondAsync<MessageResponse>(res);
        }
    }
}
