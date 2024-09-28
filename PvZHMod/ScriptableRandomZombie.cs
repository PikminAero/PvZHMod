using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace PvZHMod
{
    internal class ScriptableRandomZombie : ScriptableString
    {
        public override string Get(Entity entity)
        {
            return ConjureZombie.GetRandomZombie();
        }
    }
}
