using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
// HUGE Credits to Michael C. Without them, Overshoot wouldn't exist.

public class StatusEffectSporadicTrait : StatusEffectTemporaryTrait
{
	private bool active = true;
	public override void Init()
	{
		base.OnCardPlayed += (_,__) => { return Deactivate(); };
        base.OnTurnEnd += (_) => { return Activate(); };
        base.Init();
	}

    public IEnumerator Activate()
    {
        if (!active)
        {
            active = true;
            yield return base.StackRoutine(count);
        }
    }

    public IEnumerator Deactivate()
    {
        if (active)
        {
            active = false;
            yield return base.EndRoutine();
        }
    }

    public override bool RunTurnEndEvent(Entity entity)
    {
        return (entity?.data?.cardType?.name == "Leader");
    }

    public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
    {
        return (entity == target);
    }
}
