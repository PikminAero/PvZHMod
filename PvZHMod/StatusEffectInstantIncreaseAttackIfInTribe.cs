using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvZHMod
{
    internal class StatusEffectInstantIncreaseAttackIfInTribe : StatusEffectInstantIncreaseAttack
    {
        public List<String> tribe;

        public override IEnumerator Process()
        {
            if (tribe.Contains(target.data.name))
            {
                target.damage.current += (scriptableAmount ? scriptableAmount.Get(target) : GetAmount());
                target.PromptUpdate();
            }
            else
            {
                target.damage.current += 0;
                target.PromptUpdate();
            }
            yield return base.Process();
        }
    }
}
