using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReversoConsole.Controller;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBot;
using TgBot.BotCommands;

namespace WebBot.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class WebBotController : ControllerBase
    {
        private readonly UserController users;
        private readonly ICommandService command;

        public WebBotController(UserController usersController, ICommandService commandService)
        {
            users = usersController;    
            command = commandService;   
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] object update)
        {
            try
            {
                var upd = JsonConvert.DeserializeObject<Update>(update.ToString());
                await HandleUpdateAsync(upd);
            }
            catch (Exception)
            {
                return Ok();
            }

            return Ok();
        }

        protected async Task HandleUpdateAsync(Update update)
        {
            await Task.Run(() =>
            {
                if (update.Type == UpdateType.CallbackQuery)
                {
                    OnCallBack(update.CallbackQuery!); 
                    return;
                }

                if (update.Message?.Type == MessageType.Document)
                {
                    OnMessage(update.Message);
                }

                if (update.Message?.Type != MessageType.Text)
                    return;

                if (update.Message?.Type == MessageType.Text)
                {
                    OnMessage(update.Message);
                }
            });
        }

        protected async void OnCallBack(CallbackQuery data)
        {
            var user = users.GetUser(data.From.Id.ToString());
            data.Message!.Text = data.Data;
            data.Message!.Caption = data.Id;
            await Task.Run(() => command.Execute(user, data.Message));
        }

        protected async void OnMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            var user = users.GetUser(message?.From?.Id.ToString());
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            await Task.Run(() => command.Execute(user, message));

        }

        protected Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
    

