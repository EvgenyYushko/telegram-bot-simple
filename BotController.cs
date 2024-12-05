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
