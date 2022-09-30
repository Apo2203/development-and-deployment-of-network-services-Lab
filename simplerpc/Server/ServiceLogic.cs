namespace Servers;

using NLog;

using Services;


/// <summary>
/// Service logic.
/// </summary>
public class ServiceLogic : IService
{
	// Max amount of food/water a wolf can eat/drink.
	public const int maxWolfFood = 100;
	/// Minimum distance that make the wolf see a rabbit to eat or water to drink.
	public const int maxDistance = 10;

	private Logger log = LogManager.GetCurrentClassLogger();

	// Amount of food that wolf can eat. Default is 100. It decremeant each time the wolf eat or drink. 
	// It will be also reset when it became 0.
	int wolfFoodLeft = maxWolfFood;
	//The maximum amount of step the wolf is able to do in a single 'round'"
	int wolfStep = 10;
	
	// A flag useful to know if the "wolfFoodLeft" variable is in use from another Client.
	bool isAlreadyEating = false;

	// Coordinate in the space of walf position. From 0 to 50 where the spawn point is 0, 0.
	int xWolfPosition = 0;
	int yWolfPosition = 0;

	int var = 111;
	

	// useful for the generation of random coordinates
	Random rnd = new Random();
	

	/// <summary>
	/// Add given numbers.
	/// </summary>
	/// <param name="left">Left number.</param>
	/// <param name="right">Right number.</param>
	/// <returns>left + right</returns>
	public int AddLiteral(int left, int right)
	{ 
		log.Info($"AddLiteral({left}, {right})");
		System.Console.WriteLine("Add literal");
		return left + right;
	}

	public void generateWolfPosition(){
		
		// moving the wolf in a range of max 5 steps from his position
		int xWolfMovement, yWolfMovement;

		do xWolfMovement = rnd.Next((xWolfPosition - wolfStep), (xWolfPosition + wolfStep));
		while(xWolfMovement < 0 || xWolfMovement > 50);

		do yWolfMovement = rnd.Next((yWolfPosition - wolfStep), (yWolfPosition + wolfStep));
		while(yWolfMovement < 0 || yWolfMovement > 50);
		
		xWolfPosition = xWolfMovement;
		yWolfPosition = yWolfMovement;
		//xWolfPosition = rnd.Next(1, 50);
		//yWolfPosition = rnd.Next(1, 50);
        log.Info($"Wolf position -> x: {xWolfPosition} y: {yWolfPosition}. Food left -> {wolfFoodLeft}");
		Thread.Sleep(7500);
	}

	void eatOrDrink(int quantity)
	{
		while(isAlreadyEating){} // Just do nothing, wait for the mutual exclusion.
		isAlreadyEating = true; // take mutual exclusion

		wolfFoodLeft = wolfFoodLeft - quantity;
		if (wolfFoodLeft <= 0){ // The wolf eaten more than it could
			wolfFoodLeft = maxWolfFood; // resetting the food the wolf can eat
			log.Info("The wolf ate and drank more the he could. Now he has to wait a while before starting eating or drinking again...\n");
			Thread.Sleep(30000); // I wait 5 seconds holding the mutual exclusion
		}
		isAlreadyEating = false; // release mutual exclusion
	}

	public int generateRubbit(int rubbitWeight){
		log.Info($"var prima = {var}");
		var = 222;
		log.Info($"var dopo = {var}");
		while(true){}
		log.Info("A rubbit spawned in the server");
		// just inizialing the variable.
		int rubbitDistance = -1;		
		// If the rubbit is eaten by the wolf;
		bool isEaten = false; 
		do{
			//Rubbit distance from the wolf
			rubbitDistance = rnd.Next(1, 100);
			if(rubbitDistance < maxDistance){
				log.Info($"\n!!! A rubbit with a weight of {rubbitWeight} kg moved too close to the walf and was eaten!!!\n");
				eatOrDrink(rubbitWeight);
				isEaten = true;
			}
			else{ // If the rubbit is not so close to the wolf it just moved (and so generated again random value about distance) after some seconds
				Thread.Sleep(2000); 
			} 
		}while(!isEaten);

		return rubbitDistance;
	}

	public void generateWater(int xWaterPosition, int yWaterPosition, int litres)
	{
		log.Info($"var prima acqua = {var}");
		var = 333;
		log.Info($"var dopo acqua = {var}");
		log.Info($"A pool with about {litres} liters of water is spawned in the server at x = {xWaterPosition} y = {yWaterPosition}");
		// check periodically if the wolf is approaching the water 
		bool isClose = false;
		do{
			int xDistance = (System.Math.Abs(xWolfPosition - xWaterPosition));
			int yDistance = (System.Math.Abs(yWolfPosition - yWaterPosition));
			log.Info($"Distanza dall'acqua uguale a {xDistance} {yDistance}");
			if(xDistance <= maxDistance && yDistance <= maxDistance)
			{
				log.Info($"\n!!! The wolf moved too close to a water pull of {litres} litres and drank it!!!\n");
				eatOrDrink(litres);
				isClose = true;
			}
			else Thread.Sleep(2000);
		}while(!isClose);
		
	}


	/// <summary>
	/// Add given numbers.
	/// </summary>
	/// <param name="leftAndRight">Numbers to add.</param>
	/// <returns>Left + Right in Sum</returns>
	public ByValStruct AddStruct(ByValStruct leftAndRight)
	{
		//log.Info($"AddStruct(ByValStruct(Left={leftAndRight.Left}, Right={leftAndRight.Right}))");
		leftAndRight.Sum = leftAndRight.Left + leftAndRight.Right;
		return leftAndRight;
	}
}