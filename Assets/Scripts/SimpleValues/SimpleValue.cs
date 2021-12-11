using UnityEngine;

namespace SimpleValues
{
    public abstract class SimpleValue<T> : ScriptableObject
    {
        public T Value;

        public virtual void SetValue(T newValue)
        {
            Value = newValue;
        }
    }
}
