using System;
using System.Collections.Generic;
using System.Text;

namespace TgBot.BotCommands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Description { get; set; }
    }
}
