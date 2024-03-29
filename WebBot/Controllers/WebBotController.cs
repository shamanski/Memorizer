﻿
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
        private readonly MyUserService users;
        private readonly ICommandService command;

        public WebBotController(MyUserService usersService, ICommandService commandService)
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
            var tgId = data?.From?.Id.ToString();
            var tgName = data?.From?.Username;
            var user = new Memorizer.DbModel.User { TelegramId = tgId, TelegramName = tgName };
            await users.SetCurrentUser(tgId);
            try
            {
                user = await users.GetUserByTelegramIdAsync(tgId);
            }
            catch
            {
                await users.AddUserAsync(user);
            }    

            data.Message!.Text = data.Data;
            data.Message!.Caption = data.Id;
            await command.Execute(user, data.Message);
        }

        protected async Task OnMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            var tgId = message.From.Id.ToString();
            var tgName = message.From.Username;
            var user = new Memorizer.DbModel.User { TelegramId = tgId, TelegramName = tgName };
            await users.SetCurrentUser(tgId);
            try
            {
                user = await users.GetUserByTelegramIdAsync(tgId);
            }
            catch
            {
                await users.AddUserAsync(user);
            }

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
    

