using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram
{
    public static class TelegramFunction
    {
        [FunctionName("Telegram")]
        public static async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request,
            ILogger logger)
        {
            var telegramClient = GetTelegramBotClient();
            logger.LogInformation("Invoke telegram update function");
            var body = await request.ReadAsStringAsync();

            var update =JsonConvert.DeserializeObject<Update>(body);

            if (update.Type == UpdateType.Message)
            {
                await telegramClient.SendTextMessageAsync(update.Message.Chat, $"Lijo George : {update.Message.Text}");
            }
            return new OkResult();
        }

        private static TelegramBotClient GetTelegramBotClient()
        {
            var token = Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.Process);

            if (token is null)
            {
                throw new AggregateException("Can not get token. Set token in environment settings");
            }
            var telegramClient = new TelegramBotClient(token);
            return telegramClient;
        }
    }
}