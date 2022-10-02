namespace Clients;

using Microsoft.Extensions.DependencyInjection;

using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;

using NLog;

using Services;

// Client that implement the water.
class ClientWater
{
	// constant variable that help me to relate to water
	public const int WATER = 2;
	/// Logger for this class.
	Logger log = LogManager.GetCurrentClassLogger();
	/// Configures logging subsystem.
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

	// Program body where the water's function are managed
	private void Run() 
	{
		//configure logging
		ConfigureLogging();

		//run everything in a loop to recover from connection errors
		while( true )
		{
			try 
			{
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

				//use random's service
				var rnd = new Random();

				/**
				Generatig water. When it's drinked by the wolf the water respawn in another random point after 5 seconds.
				*/
				// Generating a random volume for the water pool
				int volumeOfWater = rnd.Next(1, 10);
				while( true )
				{	
					// Generating a random position in the map of the water pool
					int xWaterPosition = rnd.Next(1, 50);
					int yWaterPosition = rnd.Next(1, 50);

					// Asking to the server for the max distance range in which the wolf is able to see the pool 
					int maxDist = service.getMaxDistance();
					
					// Notifying client and server about the spawn of the water pool
					log.Info($"A pool with about {volumeOfWater} liters of water is spawning in the coordinates {xWaterPosition}, {yWaterPosition} ...");			
					service.notifySpawn(WATER, xWaterPosition, yWaterPosition);

					// Variable that help me to check if the wolf is so close to the water to drink it
					bool isClose = false;
					do
					{
						// Asking to the server the wolf position and checking the distance between the water pool
						int xDistance = (System.Math.Abs(service.getXWolf() - xWaterPosition));
						int yDistance = (System.Math.Abs(service.getYWolf() - yWaterPosition));
						if(xDistance <= maxDist && yDistance <= maxDist) // If the wolf is so close to drink the water
						{
							// Asking the server to make the wolf drink the water
							service.eatOrDrink(volumeOfWater, 2);
							isClose = true;
						}
						// Otherwise waiting for a while to make the wolf to change position
						else Thread.Sleep(2000);
					}while(!isClose);

					log.Info($"A wolf moved really close to the water and drank it. Respawning the same water in another position...\n");
					// waiting for a while before respawning the water in another position
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

	// Program entry point.
	static void Main(string[] args)
	{
		var self = new ClientWater();
		Console.WriteLine("The generation of the water is starting!\n");
		self.Run();
	}
}
