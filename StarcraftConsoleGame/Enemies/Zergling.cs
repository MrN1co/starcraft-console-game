namespace StarcraftConsoleGame.Enemies;

public class Zergling : Zerg
{
    private bool _isBoosted;
    private const int BoostDuration = 3;
    private int _boostRemaining;
    
    public Zergling()
    {
        Name = "Zergling";
        MaxHealth = 20;
        CurrentHealth = MaxHealth;
        Attack = 15;
        Speed = 15;
        ExperienceValue = 30;
        
        var random = new Random();
        MineralValue = random.Next(10, 21);
        GasValue = 0;

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
        
        //If the Zergling has Metabolic Boost active, decrement the remaining turns
        if (_isBoosted)
        {
            _boostRemaining--;
            if(_boostRemaining <= 0)
            {
                _isBoosted = false;
                Speed -= 5;
            }
        }
        
        //If the Zergling is burrowed, unburrow and attack
        if (IsBurrowed)
        {
            UnburrowAttack(player);
            return;
        }


        //Randomly choose an action
        var random = new Random();
        var action = random.Next(1, 4);
        switch (action)
        {
            case 1:
                Writer.SlowWrite($"{Name} attacks {player.Name}!", 50);
                BasicAttack(player);
                break;
            case 2:
                Writer.SlowWrite($"{Name} burrows!", 50, ConsoleColor.DarkMagenta);
                IsBurrowed = true;
                break;
            case 3:
                MetabolicBoost();
                break;
        }
    }

    private void MetabolicBoost()
    {
        Writer.SlowWrite($"{Name} uses Metabolic Boost!", 50, ConsoleColor.Green);
        Writer.SlowWrite("Their speed has been increased!", 50);
        _isBoosted = true;
        _boostRemaining += BoostDuration;
        Speed += 5;
    }
}