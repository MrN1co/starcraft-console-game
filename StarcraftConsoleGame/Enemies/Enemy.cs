namespace StarcraftConsoleGame.Enemies;

public abstract class Enemy : Entity
{
    
    public int ExperienceValue { get; protected init; }
    public int MineralValue { get; protected init; }
    public int GasValue { get; protected init; }
    protected override void Die()
    {
        Writer.SlowWrite($"{Name} has been slain!", 50);
        base.Die();
    }
    
}