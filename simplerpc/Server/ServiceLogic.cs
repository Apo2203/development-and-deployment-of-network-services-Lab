namespace Servers;

using NLog;

using Services;


// Service logic.
public class ServiceLogic : IService
{
	// Max amount of food/water a wolf can eat/drink.
	public const int MAXWOLFFOOD = 100;
	// Minimum range that make the wolf see a rabbit to eat or water to drink.
	public const int MAXDISTANCE = 10;
	
	// Constant that help me to relate to rabbit / water
	public const int RABBIT = 1;
	public const int WATER = 2;
	
	//The maximum amount of step the wolf is able to do in a single 'movement'
	public const int WOLFSTEP = 10;

	private Logger log = LogManager.GetCurrentClassLogger();

	// Amount of food that wolf can eat. It decremeant each time the wolf eat or drink. 
	// It will be also reset when it became 0.
	int wolfFoodLeft = MAXWOLFFOOD;


	// Coordinate in the space of walf position. From 0 to 50 where the spawn point is 0, 0.
	int xWolfPosition = 0;
	int yWolfPosition = 0;
	

	// Useful for the generation of random values
	Random rnd = new Random();

	// Function that generate the coordinates fot the wolf position.  
	public void generateWolfPosition()
	{
		// moving the wolf in a range of max 'WOLFSTEP' steps from his position
		int xWolfMovement, yWolfMovement;

		// calculating the new position
		do xWolfMovement = rnd.Next((xWolfPosition - WOLFSTEP), (xWolfPosition + WOLFSTEP));
		while(xWolfMovement < 0 || xWolfMovement > 50);

		do yWolfMovement = rnd.Next((yWolfPosition - WOLFSTEP), (yWolfPosition + WOLFSTEP));
		while(yWolfMovement < 0 || yWolfMovement > 50);
		
		//Updating the variable linked to the position of the wolf
		xWolfPosition = xWolfMovement;
		yWolfPosition = yWolfMovement;

		//Notifying the server about the wolf position and other useful information.
        log.Info($"Wolf position -> x: {xWolfPosition} y: {yWolfPosition}. Food left -> {wolfFoodLeft}");
		Thread.Sleep(7500);
	}

	//Function called when the wolf is so close to a rabbit or a water pool to eat it.
	public void eatOrDrink(int quantity, int kindOfFood)
	{

		wolfFoodLeft = wolfFoodLeft - quantity;
		
		//Notifying what the wolf is eating or drinking to the server
		if(kindOfFood == RABBIT) 
		{
			log.Info($"A rabbit with a weight of {quantity} kg moved too close to the walf and was eaten!!!\n");
		}
		else if(kindOfFood == WATER)
		{
			log.Info($"The wolf found a water pull of {quantity} litres and drank it!!!\n");
		}
		else log.Info("\nFATAL ERROR DURING EATORDRINK FUNCTION\n");

		if (wolfFoodLeft <= 0)
		{ // The wolf eaten more than it could
			wolfFoodLeft = MAXWOLFFOOD; // resetting the food the wolf can eat
			log.Info("The wolf ate and drank more the he could. Now he has to wait a while before starting eating or drinking again...\n");
			Thread.Sleep(5000); // I wait 5 seconds holding the mutual exclusion
		}
	}

	//Function called to notify when a rabbit or a water pool spawn in the server
	public void notifySpawn(int kindOfObject, int x = 0, int y = 0){
		if(kindOfObject == RABBIT) 
		{
			log.Info($"A rabbit with a weight of {x} kg just spawned in the server!");
		}
		else if(kindOfObject == WATER)
		{
			log.Info($"A pool of water just spawned in the server at the coordinates x:{x} y:{y}");
		}
		else log.Info("\nFATAL ERROR DURING NOTIFYSPAWN FUNCTION\n");
	}

	// A way to takes useful variable from the server to the clients.
	public int getXWolf(){ return xWolfPosition; }
	public int getYWolf(){ return yWolfPosition; }
	public int getMaxDistance() { return MAXDISTANCE; }

}