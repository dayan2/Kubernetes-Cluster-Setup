namespace RabbitMQ_masstransit.Models.TodoConsumerModel
{
    public class MessageRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MessageResponse
    {
        public string ResponseMessage { get; set; }
    }
}
