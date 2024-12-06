using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace WebApplication1
{
	public class WebhookConfigurator
	{
		private readonly string _botToken;
		private readonly string _webhookUrl;

		public WebhookConfigurator(string botToken, string webhookUrl)
		{
			_botToken = botToken;
			_webhookUrl = webhookUrl;
		}

		public async Task ConfigureWebhookAsync()
		{
			var botClient = new TelegramBotClient(_botToken);

			try
			{
				await botClient.SetWebhookAsync(_webhookUrl);
				Console.WriteLine("Webhook successfully configured.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error setting webhook: {ex.Message}");
			}
		}
	}
}
