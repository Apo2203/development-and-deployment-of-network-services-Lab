namespace Servers;

using NLog;

using Services;


/// <summary>
/// Service logic.
/// </summary>
public class ServiceLogic : IService
{
	/// <summary>
	/// Logger for this class.
	/// </summary>
	private Logger log = LogManager.GetCurrentClassLogger();

	// Amount of food that wolf can eat. Default is 100. It decremeant each time the wolf eat or drink. 
	// It will be also reset when it became 0.
	int wolfFoodLeft = 100;
	
	// A flag useful to know if the "wolfFoodLeft" variable is in use from another Client.
	bool isAlreadyEating = false;
	/// Minimum distance that make the wolf see a rabbit to eat or water to drink.
	int maxDistance = 5;

	// Coordinate in the space of walf position. From 0 to 50 where the spawn point is 0.
	int xWolfPosition = -1;
	int yWolfPosition = -1;
	

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
        xWolfPosition = rnd.Next(50);
        yWolfPosition = rnd.Next(50);
        log.Info($"Wolf position -> x: {xWolfPosition} y: {yWolfPosition}");
	}

	public void generateRubbit(){

		int rubbitWeight = rnd.Next(1, 20);
		
		// If the rubbit is eaten by the wolf;
		bool isEaten = false; 
		log.Info($"Rubbit generated with a weight of ({rubbitWeight}) kg\n");
		do{
			//Rubbit distance from the wolf
			int rubbitDistance = rnd.Next(1, 30);
			log.Info($"Actually the distance from the wolf is ({rubbitDistance})\n");

			if(rubbitDistance < maxDistance){
				log.Info("The rubbit is too close to the wolf, is gonna be eated!\n");

				while(isAlreadyEating){} // Just do nothing, wait for the mutual exclusion.

				isAlreadyEating = true; // take mutual exclusion
				isEaten = true;

				wolfFoodLeft = wolfFoodLeft - rubbitWeight;
				if (wolfFoodLeft < 0){ // The wolf eaten more than it could
					Thread.Sleep(5000); // I wait 5 seconds holding the mutual exclusion
					wolfFoodLeft = 100; // resetting the food the wolf can eat
				}
				isAlreadyEating = false; // release mutual exclusion
			}
			else{ // If the rubbit is not so close to the wolf it just moved (and so generated again random value about distance) after some seconds
				Thread.Sleep(2000); 
			} 
		}while(!isEaten);

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