using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvZHMod
{
    internal class StatusEffectApplyXWhenLeaderIsHit : StatusEffectApplyXWhenAllyIsHit
    {
        public override bool RunPostHitEvent(Hit hit)
        {
            if (hit.target.data.cardType == PvZHModCards.Instance.TryGet<CardType>("Leader") && target.enabled && (includeSelf || hit.target != target) && hit.canRetaliate && hit.target.owner == target.owner && hit.Offensive && hit.BasicHit && Battle.IsOnBoard(target))
            {
                return Battle.IsOnBoard(hit.target);
            }

            return false;
        }
    }
}
