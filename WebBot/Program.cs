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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration.GetValue<string>("Jwt:Key"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCommands("TgBot", typeof(IBotCommand));
builder.Services.AddDbContext<Model.Services.WebAppContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
}
);
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddScoped<WebBotController>();
builder.Services.AddScoped<UserService>();
builder.Services.AddTransient<LearningService>();
builder.Services.AddTransient<StandardLesson>();
builder.Services.AddScoped<ChatController>();
builder.Services.AddScoped<AllWordsService>();
builder.Services.AddScoped<StateController<BotCommand>>();
builder.Services.AddScoped<ICommandService, CommandService>();

var app = builder.Build();
app.Services.GetService<TelegramBot>()!.GetBot().Wait();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); 





