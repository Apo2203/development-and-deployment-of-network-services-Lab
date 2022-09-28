namespace Clients;

using Microsoft.Extensions.DependencyInjection;

using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;

using NLog;

using Services;

/// <summary>
/// Client example.
/// </summary>
class ClientRubbit
{
	/// <summary>
	/// Logger for this class.
	/// </summary>
	Logger log = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Configures logging subsystem.
	/// </summary>
	private void ConfigureLogging()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var console =
			new NLog.Targets.ConsoleTarget("console")
			{
				//Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
				Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
			};
		config.AddTarget(console);
		config.AddRuleForAllLevels(console);

		LogManager.Configuration = config;
	}

	/// <summary>
	/// Program body.
	/// </summary>
	private void Run() {
		//configure logging
		ConfigureLogging();

		//run everythin in a loop to recover from connection errors
		while( true )
		{
			try {
				//connect to the server, get service client proxy
				var sc = new ServiceCollection();
				sc
					.AddSimpleRpcClient(
						"service", 
						new HttpClientTransportOptions
						{
							Url = "http://127.0.0.1:5000/simplerpc",
							Serializer = "HyperionMessageSerializer"
						}
					)
					.AddSimpleRpcHyperionSerializer();

				sc.AddSimpleRpcProxy<IService>("service");

				var sp = sc.BuildServiceProvider();

				var service = sp.GetService<IService>();

				//use service
				var rnd = new Random();

				/**
				Generate a rubbit. When it dead (when the function that generate it end) I wait 5 sec and then I generate another one.
				*/
				int xWaterPosition = rnd.Next(1, 50);
				int yWaterPosition = rnd.Next(1, 50);
				int volumeOfWater = rnd.Next(1, 10);
				while( true )
				{	
					log.Info($"A pool with about {volumeOfWater} liters of water is spawning in the coordinates {xWaterPosition}, {yWaterPosition} ...");
					service.generateWater(xWaterPosition, yWaterPosition, volumeOfWater);
					log.Info($"A wolf moved really close to the water and drank it. Respawning the water...\n");
					Thread.Sleep(5000);
				}
			}
			catch( Exception e )
			{
				//log whatever exception to console
				log.Warn(e, "Unhandled exception caught. Will restart main loop.");

				//prevent console spamming
				Thread.Sleep(2000);
			}
		}
	}

	/// <summary>
	/// Program entry point.
	/// </summary>
	/// <param name="args">Command line arguments.</param>
	static void Main(string[] args)
	{
		var self = new ClientRubbit();
		Console.WriteLine("I'm starting the generation of the rubbits!\n");
		self.Run();
	}
}
