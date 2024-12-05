using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApplication1
{
	[ApiController]
	[Route("[controller]")]
	public class BotController : ControllerBase
	{
		private static string botToken = "6769660711:AAF1wwIyjL1MLpU9ESvelhECSUam5N48E_I"; // Замените на ваш API токен

		private readonly TelegramBotClient _botClient;

		public BotController()
		{
			_botClient = new TelegramBotClient(botToken);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Update update)
		{
			//if (update.Message != null)
			//{
			//	var chatId = update.Message.Chat.Id;
			//	var messageText = update.Message.Text;

				await _botClient.SendTextMessageAsync(1231047171, $"You said: dssaaaa");
			//}

			return Ok();
		}

		[HttpGet("Test")]
		public IActionResult Test(int a)
		{
			return Ok(a++);
		}
	}
}
