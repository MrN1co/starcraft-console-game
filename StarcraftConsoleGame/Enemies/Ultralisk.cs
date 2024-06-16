namespace StarcraftConsoleGame.Enemies;

public class Ultralisk : Zerg
{
    public Ultralisk()
    {
        Name = "Ultralisk";
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
        Attack = 30;
        Speed = 5;
        ExperienceValue = 200;
        
        var random = new Random();
        MineralValue = random.Next(100, 151);
        GasValue = random.Next(50, 101);
    }
    
    private int _storedDamage; 
    public override void TakeTurn(List<Entity> entities)
    {
        if (IsDead)
            return;
        
        //Find player in the battling entities
        if (entities.First(entity => entity is Player) is not Player player)
        {
            throw new NullReferenceException();
        }
        
        if(CurrentHealth < MaxHealth * 0.25)
        {
            TissueAssimilation();
            return;
        }
        
        var random = new Random();
        var action = random.Next(2);
        if (CurrentHealth < MaxHealth * 0.50 && player is Zeratul && action == 0)
        {
            ShieldBreak(player);
            return;
        }
        
        Writer.SlowWrite($"{Name} attacks {player.Name}!", 50);
        BasicAttack(player);
    }

    protected override void BasicAttack(Entity target)
    {
        base.BasicAttack(target);
        _storedDamage+= (int)Math.Floor(Attack*0.4);
    }

    private void TissueAssimilation()
    {
        Writer.SlowWrite($"{Name} uses Tissue Assimilation!", 50);
        if(_storedDamage > MaxHealth - CurrentHealth)
            _storedDamage = MaxHealth - CurrentHealth;
        Writer.SlowWrite($"{Name} healed for {_storedDamage} health", 50);
        CurrentHealth += _storedDamage;
        _storedDamage = 0;
    }
    
    private void ShieldBreak(Player target)
    {
        Writer.SlowWrite($"{Name} used Shield Break on {target.Name}!", 50);
        if (target is Zeratul zeratul)
            zeratul.TakeDamage(zeratul.Shield);
        else
            throw new ArgumentException("Target of shield break is not Zeratul!");
    }
}