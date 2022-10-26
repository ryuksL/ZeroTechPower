using System.Linq;
using Verse;

namespace LingGame;

public class Building_Powwer : Building
{
    public override void Tick()
    {
        base.Tick();
        foreach (var item in Find.CurrentMap.listerBuildings.allBuildingsColonist)
        {
            if (item.PowerComp != null)
            {
                item.PowerComp.PowerNet.powerComps.First().PowerOn = true;
            }
        }
    }
}