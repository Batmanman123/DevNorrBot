using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNorrBot.Commands
{
    public class CommandModule : BaseCommandModule
    {
        [Command("ping")]
        public async Task PingCommand(CommandContext ctx)
        {
                await ctx.RespondAsync("pong!");
        }
        [Command("Greet")]
        public async Task GreetCommand(CommandContext ctx,string name)
        {
            await ctx.RespondAsync($"Greetings, {name}!");
        }
    }
}
