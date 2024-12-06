using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WebApplication1
{
	[ApiController]
	[Route("api/[controller]")]
	public class BotController : ControllerBase
	{
		private static string botToken = "6769660711:AAF1wwIyjL1MLpU9ESvelhECSUam5N48E_I"; // Замените на ваш API токен

		private TelegramBotClient _botClient;

		public BotController()
		{
			InitClient();
			//Test();
		}

		private void InitClient()
		{
			_botClient = new TelegramBotClient(botToken);
		}

		private void Test()
		{
			using var cts = new CancellationTokenSource();
			// Запуск бота с обработкой сообщений
			_botClient.StartReceiving(
				HandleUpdateAsync,
				HandleErrorAsync,
				new ReceiverOptions
				{
					AllowedUpdates = Array.Empty<UpdateType>() // Обработка всех типов обновлений
				},
				cancellationToken: cts.Token
			);
			_botClient.OnApiResponseReceived += BotClient_OnApiResponseReceived;
			_botClient.OnMakingApiRequest += BotClient_OnMakingApiRequest;
			//_botClient.DeleteWebhookAsync(true);
		}

		private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			if (update.Type == UpdateType.Message && update.Message!.Text != null)
			{
				var chatId = update.Message.Chat.Id;
				var messageText = update.Message.Text;
				Console.WriteLine($"Получено сообщение в чате {chatId}: {messageText}");
				// Простой ответ на текстовые сообщения
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
		}
		private static ValueTask BotClient_OnApiResponseReceived(ITelegramBotClient botClient, Telegram.Bot.Args.ApiResponseEventArgs args, CancellationToken cancellationToken = default)
		{
			return default;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Update update)
		{
			Console.WriteLine($"Bot {await _botClient.GetMeAsync()} is running...");;

			var cancellationToken = new CancellationToken();

			if (update.Message == null) return Ok();

			var message = update.Message;
			var chatId = message.Chat.Id;

			switch (message.Type)
			{
				case MessageType.Text:
					await _botClient.SendTextMessageAsync(chatId, "Вы отправили текст: " + message.Text,
						cancellationToken: cancellationToken);
					await ShowButtons(_botClient, chatId, cancellationToken);
					break;

				case MessageType.Photo:
					await _botClient.SendTextMessageAsync(chatId, "Вы отправили фото!",
						cancellationToken: cancellationToken);
					await SendPhoto(message);
					break;

				case MessageType.Document:
					await _botClient.SendTextMessageAsync(chatId, "Вы отправили файл: " + message.Document.FileName,
						cancellationToken: cancellationToken);
					break;

				case MessageType.Audio:
					await _botClient.SendTextMessageAsync(chatId, "Вы отправили аудио!",
						cancellationToken: cancellationToken);
					break;

				case MessageType.Video:
					await _botClient.SendTextMessageAsync(chatId, "Вы отправили видео!",
						cancellationToken: cancellationToken);
					break;

				default:
					await _botClient.SendTextMessageAsync(chatId, "Я пока не умею обрабатывать это сообщение.",
						cancellationToken: cancellationToken);
					break;
			}

			return Ok();
		}

		private async Task<Message> SendPhoto(Message msg)
		{
			// Получаем самый крупный файл (последний в массиве)
			var fileId = msg.Photo.Last().FileId;

			// Отправляем фото обратно в чат
			return await _botClient.SendPhotoAsync(msg.Chat.Id, InputFile.FromFileId(fileId));
		}

		private static async Task ShowButtons(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
		{
			var keyboard = new InlineKeyboardMarkup(new[]
			{
				new[]
				{
					InlineKeyboardButton.WithCallbackData("Кнопка 1", "button1"),
					InlineKeyboardButton.WithCallbackData("Кнопка 2", "button2")
				},
				new[]
				{
					InlineKeyboardButton.WithUrl("Открыть Google", "https://www.google.com")
				}
			});

			await botClient.SendTextMessageAsync(
				chatId: chatId,
				text: "Выберите опцию:",
				replyMarkup: keyboard,
				cancellationToken: cancellationToken
			);
		}

		[HttpGet("Test")]
		public IActionResult Test(int a)
		{
			return Ok(a++);
		}
	}


	//public class TelegramResponse
	//{
	//	[JsonPropertyName("message")]
	//	public Message Message { get; set; }
	//}

	//public class Message
	//{
	//	[JsonPropertyName("message_id")]
	//	public int MessageId { get; set; }

	//	[JsonPropertyName("from")]
	//	public User From { get; set; }

	//	[JsonPropertyName("chat")]
	//	public Chat Chat { get; set; }

	//	[JsonPropertyName("date")]
	//	public long Date { get; set; } // UNIX timestamp

	//	[JsonPropertyName("text")]
	//	public string Text { get; set; }
	//}

	//public class User
	//{
	//	[JsonPropertyName("id")]
	//	public long Id { get; set; }

	//	[JsonPropertyName("is_bot")]
	//	public bool IsBot { get; set; }

	//	[JsonPropertyName("first_name")]
	//	public string FirstName { get; set; }

	//	[JsonPropertyName("last_name")]
	//	public string LastName { get; set; }

	//	[JsonPropertyName("username")]
	//	public string Username { get; set; }

	//	[JsonPropertyName("language_code")]
	//	public string LanguageCode { get; set; }
	//}

	//public class Chat
	//{
	//	[JsonPropertyName("id")]
	//	public long Id { get; set; }

	//	[JsonPropertyName("first_name")]
	//	public string FirstName { get; set; }

	//	[JsonPropertyName("last_name")]
	//	public string LastName { get; set; }

	//	[JsonPropertyName("username")]
	//	public string Username { get; set; }

	//	[JsonPropertyName("type")]
	//	public string Type { get; set; } // Например, "private"
	//}
}
