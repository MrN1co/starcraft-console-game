using StarcraftConsoleGame.Enemies;

namespace StarcraftConsoleGame;

public class Zeratul : Player
{
    private int _shield;
    public int Shield
    {
        get => _shield;
        private set
        {
            if (value > _maxShield)
            {
                _shield = _maxShield;
                return;
            }
            
            //if the damage breaks the shield, deal the leftover damage to player health
            if (value < 0)
            {
                CurrentHealth += value;
                _shield = 0;
                return;
            }
            _shield = value;
        }
    }

    private int _maxShield;
    public int MaxShield
    {
        get => _maxShield;
        private set
        {
            if (value < 0)
            {
                _maxShield = 0;
                return;
            }
            _maxShield = value;
        }
    }
    
    private double _shieldVulnerability;
    private double ShieldVulnerability
    {
        get => _shieldVulnerability;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _shieldVulnerability = value;
        }
    }
    
    public void RefillShield()
    {
        Shield = MaxShield;
    }
    
    private bool _isShieldEmpowered; 
    private const int ShieldEmpowerDuration = 2;
    private int _shieldEmpowerDurationCounter;
    private const int ShieldEmpowerCooldown = 2;
    private int _shieldEmpowerCooldownCounter;
    
    //TODO: Add Energy
    
    public Zeratul()
    {
        Name = "Zeratul";
        MaxHealth = 200;
        MaxShield = 100;
        Shield = MaxShield;
        CurrentHealth = MaxHealth;
        Attack = 10;
        Defense = 5;
        ShieldVulnerability = 1;
        Speed = 10;
    }
    
    protected override void LevelUp()
    {
        base.LevelUp();
        MaxShield += 10;
        if (Level % 3 == 0)
        {
            ShieldVulnerability -= 0.1;
        }
    }
    
    public override void TakeDamage(int damage)
    {
        if (Shield > 0)
        {
            var postProtectionDamage = (int)Math.Ceiling(damage*ShieldVulnerability);
            Writer.SlowWrite($"{Name}'s shield took {postProtectionDamage} damage!", 50);
            Shield -= postProtectionDamage;
        }
        else
        {
            base.TakeDamage(damage);
        }
    }


    protected override bool ChooseAttack(List<Entity> entities)
    {
        while (true)
        {
            Console.WriteLine("Choose an attack: (press 'c' to cancel)");
            Console.WriteLine("1. Basic Attack, 2. Spin, 3. Empower Shield");
            var key = Console.ReadKey(true);
            switch (key)
            {
                case { Key: ConsoleKey.C }:
                    BattleManager.DisplayBattleStatus(entities);
                    return false;
                
                case { Key: ConsoleKey.D1 }:
                    Console.WriteLine("Choose target: (press 'c' to cancel)");
                    var i = 0;
                    foreach (var enemy in entities.Where(entity => entity is Enemy && !entity.IsDead))
                    {
                        if(enemy is Zerg { IsBurrowed: true })
                        {
                            Console.WriteLine($"{i}. {enemy.Name} - Burrowed");
                            i++;
                            continue;
                        }

                        Console.WriteLine($"{i}. {enemy.Name} {enemy.CurrentHealth}/{enemy.MaxHealth} HP, ");
                        i++;
                    }
                    
                    while (true)
                    {
                        var choice = Console.ReadKey(true);
                        if (choice.Key == ConsoleKey.C)
                        {
                            BattleManager.DisplayBattleStatus(entities);
                            return false;
                        }

                        Console.WriteLine("\n");
                        var target = choice.KeyChar - '0';
                        
                        if(target < 0 || target>= entities.Count-1)
                        {
                            Console.WriteLine("Invalid choice!");
                            continue;
                        }

                        if (entities[target] is Zerg { IsBurrowed: true })
                        {
                            Console.WriteLine("Target is buried. Choose another target");
                            continue;
                        }
                        BasicAttack(entities[target]);
                        return true;
                    }
                case { Key: ConsoleKey.D2 }:
                    Spin(entities);
                    return true;
                case { Key: ConsoleKey.D3 }:
                    if(_isShieldEmpowered)
                    {
                        Writer.SlowWrite("Shield is already empowered!", 50);
                        break;
                    }

                    if (_shieldEmpowerCooldownCounter > 0)
                    {
                        Writer.SlowWrite("Shield Empower is on cooldown!", 50);
                        break;
                    }
                    EmpowerShield();
                    return true;
                
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }

            return false;
        }
    }

    public override void TakeTurn(List<Entity> entities)
    {
        if(_shieldEmpowerCooldownCounter > 0)
            _shieldEmpowerCooldownCounter--;
        
        if (_isShieldEmpowered)
        {
            _shieldEmpowerDurationCounter--;
            if (_shieldEmpowerDurationCounter <= 0)
            {
                _isShieldEmpowered = false;
                _shieldEmpowerCooldownCounter = ShieldEmpowerCooldown;
                MaxShield -= 100;
                if(Shield > MaxShield)
                    Shield = MaxShield;
            }
        }
        
        base.TakeTurn(entities);
    }

    private void Spin(List<Entity> enemies)
    {
        foreach (var enemy in enemies.Where(entity => entity is Enemy))
        {
            enemy.TakeDamage(Attack/2);
        }
    }
    
    private void EmpowerShield()
    {
        _isShieldEmpowered = true;
        _shieldEmpowerDurationCounter = ShieldEmpowerDuration;
        Writer.SlowWrite("Your shield has been temporarily empowered.", 50);
        MaxShield += 100;
        Shield += 100;
    }
    
    protected override void Die()
    {
        GameManager.GameOver();
    }
    
    public override void DisplayStatus()
    {
        Writer.WriteColor($"{Name}: ", ConsoleColor.White);
        Writer.WriteColor($"{CurrentHealth}/{MaxHealth} HP  ", ConsoleColor.Red);
        Writer.WriteLineColor($"{Shield}/{MaxShield} Shield ", ConsoleColor.DarkBlue);
    }
    
}