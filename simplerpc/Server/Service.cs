namespace Servers;

using Services;


// <summary>
// Service
// </summary>
public class Service : IService
{
	// <summary>
	// Access lock.
	// </summary>
	public static readonly Object accessLock = new Object();

	// <summary>
	// Service logic implementation.
	// </summary>
	public static ServiceLogic logic = new ServiceLogic();

	public static void generateWolfPosition()
	{
		lock ( accessLock )
		{
			logic.generateWolfPosition();
		}
	}

	public int getXWolf(){
		lock ( accessLock )
		{
			return logic.getXWolf();
		}
	}
	public int getYWolf(){
		lock ( accessLock )
		{
			return logic.getYWolf();
		}
	}

	public int getMaxDistance(){
		lock ( accessLock )
		{
			return logic.getMaxDistance();
		}
	}

	public bool eatOrDrink(int quantity, int kindOfFood)
	{
		lock ( accessLock )
		{
			return logic.eatOrDrink(quantity, kindOfFood);
		}
	}

	public void notifySpawn(int kindOfObject, int x, int y)
	{
		lock ( accessLock )
		{
			logic.notifySpawn(kindOfObject, x, y);
		}
	}

	public static void resetFood()
	{
		lock (accessLock)
		{
			logic.resetFood();
		}
	}

	public static bool checkStatus()
	{
		lock(accessLock)
		{
			return logic.checkStatus();
		}
	}
}