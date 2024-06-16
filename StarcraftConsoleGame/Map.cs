namespace StarcraftConsoleGame;

public class Map
{

    // ReSharper disable once NotAccessedField.Local
    private readonly string _name;
    public enum MapNode
    {
        Empty,
        RandomEncounter,
        BossFight,
        UpgradeSpot,
        Starship,
        Entrance
    }


    private readonly int _mapSize;
    private readonly MapNode[] _mapArray;
    
    public Map(int mapSize, string name)
    {
        _name = name;
        
        _mapSize = mapSize;
        
        _mapArray = new MapNode[mapSize];
        _mapArray[0] = MapNode.Entrance;
        _mapArray[mapSize - 1] = MapNode.Starship;
        _mapArray[mapSize - 2] = MapNode.BossFight;
        //_mapArray[mapSize - 3] = MapNode.UpgradeSpot;
        GenerateRandomMap();
    }
    
    
    private void GenerateRandomMap()
    {
        var random = new Random();
        for (var i = 1; i < _mapSize - 3; i++)
        {
            _mapArray[i] = random.Next(0, 100) switch
            {
                < 20 => MapNode.Empty,
                _ => MapNode.RandomEncounter
            };
        }
    }
    
    public MapNode GetNextNode(int currentPosition)
    {
        return _mapArray[currentPosition + 1];
    }
    
    
}