using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener<T> : MonoBehaviour
{
    [SerializeField] private EventChannel<T> eventChannel;
    [SerializeField] private UnityEvent<T> unityEvent;

    protected void OnEnable()
    {
        eventChannel.Register(this);
    }

    private void OnDisable()
    {
        eventChannel.Deregister(this);
    }

    public void RaiseEvent(T value)
    {
        unityEvent?.Invoke(value);
    }
}
