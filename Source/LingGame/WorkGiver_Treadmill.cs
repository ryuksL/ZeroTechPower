using RimWorld;
using Verse;
using Verse.AI;

namespace LingGame
{
    public class WorkGiver_Treadmill : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(ZeroTechPowerDefOf.LingTreadmill);

        public override bool Prioritized => true;

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return t is Building_Treadmill building_Treadmill &&
                   pawn.CanReserve(building_Treadmill, 1, -1, null, forced);
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(ZeroTechPowerDefOf.RunInTreadmill, t);
        }
    }
}