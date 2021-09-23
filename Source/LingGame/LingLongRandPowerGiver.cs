using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LingGame
{
    public class LingLongRandPowerGiver : Building
    {
        private readonly List<LingLongRandPowerNeeder> powerNeeders = new List<LingLongRandPowerNeeder>();
        public LingLongRandPowerNeeder powerNeeder;

        public int UseInt = -1;
        public float UsePower;

        public override string LabelCap => base.LabelCap + thingIDNumber;

        public CompPowerTrader CCompPowerTrader => GetComp<CompPowerTrader>();

        public override string GetInspectString()
        {
            var inspectString = base.GetInspectString();
            if (powerNeeder != null)
            {
                return string.Concat(new[]
                {
                    inspectString,
                    "\n",
                    string.Concat("LinkNeederID".Translate(), powerNeeder.thingIDNumber.ToString())
                });
            }

            return base.GetInspectString();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            yield return new Command_Action
            {
                defaultLabel = "LingLongRandPowerGiverPowerUpLabel".Translate(),
                defaultDesc = "LingLongRandPowerGiverPowerUpDesc".Translate(),
                hotKey = KeyBindingDefOf.Misc8,
                icon = LingSSWAUI.PowerUp,
                alsoClickIfOtherInGroupClicked = false,
                action = delegate { UsePower += 100f; }
            };
            yield return new Command_Action
            {
                defaultLabel = "LingLongRandPowerGiverPowerDownLabel".Translate(),
                defaultDesc = "LingLongRandPowerGiverPowerDownDesc".Translate(),
                hotKey = KeyBindingDefOf.Misc9,
                icon = LingSSWAUI.PowerDown,
                alsoClickIfOtherInGroupClicked = false,
                action = delegate
                {
                    UsePower -= 100f;
                    if (UsePower <= 0f)
                    {
                        UsePower = 0f;
                    }
                }
            };
            yield return new Command_Action
            {
                defaultLabel = "FindNeederLabel".Translate(),
                defaultDesc = "FindNeederLabelDesc".Translate(),
                alsoClickIfOtherInGroupClicked = false,
                icon = LingSSWAUI.FindNeeder,
                action = delegate
                {
                    powerNeeders.Clear();
                    foreach (var item in Map.listerBuildings.AllBuildingsColonistOfClass<LingLongRandPowerNeeder>())
                    {
                        powerNeeders.Add(item);
                    }

                    Messages.Message("FindSomeNeeder".Translate(powerNeeders.Count),
                        MessageTypeDefOf.NegativeEvent);
                }
            };
            if (powerNeeders.Count > 0)
            {
                yield return new Command_Action
                {
                    defaultLabel = "UseNextNeederLabel".Translate(),
                    defaultDesc = "UseNextNeederDesc".Translate(),
                    alsoClickIfOtherInGroupClicked = false,
                    icon = LingSSWAUI.SelectNeeder,
                    action = delegate
                    {
                        if (UseInt >= powerNeeders.Count - 1)
                        {
                            UseInt = -1;
                        }

                        if (UseInt >= powerNeeders.Count - 1)
                        {
                            return;
                        }

                        if (powerNeeder != null)
                        {
                            powerNeeder.powerGiver = null;
                        }

                        UseInt++;
                        while (powerNeeders[UseInt].powerGiver != null)
                        {
                            UseInt++;
                        }

                        powerNeeder = powerNeeders[UseInt];
                        powerNeeder.powerGiver = this;
                        FleckMaker.ThrowFireGlow(powerNeeder.DrawPos, powerNeeder.Map, 3f);
                    }
                };
            }

            foreach (var gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref UsePower, "UsePower");
            Scribe_References.Look(ref powerNeeder, "powerNeeder");
        }

        public override void Tick()
        {
            base.Tick();
            CCompPowerTrader.PowerOutput = 0f - UsePower;
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (powerNeeder != null)
            {
                powerNeeder.powerGiver = null;
                powerNeeder = null;
            }

            base.Destroy(mode);
        }
    }
}