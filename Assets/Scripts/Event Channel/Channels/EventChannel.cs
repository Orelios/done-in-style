using System.Collections.Generic;
using UnityEngine;

public abstract class EventChannel<T> : ScriptableObject
{
    private readonly HashSet<EventListener<T>> listeners = new();

    public void Invoke(T value)
    {
        foreach (var listener in listeners)
        {
            listener.RaiseEvent(value);
        }
    }
    
    public void Register(EventListener<T> listener) => listeners.Add(listener);
    public void Deregister(EventListener<T> listener) => listeners.Remove(listener);
}
