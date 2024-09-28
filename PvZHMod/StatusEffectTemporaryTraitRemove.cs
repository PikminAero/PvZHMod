using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

namespace PvZHMod
{
    internal class StatusEffectTemporaryTraitRemove : StatusEffectTemporaryTrait
    {
        public bool flagEffectEnabled;

        public override void Init()
        {
            OnCardPlayed += Check;
            OnTurnEnd += ToggleFlag;
        }
        public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
        {
            if (flagEffectEnabled)
            {
                flagEffectEnabled = false;
                return entity == target;
            }
            else
            {
                return false;
            }
        }

        /*
        public override bool RunTurnEndEvent(Entity entity)
        {
          return (entity.cardType.name == "Leader");
        }
        */
        private IEnumerator Check(Entity entity, Entity[] targets)
        {
            flagEffectEnabled = false;
            yield return EndRoutine();
        }

        private IEnumerator ToggleFlag(Entity entity)
        {
            if (!flagEffectEnabled)
            {
                flagEffectEnabled = true;
            }
            yield return StackRoutine(count);
        }
    }
}
