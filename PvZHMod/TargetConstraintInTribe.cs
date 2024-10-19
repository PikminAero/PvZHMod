using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PvZHMod
{
    // Thank you Abigail for teaching me what TargetConstraints are and how to use it
    internal class TargetConstraintInTribe : TargetConstraint
    {
        public List<String> tribe;

        public override bool Check(CardData data)
        {
            return (tribe.Contains(data.name));
        }

        public override bool Check(Entity entity)
        {
            return (tribe.Contains(entity.data.name));
        }
    }
}
