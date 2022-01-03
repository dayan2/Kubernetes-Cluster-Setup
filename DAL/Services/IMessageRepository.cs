using RabbitMQ_masstransit.Models.TodoConsumerModel;

namespace DAL
{
    public interface IMessageRepository
    {
        MessageResponse GetMessages();
    }
}
