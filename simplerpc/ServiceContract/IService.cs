namespace Services;
// Service contract.
public interface IService
{
	void generateWolfPosition();
	int getXWolf();
	int getYWolf();
	int getMaxDistance();
	bool eatOrDrink(int quantity, int kindOfFood);
	void notifySpawn(int kindOfObject, int x = 0, int y = 0);
	void resetFood();
	bool checkStatus();
}