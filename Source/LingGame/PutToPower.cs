using RimWorld;
using Verse;

namespace LingGame
{
    public class PutToPower : Building
    {
        private float Expower;

        public CompPowerTrader CCompPowerTrader => GetComp<CompPowerTrader>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref Expower, "Expower");
        }

        public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PreApplyDamage(ref dinfo, out absorbed);
            Expower += dinfo.Amount;
            CCompPowerTrader.PowerOutput = Expower * (HitPoints / (float)MaxHitPoints);
            if (HitPoints <= 1000)
            {
                HitPoints = 1000;
            }
        }

        public override void TickRare()
        {
            base.TickRare();
            if (!(Expower > 0f))
            {
                return;
            }

            Expower *= 0.99f;
            CCompPowerTrader.PowerOutput = Expower * (HitPoints / (float)MaxHitPoints);
        }
    }
}