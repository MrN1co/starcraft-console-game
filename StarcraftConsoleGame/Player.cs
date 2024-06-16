using System.ComponentModel;
using StarcraftConsoleGame.Enemies;

namespace StarcraftConsoleGame;

public abstract class Player : Entity
{
    private readonly Dictionary<int, int> _experienceDict = new()
    {
        {1, 100},
        {2, 200},
        {3, 300},
        {4, 400},
        {5, 500},
        {6, 600},
        {7, 700},
        {8, 800},
        {9, 900},
        {10, 1000}
    };
    
    private int _level = 1;
    public int Level
    {
        get => _level;
        private set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _level = value;
        }
    }
    
    private int _experience;
    private int Experience
    {
        get => _experience;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _experience = value;
        }
    }
    
    private void GainExperience(int experience)
    {
        Writer.SlowWrite($"You have gained {experience} experience!", 50, ConsoleColor.Yellow);
        Experience += experience;
        if (Experience < _experienceDict[Level]) return;
        
        Experience -= _experienceDict[Level];
        LevelUp();
    }
    
    protected virtual void LevelUp()
    {
        Writer.SlowWrite("You have leveled up!", 75, ConsoleColor.Yellow);
        Level++;
        //TODO:Implement skill points (LevelUpManager)
        CurrentHealth = MaxHealth;
        Attack += 5;
        Defense += 1;
    }
    
    private readonly Inventory _backpack = new Inventory();
    private int _currentPostion;

    public override void DisplayStatus()
    {
        Console.WriteLine($"{Name}: {CurrentHealth}/{MaxHealth} HP");
    }
    
    public override void TakeTurn(List<Entity> entities)
    {
        while (true)
        {
            Console.WriteLine("It's your turn, what would you like to do?");
            Console.WriteLine("1. Attack, 2. Wait");
            var choice = Console.ReadKey(true);
            switch (choice)
            {
                case {Key: ConsoleKey.D1}:
                    if (!ChooseAttack(entities))
                        continue;
                    return;
                case {Key: ConsoleKey.D2}:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            
        }
    }

    protected abstract bool ChooseAttack(List<Entity> entities);
    

    protected override void BasicAttack(Entity target)
    {
        Writer.SlowWrite($"{Name} attacks {target.Name}!", 50);
        base.BasicAttack(target); 
    }

    public void DisplayStats()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Level: {Level} ({Experience}/{_experienceDict[Level]})");
        Writer.WriteLineColor($"Health: {CurrentHealth}/{MaxHealth}", ConsoleColor.Red);
        if(this is Zeratul zeratul)
            Writer.WriteLineColor($"Shield: {zeratul.Shield}/{zeratul.MaxShield}", ConsoleColor.Blue);
        Console.WriteLine($"Attack: {Attack}");
        Console.WriteLine($"Defense: {Defense}");
        Console.WriteLine($"Speed: {Speed}");
    }

    public void Scout()
    {
        Writer.SlowWrite($"The next room is a: {GameManager.ActiveMap.GetNextNode(_currentPostion).ToString()}", 75);
    }


    public void MoveUp()
    {
        var room = GameManager.ActiveMap.GetNextNode(_currentPostion);
        switch (room)
        {
            case Map.MapNode.Empty:
                Writer.SlowWrite("You move forward... this place seems to be desolate.", 75);
                _currentPostion++;
                break;
            case Map.MapNode.RandomEncounter:
                Writer.SlowWrite("You encounter a group of enemies! Defend yourself!", 75);
                BattleManager.RandomEncounter();
                _currentPostion++;
                break;
            case Map.MapNode.BossFight:
                Writer.SlowWrite("You encounter the boss!", 75);
                BattleManager.Encounter([new Ultralisk()]);
                _currentPostion++;
                break;
            // case Map.MapNode.UpgradeSpot:
            //     Writer.SlowWrite("You find an upgrade spot!", 75);
            //     Map.UpgradeSpot();
            //     _currentPostion++;
            //     break;
            case Map.MapNode.Starship:
                Writer.SlowWrite("You reach the starship!", 75);
                GameManager.Victory();
                break;
            case Map.MapNode.Entrance:
                Writer.SlowWrite("You can't go back!", 75);
                break;
            default:
                throw new InvalidEnumArgumentException();
        }
        
    }

    public void GainLoot(int experience, int minerals, int gas)
    {
        GainExperience(experience);
        _backpack.GainMinerals(minerals);
        _backpack.GainGas(gas);
    }
}
