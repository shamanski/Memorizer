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
builder.Services.AddDbContext<ReversoConsole.Controller.WebAppContext>();
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddScoped<WebBotController>();
builder.Services.AddTransient<UserController>();
builder.Services.AddSingleton<ChatController>();
builder.Services.AddTransient<StateController>();
builder.Services.AddSingleton<ICommandService, CommandService>();

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





