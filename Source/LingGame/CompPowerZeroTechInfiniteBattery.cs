using RimWorld;
using Verse;

namespace LingGame
{
    public class CompPowerZeroTechInfiniteBattery : ThingComp
    {
        public float IdleCapacity;

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref IdleCapacity, "IdleCapacity");
        }

        public override string CompInspectStringExtra()
        {
            return string.Concat("IdleCapacity".Translate(), IdleCapacity.ToString());
        }

        public override void CompTick()
        {
            var compPowerBattery = parent.TryGetComp<CompPowerBattery>();
            if (IdleCapacity > 0f)
            {
                compPowerBattery.Props.efficiency = (100f - IdleCapacity) / 100f;
                if (compPowerBattery.Props.efficiency <= 0.1f)
                {
                    compPowerBattery.Props.efficiency = 0.1f;
                }
            }

            base.CompTick();
            if (compPowerBattery.StoredEnergyPct >= 0.99f)
            {
                compPowerBattery.SetStoredEnergyPct(0.1f);
                IdleCapacity += 1f;
            }
            else if (compPowerBattery.StoredEnergyPct <= 0.05f && IdleCapacity > 0f)
            {
                compPowerBattery.SetStoredEnergyPct(0.98f);
                IdleCapacity -= 1f;
            }
        }
    }
}