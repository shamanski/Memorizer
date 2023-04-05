namespace WebBot.Extensions
{

    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;
    using Microsoft.Extensions.DependencyInjection;
    using TgBot.BotCommands;
    using TgBot.BotCommands.Commands;

    public static class CommandExtensions
    {

        public static IServiceCollection AddCommands(this IServiceCollection services, string assemblyName, Type commandBaseType)
        {
            var assembly = Assembly.Load(new AssemblyName(assemblyName));

            var commandTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(CommandAttribute), true).Any() && commandBaseType.IsAssignableFrom(t));

            foreach (var type in commandTypes)
            {
                services.AddTransient(commandBaseType, type);
            }

            return services;
        }
    }
    }

