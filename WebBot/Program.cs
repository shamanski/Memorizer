using Microsoft.EntityFrameworkCore;
using TgBot;
using TgBot.BotCommands;
using TgBot.BotCommands.Commands;
using WebBot.Controllers;
using Microsoft.Extensions.Configuration;
using Model.Services;
using Memorizer.Algorithm;
using System.Reflection;
using WebBot.Extensions;
using Model.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Model.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration.GetValue<string>("Jwt:Key"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCommands("TgBot", typeof(IBotCommand));

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
builder.Services.AddDbContext<IdentityDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));
builder.Services.AddDbContext<Model.Services.WebAppContext>(options =>
options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders()
.AddUserManager<UserManager<ApplicationUser>>();

builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddScoped<WebBotController>();
builder.Services.AddScoped<IUserService,MyUserService>();
builder.Services.AddScoped<MyUserService>();
builder.Services.AddTransient<LearningService>();
builder.Services.AddTransient<StandardLesson>();
builder.Services.AddScoped<ChatController>();
builder.Services.AddScoped<AllWordsService>();
builder.Services.AddScoped<StateController<BotCommand>>();
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIdentityUserService, IdentityUserService>();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();
app.Services.GetService<TelegramBot>()!.GetBot().Wait();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!roleManager.RoleExistsAsync("User").Result)
    {
        roleManager.CreateAsync(new IdentityRole("User")).Wait();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();





