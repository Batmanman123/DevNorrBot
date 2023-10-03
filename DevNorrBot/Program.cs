using System;
using System.Reflection;
using System.Threading.Tasks;
using DevNorrBot;
using DevNorrBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    public static List<DevNorrBot.Payload.Message> Messages = new List<DevNorrBot.Payload.Message>();
    static async Task Main(string[] args)
    {

        DependencyInjection dependencyInjection = new DependencyInjection();
        dependencyInjection.AddDependencies();

        var config = dependencyInjection.GetService<IConfiguration>();
        var discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = Constants.DiscordToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });

        var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { "!" }
        });

        commands.RegisterCommands<CommandModule>();
        commands.RegisterCommands<GptCommands>();


        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
}


