using Microsoft.OpenApi.Models;
using OrderAggregator.Clients;
using OrderAggregator.Middlewares;
using OrderAggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(o => o.SuppressMapClientErrors = true);

builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders API", Version = "v1" });
    });

builder.Services.AddHostedService<OrderAggregator.Workers.OrderAggregator>();
builder.Services.AddSingleton<IThirdPartySystemClient, ThirdPartySystemClient>();
builder.Services.AddSingleton<IOrderQueue, OrderQueue>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

app.MapControllers();

//app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
