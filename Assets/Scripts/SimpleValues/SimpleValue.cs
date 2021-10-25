using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleValue<T> : ScriptableObject
{
    public T Value;

    public virtual void SetValue(T newValue)
    {
        Value = newValue;
    }
}
