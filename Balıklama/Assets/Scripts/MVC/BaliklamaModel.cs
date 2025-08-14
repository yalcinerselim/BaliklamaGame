public class BaliklamaModel
{
    public int bubbleCount;
    public int spawnCount;
    public int speed;

    public void Initialise(GameData gameData, int level)
    {
        bubbleCount = gameData.bubbleCount[level - 1];
        spawnCount = gameData.spawnCount[level - 1];
        speed = gameData.speed[level - 1];
    }
}
