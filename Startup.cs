using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers().AddNewtonsoftJson();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			//app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapRazorPages();
			});

			var local = false;
			var s = new BotController();
			if (local)
			{
				s.Test();
			}
			else
			{
				Task.Run(async () => await s.ConfigureWebhookAsync(false));
			}

			//StartTimer();
		}

		//private static Timer _timer;

		//static void StartTimer()
		//{
		//	// Настраиваем таймер, чтобы метод RunPeriodicTask вызывался каждые 30 минут
		//	_timer = new Timer(async _ => await SendHttpRequest(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

		//	Console.WriteLine("StartTimer...");
		//}

		//private static async Task SendHttpRequest()
		//{
		//	Console.WriteLine("Start KeepAlive");
		//	// Создаём экземпляр HttpClient
		//	using (HttpClient client = new HttpClient())
		//	{
		//		// Указываем базовый адрес (опционально)
		//		client.BaseAddress = new Uri("https://telegram-bot-simple.onrender.com");

		//		// Настройка пропуска проверки SSL-сертификата (только для локальной разработки)
		//		HttpClientHandler handler = new HttpClientHandler
		//		{
		//			ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
		//		};

		//		// Повторное использование HttpClient с обработчиком
		//		using (HttpClient secureClient = new HttpClient(handler))
		//		{
		//			try
		//			{
		//				// Полный адрес с параметрами
		//				string url = "https://telegram-bot-simple.onrender.com/api/Bot/KeepAlive?a=1";

		//				// Отправляем GET-запрос
		//				HttpResponseMessage response = await secureClient.GetAsync(url);

		//				// Проверяем успешность запроса
		//				if (response.IsSuccessStatusCode)
		//				{
		//					// Читаем содержимое ответа
		//					string responseContent = await response.Content.ReadAsStringAsync();
		//					Console.WriteLine($"KeepAliveStatus: {responseContent}");
		//				}
		//				else
		//				{
		//					Console.WriteLine($"Ошибка: {response.StatusCode}");
		//				}
		//			}
		//			catch (Exception ex)
		//			{
		//				// Обработка ошибок
		//				Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
		//			}
		//		}
		//	}
		//	Console.WriteLine("End KeepAlive");
		//}
	}
}
