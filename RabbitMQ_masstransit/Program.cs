using DAL;
using GreenPipes;
using MassTransit;
using RabbitMQ_masstransit.Consumer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MessageConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host("rabbitmq://host.docker.internal");

        cfg.ReceiveEndpoint("message-consumer", ep =>
        {
            ep.PrefetchCount = 16;
            cfg.ConfigureEndpoints(provider);
            //ep.UseMessageRetry(r => r.Interval(2, 10));
        });
    }));

});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}



app.UseAuthorization();

app.MapControllers();

app.Run();
