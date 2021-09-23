using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LingGame
{
    public class LingLongRandPowerNeeder : Building
    {
        public CompPowerTrader CCompPowerPlant;
        public LingLongRandPowerGiver powerGiver;

        public float UseePower;

        public override string LabelCap => base.LabelCap + thingIDNumber;

        public override string GetInspectString()
        {
            if (powerGiver == null)
            {
                return base.GetInspectString();
            }

            var inspectString = base.GetInspectString();
            return string.Concat(inspectString, "\n", "LinkMe".Translate(), ": ",
                powerGiver.thingIDNumber.ToString());
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref powerGiver, "powerGiver");
            Scribe_Values.Look(ref UseePower, "UseePower");
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            CCompPowerPlant = GetComp<CompPowerTrader>();
        }

        public override void Tick()
        {
            base.Tick();
            if (powerGiver != null && powerGiver.CCompPowerTrader.PowerOn)
            {
                UseePower = powerGiver.UsePower;
            }

            if (powerGiver == null || !powerGiver.CCompPowerTrader.PowerOn)
            {
                UseePower = 0f;
            }

            CCompPowerPlant.PowerOutput = UseePower;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }

            if (powerGiver != null)
            {
                yield return new Command_Action
                {
                    defaultLabel = "CutLinkLabel".Translate(),
                    defaultDesc = "CutLinkDesc".Translate(),
                    icon = LingSSWAUI.BreakLink,
                    alsoClickIfOtherInGroupClicked = false,
                    action = delegate
                    {
                        powerGiver.powerNeeder = null;
                        powerGiver.UseInt = -1;
                        powerGiver = null;
                    }
                };
            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (powerGiver != null)
            {
                powerGiver.powerNeeder = null;
                powerGiver.UseInt = -1;
                powerGiver = null;
            }

            base.Destroy(mode);
        }
    }
}