using UnityEngine;

public abstract class ScriptableString : ScriptableObject
{
    public virtual string Get(Entity entity)
    {
        return "";
    }

    public ScriptableString()
    {
    }
}