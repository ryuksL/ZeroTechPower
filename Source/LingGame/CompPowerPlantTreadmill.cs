using RimWorld;

namespace LingGame;

public class CompPowerPlantTreadmill : CompPowerPlant
{
    public override void CompTickRare()
    {
        base.CompTickRare();
        PowerOutput = 0f;
    }
}