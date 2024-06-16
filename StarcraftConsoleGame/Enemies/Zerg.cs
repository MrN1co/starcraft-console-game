namespace StarcraftConsoleGame.Enemies;

public abstract class Zerg : Enemy
{
    public bool IsBurrowed;
    
    protected void UnburrowAttack(Entity target)
    {
        Writer.SlowWrite($"{Name} unburrows and attacks {target.Name}!", 50);
        target.TakeDamage((int)Math.Floor(Attack*1.5));
        IsBurrowed = false;
    }

    public override void DisplayStatus()
    {
        if (!IsDead && IsBurrowed)
        {
            Console.Write($"{Name} - ");
            Writer.WriteColor($"Burrowed    ", ConsoleColor.DarkMagenta);
            return;
        }
        base.DisplayStatus();
    }
    
}