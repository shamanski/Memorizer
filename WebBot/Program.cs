using ReversoConsole.Controller;
using TgBot;
using TgBot.BotCommands;
using TgBot.BotCommands.Commands;
using WebBot.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ReversoConsole.Controller.WebAppContext>();
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddScoped<WebBotController>();
builder.Services.AddTransient<UserController>();
builder.Services.AddScoped<ChatController>();
builder.Services.AddScoped<AllWordsController>();
builder.Services.AddSingleton<StateController>();
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





