using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WebApplication1
{
	[ApiController]
	[Route("api/[controller]")]
	public class BotController : ControllerBase
	{
		private static string botToken = "6769660711:AAF1wwIyjL1MLpU9ESvelhECSUam5N48E_I"; // Замените на ваш API токен

		private readonly TelegramBotClient _botClient;

		public BotController()
		{
			_botClient = new TelegramBotClient(botToken);

			using var cts = new CancellationTokenSource();

			// Запуск бота с обработкой сообщений
			//_botClient.StartReceiving(
			//	HandleUpdateAsync,
			//	HandleErrorAsync,
			//	new ReceiverOptions
			//	{
			//		AllowedUpdates = Array.Empty<UpdateType>() // Обработка всех типов обновлений
			//	},
			//	cancellationToken: cts.Token
			//);
			//_botClient.OnApiResponseReceived += BotClient_OnApiResponseReceived;
			//_botClient.OnMakingApiRequest += BotClient_OnMakingApiRequest;
			//_botClient.DeleteWebhookAsync(true);
			//Task.Run(async () => await _botClient.SendTextMessageAsync(1231047171, "Bot started")).Wait();
		}

		// Метод обработки обновлений
		private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			if (update.Type == UpdateType.Message && update.Message!.Text != null)
			{
				var chatId = update.Message.Chat.Id;
				var messageText = update.Message.Text;
				Console.WriteLine($"Получено сообщение в чате {chatId}: {messageText}");
				
				await botClient.SendTextMessageAsync(
					chatId: chatId,
					text: "Вы написали: " + messageText,
					cancellationToken: cancellationToken
				);
			}
		}

		private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			var errorMessage = exception switch
			{
				ApiRequestException apiRequestException
					=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
				_ => exception.ToString()
			};
			Console.WriteLine(errorMessage);
			return Task.CompletedTask;
		}

		private static ValueTask BotClient_OnMakingApiRequest(ITelegramBotClient botClient, Telegram.Bot.Args.ApiRequestEventArgs args, CancellationToken cancellationToken = default)
		{
			return default;
			//throw new NotImplementedException();
		}
		private static ValueTask BotClient_OnApiResponseReceived(ITelegramBotClient botClient, Telegram.Bot.Args.ApiResponseEventArgs args, CancellationToken cancellationToken = default)
		{
			return default;
			//throw new NotImplementedException();
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] TelegramResponse update)
		{
			Console.WriteLine($"Start");
			Console.WriteLine($"Raw data: {update}");

			if (update.Message != null)
			{
				var chatId = update.Message.Chat.Id;
				var messageText = update.Message.Text;

				//1231047171
				await _botClient.SendTextMessageAsync(chatId, $"You said: {messageText}");
			}

			return Ok();
		}

		[HttpGet("Test")]
		public IActionResult Test(int a)
		{
			return Ok(a++);
		}
	}


	public class TelegramResponse
	{
		[JsonPropertyName("message")]
		public Message Message { get; set; }
	}

	public class Message
	{
		[JsonPropertyName("message_id")]
		public int MessageId { get; set; }

		[JsonPropertyName("from")]
		public User From { get; set; }

		[JsonPropertyName("chat")]
		public Chat Chat { get; set; }

		[JsonPropertyName("date")]
		public long Date { get; set; } // UNIX timestamp

		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class User
	{
		[JsonPropertyName("id")]
		public long Id { get; set; }

		[JsonPropertyName("is_bot")]
		public bool IsBot { get; set; }

		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		[JsonPropertyName("username")]
		public string Username { get; set; }

		[JsonPropertyName("language_code")]
		public string LanguageCode { get; set; }
	}

	public class Chat
	{
		[JsonPropertyName("id")]
		public long Id { get; set; }

		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		[JsonPropertyName("username")]
		public string Username { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; } // Например, "private"
	}
}
