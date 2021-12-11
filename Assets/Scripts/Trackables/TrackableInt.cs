using UnityEngine;

namespace Trackables
{
    [CreateAssetMenu(fileName = "IntTrackableValue",menuName = "CustomScriptables/TrackableValue/Int")]
    public class TrackableInt : Trackable<int>
    {
        public void AddToValue(int addingValue)
        {
            Value = _value + addingValue;
        }
    }
}