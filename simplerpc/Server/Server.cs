namespace Servers;

using System.Net;

using NLog;

using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Server;
using SimpleRpc.Serialization.Hyperion;

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
		
		//Calling the function that will generate the wolf coordinates.
		//TODO METTI IL LOCK NEL SERVER (come fai con il client)
		
		var sc = new ServiceCollection();
		var sp = sc.BuildServiceProvider();
		var service = sp.GetService<IService>();

		while(true)
		{
			Service.logic.generateWolfPosition();
			//service.generateWolfPosition();
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