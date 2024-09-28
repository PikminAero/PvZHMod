using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace PvZHMod
{
    public class ScriptableFlamencoEffect : ScriptableAmount
    {
        public override int Get(Entity entity)
        {
            if (!entity)
            {
                return 0;
            }
            var dmg = 0;
            List<Entity> allies = entity.GetAllAllies();
            foreach (Entity item in allies)
            {
                if (ConjureZombie.Dancing.zombies.Contains(item.data.name))
                {
                    dmg += 1;
                }
            }
            return dmg * ((entity._data.startWithEffects[0].count + entity.effectBonus) * Mathf.CeilToInt(entity.effectFactor));
        }
    }
}
