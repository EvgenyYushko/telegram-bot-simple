using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using VkNet;
using VkNet.Model;
using UpdateType = Telegram.Bot.Types.Enums.UpdateType;

public class Program
{
    private static string botToken = "6769660711:AAF1wwIyjL1MLpU9ESvelhECSUam5N48E_I"; // Замените на ваш API токен
    private static TelegramBotClient botClient;

	private static string vkTokenData = "https://oauth.vk.com/blank.html#access_token=vk1.a.T8SZBMFoACwm77HA76VlpSPRUQi_VBY2GR9O-kXKk_dFzsG8pisdozUWxEIVUnja8Q4hcnXRABAq7kGWy33B-5vOqz7qoJexzn_wqTNDf4okFu9TcStV4NGrszA2AbieikyY6ZKWoLyNPSerQTL8rCxfuiKZ9-j8078XlZF3UlWxh99OzDjqI9DJsCyimU1mnE0S8Z_qk-QV8dzK4VZ1og&user_id=15062517";
	private static string vkTokenData2 = "https://oauth.vk.com/blank.html#access_token=vk1.a.lxHy6lkT9oqxoWSbJfOvZMj3UEP7ZnqY7F49h4K6mpo9_IdNXjBo_aV55rNl_GlSvNbLHKbaDKVHC5NgrGW0cWsaAshM-K0r6EnLJsEBRd4oum1xOvxFLKwONEzq-NxL5w4PKeaFPG3n7qLKULZI0jVEi63b-UMMLU6MxulOP7ji_7DSzyF8MxzprtAPRWxjmM0sndgCxnR22dn_KBj8Uw&user_id=15062517";
	private static string vkTokenData3 = "https://oauth.vk.com/blank.html#access_token=vk1.a.Jc9MaLs2mOeyzTh65KDudSps-KdM2lPEbBoteRnxnVN37CrA4tpaPHNrshDPoAtETAlmEqyExxY5JpUtHbpt7oBw_0dWSDLbQkQ6Jd9lE9asxcwRNGD06QlXzeTJRTbtbDMNUtPHwk9FGVXhCJOoy_oyrowMKBNPxBkhlgJ88uAuP2k0lVYCNJQbuuE4WbS9zGRxYRdl6uYyJUKYj21HUA&expires_in=0&user_id=15062517";

	private static string vkToken =	"vk1.a.T8SZBMFoACwm77HA76VlpSPRUQi_VBY2GR9O-kXKk_dFzsG8pisdozUWxEIVUnja8Q4hcnXRABAq7kGWy33B-5vOqz7qoJexzn_wqTNDf4okFu9TcStV4NGrszA2AbieikyY6ZKWoLyNPSerQTL8rCxfuiKZ9-j8078XlZF3UlWxh99OzDjqI9DJsCyimU1mnE0S8Z_qk-QV8dzK4VZ1og";
	private static string vkToken2 = "vk1.a.lxHy6lkT9oqxoWSbJfOvZMj3UEP7ZnqY7F49h4K6mpo9_IdNXjBo_aV55rNl_GlSvNbLHKbaDKVHC5NgrGW0cWsaAshM-K0r6EnLJsEBRd4oum1xOvxFLKwONEzq-NxL5w4PKeaFPG3n7qLKULZI0jVEi63b-UMMLU6MxulOP7ji_7DSzyF8MxzprtAPRWxjmM0sndgCxnR22dn_KBj8Uw";
	private static string vkToken3 = "vk1.a.bdeX0iLIVRVYMdy1yqzzRLbdJB7aEGqRiUEo4e_FOwYClmHMAZgP8t31ivZwgQ9VTnhledfjzSOrBNifkJmXBRxEziGgx6oNHaaTVi-nGdOxZFtSWkCLJVsV51pjFzyh2wRe0nXb724Hb3FC9_gxDtM_c_WNY5qXX1Mm5JB7eiJjA4XiABlGjZXvQj07VOsXf5ODNr3dqCyxpXvQIOM3mQ";
	private static string vkToken4 = "vk1.a.Jc9MaLs2mOeyzTh65KDudSps-KdM2lPEbBoteRnxnVN37CrA4tpaPHNrshDPoAtETAlmEqyExxY5JpUtHbpt7oBw_0dWSDLbQkQ6Jd9lE9asxcwRNGD06QlXzeTJRTbtbDMNUtPHwk9FGVXhCJOoy_oyrowMKBNPxBkhlgJ88uAuP2k0lVYCNJQbuuE4WbS9zGRxYRdl6uYyJUKYj21HUA";
	private static string vkToken5 = "vk1.a.rv6WOOKI5wOVxLwq5K899zdci-5I8QPx4CNhlOwd2l9hkd7FNjv6DUcZGHf2AdbDrzf1Ykyel-k4bn1Q686OvZ8fTFGly1-P5HgBJd_tIvJLd0cfATQLs8sDxOwXkMyCISM4Df4g3tO_IIz8C7QnF8u_cc_GaeztcqmKwM3Xn4xbR1mC5Ep_BOYSBFIRl5-3";

	private static VkApi vkApi;

    static async Task Main(string[] args)
    {

        botClient = new TelegramBotClient(botToken);

		// Инициализация Vk API
		//vkApi = new VkApi();
		//vkApi.Authorize(new ApiAuthParams { AccessToken = vkToken4 });

		//try
		//{
		//	// Запрос информации о текущем пользователе
		//	var user = vkApi.Account.GetProfileInfo();
		//	Console.WriteLine($"Токен действителен. Текущий пользователь: {user.FirstName} {user.LastName}");
		//}
		//catch (Exception ex)
		//{
		//	Console.WriteLine($"Ошибка при проверке токена: {ex.Message}");
		//}

		// Запуск цикла проверки сообщений
		//while (true)
		//{
		//	await CheckVkMessages();
		//	Thread.Sleep(10000); // Пауза на 10 секунд перед повторной проверкой
		//}

		using var cts = new CancellationTokenSource();

        // Запуск бота с обработкой сообщений
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // Обработка всех типов обновлений
            },
            cancellationToken: cts.Token
        );

		botClient.OnApiResponseReceived += BotClient_OnApiResponseReceived;
		botClient.OnMakingApiRequest += BotClient_OnMakingApiRequest;

        Console.WriteLine("Бот запущен. Нажмите Enter для выхода.");
        //Console.ReadLine();

		await botClient.SendTextMessageAsync(
			chatId: 1231047171,
			text: $"Новое сообщение от пользователя {vkUserId}: Я запустился!"
		);

        // Завершение работы бота
        //cts.Cancel();
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

	private static long vkUserId = 141230817;
	private static long chatBotId = -4585706473;
	//private static long vkUserId = 15062517; //мой
	private async static Task CheckVkMessages()
	{
		//var s = vkApi.Friends.GetLists();
		var a1 = vkApi.Messages.GetDialogs(new MessagesDialogsGetParams());
		var s2 = vkApi.Messages.GetConversations(new GetConversationsParams());

		var history = vkApi.Messages.GetHistory(new MessagesGetHistoryParams
		{
			UserId = vkUserId,
			Count = 5 // Получить последние 5 сообщений
		});

		if (history.Messages.Any())
		{
			var lastMessage = history.Messages.FirstOrDefault();
			if (lastMessage != null)
			{
				// Отправляем сообщение в Telegram
				await botClient.SendTextMessageAsync(
					chatId: 1231047171,
					text: $"Новое сообщение от пользователя {vkUserId}: {lastMessage.Text}"
				);
			}
		}
	}

	// Метод обработки обновлений
	private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message!.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Получено сообщение в чате {chatId}: {messageText}");

            // 1 opanAi
			//var prompt = "Напиши шутку про программистов";
			//var response = await GetOpenAIResponseAsync(prompt);
			//Console.WriteLine("Ответ от OpenAI: " + response);

            // 2
			//var prompt = "Расскажи интересную историю о путешествии на Марс.";
			//var response = await GetGPTJResponseAsync(prompt);
			//Console.WriteLine("GPT-J Response: " + response);

			// Простой ответ на текстовые сообщения
			await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Вы написали: " + messageText,
                cancellationToken: cancellationToken
            );

			//await botClient.SendTextMessageAsync(
			//	chatId: chatBotId,
			//	text: "Вы написали: " + messageText,
			//	cancellationToken: cancellationToken
			//);
        }

		//var an = update.Message.Animation;
		
		//await botClient.SendAnimationAsync(
		//	chatId: chatBotId,
		//	InputFile.FromFileId(an.FileId),
		//	cancellationToken: cancellationToken
		//);
	}

    // Метод обработки ошибок
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

	private static readonly string apiKey = "sk-r-hC07pFsINWKp-t6c4JoXBwqr9hjgak49MRwW3MmXT3BlbkFJ3jMSdkJI9CimHphSf2J4mzX9uCXFvsgGw19tO0HL8A"; // Замените на ваш API ключ
	private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";

	private static async Task<string> GetOpenAIResponseAsync(string prompt)
	{
        using (var httpClient = new HttpClient())
		{
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

			var requestBody = new
			{
				model = "gpt-3.5-turbo", // Используемая модель
				messages = new[]
				{
					new { role = "user", content = prompt }
				},
				max_tokens = 150, // Ограничение на количество токенов в ответе
			};

			var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

			var response = await httpClient.PostAsync(apiUrl, content);
			var responseString = await response.Content.ReadAsStringAsync();

			dynamic responseJson = JsonConvert.DeserializeObject(responseString);
			return responseJson.choices[0].message.content;
		}
	}

    private static readonly string apiKey2 = "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"; // Замените на ваш токен Hugging Face
    private static readonly string apiUrl2 = "https://api-inference.huggingface.co/models/EleutherAI/gpt-j-6B";

    private static async Task<string> GetGPTJResponseAsync(string prompt)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey2}");

            var requestBody = new
            {
                inputs = prompt,
                options = new { max_length = 200 } // Максимальная длина ответа
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl2, content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
