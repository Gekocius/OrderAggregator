using OrderAggregator.Clients;
using OrderAggregator.Middlewares;
using OrderAggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHostedService<OrderAggregator.Workers.OrderAggregator>();
builder.Services.AddSingleton<IThirdPartySystemClient, ThirdPartySystemClient>();
builder.Services.AddSingleton<IOrderQueue, OrderQueue>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

app.MapControllers();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
