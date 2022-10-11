namespace Clients;

using Microsoft.Extensions.DependencyInjection;

using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;

using NLog;

using Services;

// Client that implement the rabbit
class Clientrabbit
{
	// Logger for this class.
	Logger log = LogManager.GetCurrentClassLogger();

	// constant variable that help me to relate to the rabbit
	public const int RABBIT = 1;

	// Configures logging subsystem.
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

	// Program body.
	private void Run()
	{
		//configure logging
		ConfigureLogging();

		//run everythin in a loop to recover from connection errors
		while( true )
		{
			try 
			{
				//connect to the server, get service client proxy
				//HttpClient httpClient = new HttpClient();
				//httpClient.Timeout = InfiniteTimeSpan;
				var sc = new ServiceCollection();
				
				sc
					.AddSimpleRpcClient(
						"service", 
						new HttpClientTransportOptions
						{
							Url = "http://127.0.0.1:5000/simplerpc",
							Serializer = "HyperionMessageSerializer",
						}
						
						
					)
					.AddSimpleRpcHyperionSerializer();
				
				sc.AddSimpleRpcProxy<IService>("service");

				
				var sp = sc.BuildServiceProvider();

				var service = sp.GetService<IService>();

				//use random's service
				var rnd = new Random();

				/**
				Generating rabbits. When it dead it will be respawned in another point after 5 seconds.
				*/
				while( true )
				{
					//Generating a random value for the rabbit weight
					int rabbitWeight = rnd.Next(1, 20);
					//Notifying client and server about the spawn of the rabbit
					log.Info($"A rabbit with a weight of {rabbitWeight} kg was spawned in the server");
					service.notifySpawn(RABBIT, rabbitWeight);

					// inizialing the variable that will decide if the wolf is to close to the rabbit to eat it.
					int rabbitDistance = -1;	

					// If the rabbit is eaten by the wolf;
					bool isEaten = false; 
					do
					{
						//generating random distance from the wolf
						rabbitDistance = rnd.Next(1, 50);

						if(rabbitDistance < service.getMaxDistance())
						{ //if the wolf is so close to the rabbit to eat it
							log.Info($"The rabbit moved too close to the wolf ({rabbitDistance} metres) and was eaten.\n");

							if (service.eatOrDrink(rabbitWeight, RABBIT) == 1) // The walf eat more than he could.
							{
								// Wait 5 seconds then allow the wolf to eat or drink again.
								Thread.Sleep(5000);
								service.resetFood();
							}
							isEaten = true;
						}
						else
						{ // If the rabbit is not so close to the wolf it just moved (and so generated again random value about distance) after some seconds
							Thread.Sleep(3000); 
						} 
					}while(!isEaten);

					// rabbit eaten. Let's respawn him after 5 seconds.
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
		var self = new Clientrabbit();
		
		Console.WriteLine("The generation of the rabbits is starting!\n");
		self.Run();
	}
}
