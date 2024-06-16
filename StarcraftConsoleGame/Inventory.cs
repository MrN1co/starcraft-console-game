namespace StarcraftConsoleGame;

public class Inventory
{
    private const int StarterMinerals = 50;
    private const int StarterGas = 0;
    
    public Inventory()
    {
        Minerals = StarterMinerals;
        Gas = StarterGas;
    }
    public Inventory(int minerals, int gas)
    {
        Minerals = minerals;
        Gas = gas;
    }
    
    
    private int _minerals;
    public int Minerals
    {
        get => _minerals;
        private set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _minerals = value;
        }
    }
    
    private int _gas;
    public int Gas
    {
        get => _gas;
        private set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _gas = value;
        }
    }

    public bool CanBuy((int mineralCost, int gasCost) cost)
    {
        if(Minerals < cost.mineralCost || Gas < cost.gasCost)
            return false;
        Minerals -= cost.mineralCost;
        Gas -= cost.gasCost;
        return true;
    }
    
    public void GainMinerals(int minerals)
    {
        if (minerals == 0)
            return;
        Writer.SlowWrite($"You have gained {minerals} minerals!", 75, ConsoleColor.Cyan);
        Minerals += minerals;
    }
    
    public void GainGas(int gas)
    {
        if (gas == 0)
            return;
        Writer.SlowWrite($"You have gained {gas} vespene gas!", 75, ConsoleColor.DarkGreen);
        Gas += gas;
    }
}
