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
		//	// ����������� ������, ����� ����� RunPeriodicTask ��������� ������ 30 �����
		//	_timer = new Timer(async _ => await SendHttpRequest(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

		//	Console.WriteLine("StartTimer...");
		//}

		//private static async Task SendHttpRequest()
		//{
		//	Console.WriteLine("Start KeepAlive");
		//	// ������ ��������� HttpClient
		//	using (HttpClient client = new HttpClient())
		//	{
		//		// ��������� ������� ����� (�����������)
		//		client.BaseAddress = new Uri("https://telegram-bot-simple.onrender.com");

		//		// ��������� �������� �������� SSL-����������� (������ ��� ��������� ����������)
		//		HttpClientHandler handler = new HttpClientHandler
		//		{
		//			ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
		//		};

		//		// ��������� ������������� HttpClient � ������������
		//		using (HttpClient secureClient = new HttpClient(handler))
		//		{
		//			try
		//			{
		//				// ������ ����� � �����������
		//				string url = "https://telegram-bot-simple.onrender.com/api/Bot/KeepAlive?a=1";

		//				// ���������� GET-������
		//				HttpResponseMessage response = await secureClient.GetAsync(url);

		//				// ��������� ���������� �������
		//				if (response.IsSuccessStatusCode)
		//				{
		//					// ������ ���������� ������
		//					string responseContent = await response.Content.ReadAsStringAsync();
		//					Console.WriteLine($"KeepAliveStatus: {responseContent}");
		//				}
		//				else
		//				{
		//					Console.WriteLine($"������: {response.StatusCode}");
		//				}
		//			}
		//			catch (Exception ex)
		//			{
		//				// ��������� ������
		//				Console.WriteLine($"������ ��� ���������� �������: {ex.Message}");
		//			}
		//		}
		//	}
		//	Console.WriteLine("End KeepAlive");
		//}
	}
}
