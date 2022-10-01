﻿namespace Services;


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
	/// <summary>
	/// Add given numbers.
	/// </summary>
	/// <param name="left">Left number.</param>
	/// <param name="right">Right number.</param>
	/// <returns>left + right</returns>
	int AddLiteral(int left, int right);

	int getXWolf();
	int getYWolf();
	int getMaxDistance();
	void eatOrDrink(int quantity, int kindOfFood);
	void notifySpawn(int kindOfObject, int x = 0, int y = 0);

	/// <summary>
	/// Add given numbers.
	/// </summary>
	/// <param name="leftAndRight">Numbers to add.</param>
	/// <returns>Left + Right in Sum</returns>
	ByValStruct AddStruct(ByValStruct leftAndRight);
}