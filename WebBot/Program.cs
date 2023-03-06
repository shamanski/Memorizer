using Microsoft.EntityFrameworkCore;
using TgBot;
using TgBot.BotCommands;
using TgBot.BotCommands.Commands;
using WebBot.Controllers;
using Microsoft.Extensions.Configuration;
using Model.Services;
using Memorizer.Algorithm;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Model.Services.WebAppContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
}
);
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddScoped<WebBotController>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<StandardLesson>();
builder.Services.AddScoped<ChatController>();
builder.Services.AddScoped<AllWordsService>();
builder.Services.AddSingleton<StateController<BotCommand>>();
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





