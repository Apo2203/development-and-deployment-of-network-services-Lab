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
class Client
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

				while( true )
				{

					/**
					//test simple call
					var left = rnd.Next(-100, 100);
					var right = rnd.Next(-100, 100);

					log.Info($"Before 'int Add(int, int)': left={left}, right={right}");
					System.Console.WriteLine("provaaaaa");
					var sum = service.AddLiteral(left, right);
					log.Info($"After 'int Add(int, int)': sum={sum}, left={left}, right={right}");
					log.Info("---");

					left = rnd.Next(-100, 100);
					right = rnd.Next(-100, 100);
					var leftAndRight = new ByValStruct() { Left = left, Right = right};

					Thread.Sleep(2000);
					*/

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
		var self = new Client();
		Console.WriteLine("I'm starting the service");
		self.Run();
	}
}
