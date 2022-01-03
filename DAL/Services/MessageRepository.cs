using RabbitMQ_masstransit.Models.TodoConsumerModel;

namespace DAL
{
    public class MessageRepository: IMessageRepository
    {
        public MessageResponse GetMessages()
        {
            MessageResponse res = new() { ResponseMessage = "DONE" };
            return res;
        }
    }
}