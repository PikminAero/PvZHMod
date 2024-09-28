using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvZHMod
{
    internal class StatusEffectApplyXWhenCertainAllyIsKilled : StatusEffectApplyXWhenAllyIsKilled
    {

        public CardData ally;
        public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
        {
            
            if (target.enabled && target.alive && DeathSystem.CheckTeamIsAlly(entity, target) && Battle.IsOnBoard(target) && Battle.IsOnBoard(entity) && entity.name == ally.name)
            {
                if (sacrificed)
                {
                    return DeathSystem.KilledByOwnTeam(entity);
                }

                return true;
            }

            return false;
        }
    }


}
