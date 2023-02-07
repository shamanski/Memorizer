
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Model.Services;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBot.BotCommands;

namespace WebBot.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class WebBotController : ControllerBase
    {
        private readonly UserService users;
        private readonly ICommandService command;

        public WebBotController(UserService usersService, ICommandService commandService)
        {
            users = usersService;    
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

                if (update.Type == UpdateType.CallbackQuery)
                {
                    await OnCallBack(update.CallbackQuery!); 
                    return;
                }

                if (update.Message?.Type == MessageType.Document || update.Message?.Type == MessageType.Text)
                {                   
                    await OnMessage(update.Message);
                }
        }

        protected async Task OnCallBack(CallbackQuery data)
        {
            var user = users.GetUser(data.From.Id.ToString());
            data.Message!.Text = data.Data;
            data.Message!.Caption = data.Id;
            await command.Execute(user, data.Message);
        }

        protected async Task OnMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            var user = users.GetUser(message?.From?.Id.ToString());
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            await command.Execute(user, message);

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
    

