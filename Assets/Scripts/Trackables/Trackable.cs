using UnityEngine;
using UnityEngine.Events;

namespace Trackables
{
    public abstract class Trackable<T> : ScriptableObject
    {
        [SerializeField] protected T _value;
        [HideInInspector] public UnityEvent<T> CallbackOnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                CallbackOnValueChanged.Invoke(_value);
            }
        }
    }
}