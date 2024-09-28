using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvZHMod
{
    internal class StatusEffectIgnoreDamageOnHit : StatusEffectApplyXWhenHit
    {
        public override void Init()
        {
            this.OnHit += PreventDamage;
        }

        public override bool RunHitEvent(Hit hit)
        {
            return hit.target == target;
        }

        // Credits to Abigail
        private IEnumerator PreventDamage(Hit hit)
        {
            hit.damage = 0;
            yield break;
        }
    }
}
