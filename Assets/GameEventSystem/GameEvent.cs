using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent", fileName = "New Event", order = 51)]
public class GameEvent : ScriptableObject
{
    private HashSet<GameEventListener> _listener = new HashSet<GameEventListener>();

    public void Invoke()
    {
        foreach (var listener in _listener)
        {
            listener.OnEventRaise();
        }
    }

    public void Register(GameEventListener listener) => _listener.Add(listener);

    public void Unregister(GameEventListener listener) => _listener.Remove(listener);
}
