using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("redis.signalr"), o => o.Configuration.ChannelPrefix = "signalr");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IRoleService, RoleService>();
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = key,
        NameClaimType = "Name",
        RoleClaimType = "Role",
    };
    options.SecurityTokenValidators.Clear();
    var serviceProvider = builder.Services.BuildServiceProvider().GetRequiredService<IServiceProvider>();
    options.SecurityTokenValidators.Add(new RoleJwtSecurityTokenHandler(serviceProvider));
});
builder.Services.AddSingleton(new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));

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
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TestHub>("/hub");
});

app.Run();
