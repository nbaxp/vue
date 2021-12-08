var builder = WebApplication.CreateBuilder(args);
var origins = "AllowAllHeaders";

builder.Services.AddCors(options =>
{
    options.AddPolicy(origins, builder =>
     {
         builder.SetIsOriginAllowed(o => true)
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials();
     });
});
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("redis.signalr"),o => o.Configuration.ChannelPrefix = "signalr");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var webSocketOptions = new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
};
app.UseWebSockets();
app.MapControllers();
app.UseRouting();
app.UseCors(origins);
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TestHub>("/hub");
});

app.Run();
