using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace PvZHMod
{
    public class ScriptableAllyNbr : ScriptableAmount
    {
        public override int Get(Entity entity)
        {
            if (!entity)
            {
                return 0;
            }
            return (entity.GetAllAllies().Count() + entity.effectBonus) * (entity._data.startWithEffects[0].count) * Mathf.CeilToInt(entity.effectFactor);
        }
    }
}
