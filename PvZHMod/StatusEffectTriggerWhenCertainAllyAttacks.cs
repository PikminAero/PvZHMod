using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;

namespace PvZHMod
{
    public class StatusEffectTriggerWhenCertainAllyAttacks : StatusEffectTriggerWhenAllyAttacks
    {
        public CardData ally;
        public override bool RunHitEvent(Hit hit)
        {
            if (hit.attacker?.name == ally.name)
            {
                return base.RunHitEvent(hit);
            }
            return false;
        }
    }
}
