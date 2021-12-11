using UnityEngine;
using UnityEngine.Events;

namespace Trackables.TrackableObservers
{
    public class TrackableObserver<T> : MonoBehaviour
    {
        [SerializeField] private Trackable<T> _trackable;
        [SerializeField] private UnityEvent<T> _callback;

        private void OnEnable()
        {
            _trackable.CallbackOnValueChanged.AddListener(_callback.Invoke);
        }

        private void OnDisable()
        {
            _trackable.CallbackOnValueChanged.RemoveListener(_callback.Invoke);
        }
    }
}
