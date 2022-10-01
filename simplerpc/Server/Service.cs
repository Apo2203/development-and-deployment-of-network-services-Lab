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


	/// <summary>
	/// Add given numbers.
	/// </summary>
	/// <param name="left">Left number.</param>
	/// <param name="right">Right number.</param>
	/// <returns>left + right</returns>
	public int AddLiteral(int left, int right)
	{
		lock( accessLock )
		{
			return logic.AddLiteral(left, right);
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

	/// <summary>
	/// Add given numbers.
	/// </summary>
	/// <param name="leftAndRight">Numbers to add.</param>
	/// <returns>Left + Right in Sum</returns>
	public ByValStruct AddStruct(ByValStruct leftAndRight)
	{
		lock( accessLock )
		{
			return logic.AddStruct(leftAndRight);
		}
	}
}