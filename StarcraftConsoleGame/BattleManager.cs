using StarcraftConsoleGame.Enemies;

namespace StarcraftConsoleGame;

public static class BattleManager
{
    private static int _gatheredExperience;
    private static int _gatheredMinerals;
    private static int _gatheredGas;
    
    public static void RandomEncounter()
    {
        List<Entity> entities = [];
        
        var random = new Random();
        
        var count = random.Next(1, GameManager.ActivePlayer.Level + 3);
        for (var i = 0; i < count; i++)
        {
            Enemy enemy = random.Next(0, 100) switch
            {
                < 70 => new Zergling(),
                _ => new Hydralisk()
            };
            //Enemy enemy = new Zergling();
            entities.Add(enemy);
        }
        
        entities.Add(GameManager.ActivePlayer);
        BattleLoop(entities);
        
    }
    
    public static void Encounter(List<Entity> entities)
    {
        entities.Add(GameManager.ActivePlayer);
        BattleLoop(entities);
    }
    
    private static void BattleLoop(List<Entity> entities)
    {
        _gatheredExperience = 0;
        _gatheredMinerals = 0;
        _gatheredGas = 0;
        DisplayBattleStatus(entities);
        
        while(entities.Any(entity => entity is Enemy))
        {
            var turnOrder = CheckInitiative(entities);
            while(turnOrder.Count > 0)
            {
                var entity = turnOrder.Dequeue();
                entity.TakeTurn(entities);
                RemoveBodies(entities);
                Thread.Sleep(500);
                DisplayBattleStatus(entities);
            }
        }
        EndBattle();
    }

    private static Queue<Entity> CheckInitiative(IEnumerable<Entity> entities)
    {
        var initiativeQueue = new Queue<Entity>(entities.OrderByDescending(entity => entity.Speed));
        return initiativeQueue;
    }

    private static void RemoveBodies(List<Entity> entities)
    {
        foreach(var deadEnemy in entities.OfType<Enemy>().Where(enemy => enemy.IsDead))
        {
            _gatheredExperience += deadEnemy.ExperienceValue;
            _gatheredMinerals += deadEnemy.MineralValue;
            _gatheredGas += deadEnemy.GasValue;
        }

        entities.RemoveAll(entity => entity.IsDead);
    }
    
    public static void DisplayBattleStatus(List<Entity> entities)
    {
        Console.Clear();
        entities.Find(entity => entity is Player)!.DisplayStatus();
        foreach (var entity in entities.Where(entity => entity is Enemy))
        {
            entity.DisplayStatus();
        }
        
        Console.WriteLine("\n\n");
    }

    private static void EndBattle()
    {
        Writer.SlowWrite("The battle is won!", 75);
        GameManager.ActivePlayer.GainLoot(_gatheredExperience, _gatheredMinerals, _gatheredGas);

        if(GameManager.ActivePlayer is Zeratul zeratul)
            zeratul.RefillShield();
        
        Console.WriteLine("Press any key to continue...");
    }
    
    
    
}