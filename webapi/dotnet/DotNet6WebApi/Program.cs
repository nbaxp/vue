using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    o.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});
builder.Services.AddLocalization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddTransient<IRoleService, RoleService>();
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
//https://stackoverflow.com/questions/47138849/how-to-correctly-get-dependent-scoped-services-from-isecuritytokenvalidator
builder.Services.AddSingleton<CustomJwtSecurityTokenHandler>();
builder.Services.AddSingleton<JwtSecurityTokenHandler, CustomJwtSecurityTokenHandler>();
builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, CustomJwtBearerPostConfigureOptions>();
var tokenValidationParameters = new TokenValidationParameters
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
    ClockSkew = TimeSpan.Zero,//default 300
};
builder.Services.AddSingleton<TokenValidationParameters>(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
builder.Services.AddSingleton(new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("redis.signalr"), o => o.Configuration.ChannelPrefix = "signalr");

builder.Services.AddScoped<DbContext, ApplicationDbContext>();
var dbKey = builder.Configuration.GetValue("Database", "sqlite");
var connectionString = builder.Configuration.GetConnectionString($"db.{dbKey}");
builder.Services.AddDbContext<ApplicationDbContext>(
        options =>
        {
            if (dbKey == "mysql")
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
            else if (dbKey == "cockroachdb")
            {
                options.UseNpgsql(connectionString);
            }
            else
            {
                options.UseSqlite(connectionString);
            }
        });

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
app.UseStaticFiles();
app.UseRouting();

app.UseCors(origins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TestHub>("/hub");
});
using var scope = app.Services.CreateScope();
using var db = scope.ServiceProvider.GetRequiredService<DbContext>();

if (db.Database.EnsureCreated())
{
    //db.Seed();
    db.Set<User>().Add(new User { UserName = "admin" });
    db.SaveChanges();
}
app.Run();
