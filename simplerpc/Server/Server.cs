namespace Servers;

using System.Net;

using NLog;

using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Server;
using SimpleRpc.Serialization.Hyperion;
using System.Threading;

using Services;


public class Server
{
	// Logger for this class.
	Logger log = LogManager.GetCurrentClassLogger();

	// Configure loggin subsystem.
	private void ConfigureLogging()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var console =
			new NLog.Targets.ConsoleTarget("console")
			{
				Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
			};
		config.AddTarget(console);
		config.AddRuleForAllLevels(console);

		LogManager.Configuration = config;
	}

	// Program entry point.
	public static void Main(string[] args)
	{
		var self = new Server();
		self.Run(args);
	}

	// Program body.
	private void Run(string[] args) 
	{
		//configure logging
		ConfigureLogging();

		//indicate server is about to start
		log.Info("Server is about to start...");
		
		//start the server
		StartServer(args);
		
		// New thread that will help me to check and work constantly on some parameters in the server
		new Thread(() => 
		{
			// Make actual thread to continue working
			Thread.CurrentThread.IsBackground = true; 

			// Operation I want to do in the new thread.
			while(true)
			{
				// Wolf eat more than he could. Waiting 5 seconds and make him eat again.
				if (Service.checkStatus() == false)
				{
					Thread.Sleep(5000);
					Service.resetFood();
				}
				// To avoid checking spam
				else Thread.Sleep(100);
			}

		}).Start();

		//Calling the function that will generate and update the wolf coordinates.		
		while(true)
		{
			
			Service.generateWolfPosition();
			// Update wolf position every 7 seconds.
			Thread.Sleep(7000);
		}

	}

	// Starts integrated server.
	private void StartServer(string[] args)
	{
		//create web app builder
		var builder = WebApplication.CreateBuilder(args);

		//configure integrated server
		builder.WebHost.ConfigureKestrel(opts => {
			opts.Listen(IPAddress.Loopback, 5000);
		});

		//add SimpleRPC services
		builder.Services
			.AddSimpleRpcServer(new HttpServerTransportOptions { Path = "/simplerpc" })
			.AddSimpleRpcHyperionSerializer();

		//add our custom services
		builder.Services
			.AddSingleton<IService, Service>();

		//build the server
		var app = builder.Build();

		//add SimpleRPC middleware
		app.UseSimpleRpcServer();
		
		//run the server like this to implement background processing in the main thread
		app.RunAsync();
	}
}