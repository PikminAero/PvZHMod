using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvZHMod
{
    internal class ScriptableOvershootStacks : ScriptableAmount
    {
        public override int Get(Entity entity)
        {
            if (!entity)
            {
                return 0;
            }
            var overshootStacks = 0;
            foreach(var trait in entity.traits)
            {
                if (trait.data == PvZHModCards.Instance.TryGet<TraitData>("Overshoot"))
                {
                    overshootStacks++;
                }
            }
            return overshootStacks;
        }
    }
}
