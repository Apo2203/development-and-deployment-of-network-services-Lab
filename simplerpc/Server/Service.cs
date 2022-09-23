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
	private readonly Object accessLock = new Object();

	/// <summary>
	/// Service logic implementation.
	/// </summary>
	private ServiceLogic logic = new ServiceLogic();


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

	public void generateWolfPosition(){
		lock(accessLock){
			System.Console.WriteLine("Wolf in service");
			logic.generateWolfPosition();
			System.Console.WriteLine("after call in service");
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