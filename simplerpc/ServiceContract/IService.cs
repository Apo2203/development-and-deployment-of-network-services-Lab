namespace Services;


/// <summary>
/// Structure for testing pass-by-value calls.
/// </summary>
public class ByValStruct
{
	/// <summary>
	/// Left number.
	/// </summary>
	public int Left{ get; set; }

	/// <summary>
	/// Right number.
	/// </summary>
	/// <value></value>
	public int Right{ get; set; }

	/// <summary>
	/// Left + Right
	/// </summary>
	public int Sum{ get; set; }
}

/// <summary>
/// Service contract.
/// </summary>
public interface IService
{
	int getXWolf();
	int getYWolf();
	int getMaxDistance();
	void eatOrDrink(int quantity, int kindOfFood);
	void notifySpawn(int kindOfObject, int x = 0, int y = 0);
}