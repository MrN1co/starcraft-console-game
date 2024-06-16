namespace StarcraftConsoleGame.Enemies;

public class Hydralisk : Zerg
{
    private bool _isAttackPoisoned;
    private const int PoisonAttackDuration = 3;
    private int _poisonAttackCounter;
    
    public Hydralisk()
    {
        Name = "Hydralisk";
        MaxHealth = 40;
        CurrentHealth = MaxHealth;
        Attack = 25;
        Speed = 9;
        ExperienceValue = 50;
        
        var random = new Random();
        MineralValue = random.Next(40, 61);
        GasValue = random.Next(15, 31);
    }
    
    public override void TakeTurn(List<Entity> entities)
    {
        if (IsDead)
            return;
        
        //Find player in the battling entities
        if (entities.First(entity => entity is Player) is not Player player)
        {
            throw new NullReferenceException();
        }
        
        if (_isAttackPoisoned)
        {
            _poisonAttackCounter--;
            if (_poisonAttackCounter <= 0)
            {
                _isAttackPoisoned = false;
                Attack -= 5;
            }
        }

        //If the Zerg is burrowed, unburrow and attack
        if (IsBurrowed)
        {
            UnburrowAttack(player);
            return;
        }

        //Randomly choose an action
        var random = new Random();
        int action;
        
        if (!_isAttackPoisoned)
        {
            action = random.Next(3);
            if(action == 0)
            {
                AddPoison();
                return;
            }
        }
        
        action = random.Next(1, 3);
        switch (action)
        {
            case 1:
                Writer.SlowWrite($"{Name} attacks {player.Name}!", 50);
                BasicAttack(player);
                break;
            case 2:
                Writer.SlowWrite($"{Name} burrows!", 50);
                IsBurrowed = true;
                break;
                
        }
    }
    
    private void AddPoison()
    {
        Writer.SlowWrite($"{Name} uses Poisonous Secretion!", 50, ConsoleColor.Green);
        Writer.SlowWrite("Their damage has been increased!", 50);
        _isAttackPoisoned = true;
        _poisonAttackCounter = PoisonAttackDuration;
        Attack += 5;
    }
}