namespace WebBot.Extensions
{

    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using TgBot.BotCommands;

    public static class CommandExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services, string assemblyName, Type commandBaseType)
        {

            var assembly = Assembly.Load(new AssemblyName(assemblyName));


            var commandTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes<CommandAttribute>().Any());


            foreach (var type in commandTypes)
            {
                services.AddScoped(commandBaseType, type);
            }

            return services;
        }
    }
}
