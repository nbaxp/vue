using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using DotNet6WebApi;
using DotNet6WebApi.Localization;
using DotNet6WebApi.Resources;
using DotNet6WebApi.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var origins = "AllowAllHeaders";

builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
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
builder.Services.AddSingleton<IValidationAttributeAdapterProvider, LocalizedValidationAttributeAdapterProvider>();
builder.Services.AddLocalization();
//builder.Services.AddPortableObjectLocalization(o=>o.ResourcesPath = "Resources");
builder.Services.AddMvc(o=>o.ModelMetadataDetailsProviders.Insert(0,new CustomIDisplayMetadataProvider()))
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var localizer = factory.Create(typeof(Resource));
            return localizer;
        };
    })
    .ConfigureApplicationPartManager(apm =>apm.FeatureProviders.Add(new GenericControllerFeatureProvider()));
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] {
        new CultureInfo("zh-CN"),
        new CultureInfo("en-US"),
   };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    //RouteDataRequestCultureProvider不能使用options作为初始化参数，否则RouteAttribute路由的locale无效
    options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
});
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

using var scope = app.Services.CreateScope();

app.UseRequestLocalization(scope.ServiceProvider.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);//必须在UseEndpoints前
app.UseEndpoints(endpoints =>//MapDynamicControllerRoute
{
    endpoints.MapControllerRoute(//必须在前面否则url的culture部分会变成参数而不是路径
        name: "cultureRoute",
        pattern: "{culture}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<TestHub>("/hub");
    endpoints.MapSwagger();
});

using var db = scope.ServiceProvider.GetRequiredService<DbContext>();
if (db.Database.EnsureCreated())
{
    //db.Seed();
    db.Set<User>().Add(new User { UserName = "admin" });
    db.SaveChanges();
}

app.Run();
