using DevNorrBot.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevNorrBot.Commands
{
    public class GptCommands : BaseCommandModule
    {

        [Command("gpt")]
        public async Task GptCommand(CommandContext ctx, [RemainingText] string query)
        {
            var gptService = new Gpt3Service(Constants.Gpt3Token);
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            try
            {

                ShowTypingIndicatorUntilCancel(cancellationToken.Token, ctx);

                var response = await gptService.GenerateChatResponse(query, Program.Messages);

                await ctx.RespondAsync(response.choices.First().message.content);

                cancellationToken.Cancel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await ctx.RespondAsync("Sorry, there was an error processing your request.");
            }
        }

        [Command("gptimage")]
        public async Task GptImage(CommandContext ctx, [RemainingText] string query)
        {
            var gptService = new Gpt3Service(Constants.Gpt3Token);
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            
            try
            {
                ShowTypingIndicatorUntilCancel(cancellationToken.Token, ctx);


                var response = await gptService.GenerateImageResponse(1, query);


                var resultString = string.Join("\n", response);
                //foreach (var item in response)
                //{
                //    await ctx.RespondAsync(item);

                //}

                await ctx.RespondAsync(resultString);
                cancellationToken.Cancel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await ctx.RespondAsync("Sorry, there was an error processing your request.");
            }
        }

        private async Task ShowTypingIndicatorUntilCancel(CancellationToken cancellationToken, CommandContext ctx)
        {
            int typingIndicatorDuration = 9000;
            var counter = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                // Om det tar längre än 6 x typingIndicatorDuration så slutar den visa för mest troligt har det blivit något fel.
                if (counter >= 10)
                {
                    return;
                }

                await ctx.Channel.TriggerTypingAsync();
                await Task.Delay(typingIndicatorDuration, cancellationToken);
                counter++;
            }
        }
    }
}
