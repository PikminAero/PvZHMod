﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PvZHMod
{
    internal class ScriptableBonusDamageIfRowFull : ScriptableAmount
    {
        public override int Get(Entity entity)
        {
            if (!entity)
            {
                return 0;
            }
            var dmg = 0;
            List<Entity> alliesInRow = entity.GetAlliesInRow();
            Debug.Log("In ScriptableBonusDamageIfRowFull.Get()");
            Debug.Log($"entity.GetAlliesInRow.Count = {entity.GetAlliesInRow().Count}");
            if (alliesInRow.Count >= 2)
            {
                dmg = ((entity._data.startWithEffects[0].count + entity.effectBonus) * Mathf.CeilToInt(entity.effectFactor));
            }
            return dmg;
        }
    }
}
