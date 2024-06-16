

namespace StarcraftConsoleGame;

public abstract class Entity 
{
    public bool IsDead ;
    
    private int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        protected set
        {
            if (value > _maxHealth)
            {
                _currentHealth = _maxHealth;
                return;
            }
            if (value <= 0)
            {
                _currentHealth = 0;
                Die();
            }
            _currentHealth = value;
        }
    }

    private int _maxHealth;
    public int MaxHealth
    { 
        get => _maxHealth;
        protected set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _maxHealth = value;
        }
    }

    private int _attack;
    protected int Attack
    {
        get => _attack;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _attack = value;
        }
    }
    
    private int _defense;

    protected int Defense
    {
        get => _defense;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _defense = value;
        }
    }
    
    public int Speed { get; protected set; }

    public string Name { get; protected init; } = "NoNameEntity";
    
    public virtual void TakeDamage(int damage)
    {
        var postMitigationDamage = damage - Defense;
        if(postMitigationDamage > 0)
        {
            Writer.SlowWrite($"{Name} took {postMitigationDamage} damage!", 50);
            CurrentHealth -= postMitigationDamage;
            return;
        }
        
        Writer.SlowWrite($"{Name} took no damage!", 50);
    }

    protected virtual void BasicAttack(Entity target)
    {
        target.TakeDamage(Attack);
    }

    protected virtual void Die()
    {
        IsDead = true;
    }

    public abstract void TakeTurn(List<Entity> entities);


    public virtual void DisplayStatus()
    {
        if (IsDead)
        {
            Console.Write($"{Name}: DEAD   ");
            return;
        }
        Writer.WriteColor($"{Name}: ", ConsoleColor.White);
        Writer.WriteColor($"{CurrentHealth}/{MaxHealth} HP  ", ConsoleColor.Red);
    }
}