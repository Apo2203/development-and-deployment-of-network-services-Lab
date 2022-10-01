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
	public static readonly TimeSpan InfiniteTimeSpan;
	public const int RUBBIT = 1;

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

				//use service
				var rnd = new Random();

				/**
				Generate a rubbit. When it dead (when the function that generate it end) I wait 5 sec and then I generate another one.
				*/
				int rubbitWeight = rnd.Next(1, 20);
				while( true )
				{
					log.Info($"A rubbit with a weight of {rubbitWeight} kg was spawned in the server");
					service.notifySpawn(RUBBIT, rubbitWeight);
					// just inizialing the variable.
					int rubbitDistance = -1;		
					// If the rubbit is eaten by the wolf;
					bool isEaten = false; 
					do{
						//Rubbit distance from the wolf
						rubbitDistance = rnd.Next(1, 20);
						if(rubbitDistance < service.getMaxDistance()){
							log.Info($"The rubbit moved too close to the wolf ({rubbitDistance} metres) and was eaten.\n");
							service.eatOrDrink(rubbitWeight, 1);
							isEaten = true;
						}
						else{ // If the rubbit is not so close to the wolf it just moved (and so generated again random value about distance) after some seconds
							Thread.Sleep(2000); 
						} 
					}while(!isEaten);

					// Rubbit eaten. Let's respawn him after 5 seconds.
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
