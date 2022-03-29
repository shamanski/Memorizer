using ReversoConsole.Controller;
using TgBot;
using TgBot.BotCommands;
using TgBot.BotCommands.Commands;
using WebBot.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddSingleton<WebBotController>();
builder.Services.AddSingleton<UserController>();
builder.Services.AddSingleton<ChatController>();
builder.Services.AddTransient<BotCommand, LessonCommand>();
builder.Services.AddTransient<BotCommand, AddCommand>();
builder.Services.AddTransient<BotCommand, InfoCommand>();
builder.Services.AddTransient<BotCommand, RemoveCommand>();
builder.Services.AddTransient<BotCommand, StartCommand>();
builder.Services.AddTransient<StateController>();
builder.Services.AddSingleton<ICommandService, CommandService>();

var app = builder.Build();
_ = app.Services.GetService<TelegramBot>()!.GetBot();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); 




