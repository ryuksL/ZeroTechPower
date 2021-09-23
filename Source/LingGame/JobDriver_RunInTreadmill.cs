using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace LingGame
{
    public class JobDriver_RunInTreadmill : JobDriver
    {
        private const int JobEndInterval = 4000;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            var p = pawn;
            var targetA = job.targetA;
            var localJob = job;
            return p.Reserve(targetA, localJob, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            var tick = 300;
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            this.FailOnThingHavingDesignation(TargetIndex.A, DesignationDefOf.Uninstall);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            var work = new Toil();
            work.tickAction = delegate
            {
                var actor = work.actor;
                tick--;
                if (tick <= 0)
                {
                    HealthUtility.AdjustSeverity(actor, ZeroTechPowerDefOf.LingThinkingLoss, 0.01f);
                    tick = 300;
                }

                var level = actor.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
                var num = actor.skills.GetSkill(SkillDefOf.Intellectual).levelInt *
                          actor.GetStatValue(StatDefOf.PsychicSensitivity);
                ((Building_Treadmill)actor.CurJob.targetA.Thing).TryGetComp<CompPowerPlant>().PowerOutput =
                    level * num * 100f;
            };
            work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
            work.WithEffect(ZeroTechPowerDefOf.BrainBlood, TargetIndex.A);
            work.defaultCompleteMode = ToilCompleteMode.Delay;
            work.defaultDuration = 4000;
            yield return work;
        }
    }
}