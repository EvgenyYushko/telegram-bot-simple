﻿using System;
using System.Linq;
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
		private static string _webhookUrl = "https://telegram-bot-simple.onrender.com/api/bot";

		public TelegramBotClient _botClient;

		public BotController()
		{
			InitClient();
		}

		private void InitClient()
		{
			_botClient = new TelegramBotClient(botToken);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Update update)
		{
			Console.WriteLine($"Bot {await _botClient.GetMeAsync()} is running..."); ;

			var cancellationToken = new CancellationToken();

			if (update.Message == null)
			{
				return Ok();
			}

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

		[HttpGet("/health")]
		public IActionResult health(int a)
		{
			Console.WriteLine("health - OK");
			return Ok("OK");
		}

		//[HttpGet("KeepAlive")]
		//public IActionResult KeepAlive(int a)
		//{
		//	return Ok(++a);
		//}

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

		public async Task ConfigureWebhookAsync(bool local)
		{
			if (local)
			{
				await _botClient.DeleteWebhookAsync();
			}
			else
			{
				var wh = await _botClient.GetWebhookInfoAsync();
				if (wh.IpAddress is null)
				{
					await _botClient.SetWebhookAsync(_webhookUrl);
				}
			}
		}

		#region Test

		public void Test()
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

			Task.Run(async () => await ConfigureWebhookAsync(true));
		}

		private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			await Post(update);
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

		private static ValueTask BotClient_OnMakingApiRequest(ITelegramBotClient botClient,
			Telegram.Bot.Args.ApiRequestEventArgs args, CancellationToken cancellationToken = default) => default;


		private static ValueTask BotClient_OnApiResponseReceived(ITelegramBotClient botClient,
			Telegram.Bot.Args.ApiResponseEventArgs args, CancellationToken cancellationToken = default) => default;

		#endregion
	}
}
