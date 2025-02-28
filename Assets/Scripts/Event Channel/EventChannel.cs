using System.Collections.Generic;
using UnityEngine;

public abstract class EventChannel<T> : ScriptableObject
{
    private HashSet<EventListener<T>> listeners = new();

    public void Invoke(T value)
    {
        foreach (var listener in listeners)
        {
            listener.Raise(value);
        }
    }

    public void Register(EventListener<T> listener) => listeners.Add(listener);

    public void Deregister(EventListener<T> listener) => listeners.Remove(listener);
}

public readonly struct Empty {}

[CreateAssetMenu(fileName = "New Empty Event Channel", menuName = "Scriptable Objects/Event Channels/Empty")]
public class EventChannel : EventChannel<Empty> { }
