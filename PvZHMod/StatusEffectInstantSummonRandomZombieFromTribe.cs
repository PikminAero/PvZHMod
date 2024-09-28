using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvZHMod
{
    public class StatusEffectInstantSummonRandomZombieFromTribe : StatusEffectInstantSummon
    {
        public List<String> zombies;

        // Credits to Abigail for this
        public override System.Collections.IEnumerator Process()
        {
            
            Routine.Clump clump = new();
            int amount = GetAmount();
            for (int i = 0; i < amount; i++)
            {
                if (zombies.Count > 0)
                {
                    targetSummon.gainTrait = null;
                    targetSummon.summonCard = PvZHModCards.Instance.TryGet<CardData>(zombies.RandomItem());
                }
                clump.Add(TrySummon());
                yield return clump.WaitForEnd();
            }
            yield return Remove();
        }
    }
}
