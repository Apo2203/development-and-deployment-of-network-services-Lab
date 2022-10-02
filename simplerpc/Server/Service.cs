namespace Servers;

using Services;


/// <summary>
/// Service
/// </summary>
public class Service : IService
{
	/// <summary>
	/// Access lock.
	/// </summary>
	public static readonly Object accessLock = new Object();

	/// <summary>
	/// Service logic implementation.
	/// </summary>
	public static ServiceLogic logic = new ServiceLogic();


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

	public void eatOrDrink(int quantity, int kindOfFood)
	{
		lock ( accessLock )
		{
			logic.eatOrDrink(quantity, kindOfFood);
		}
	}

	public void notifySpawn(int kindOfObject, int x, int y)
	{
		lock ( accessLock )
		{
			logic.notifySpawn(kindOfObject, x, y);
		}
	}

}