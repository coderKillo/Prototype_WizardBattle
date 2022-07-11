using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent _unityEvent;
    [SerializeField] private GameEvent _gameEvent;

    private void Awake() => _gameEvent.Register(this);

    private void OnDestroy() => _gameEvent.Unregister(this);

    public void OnEventRaise() => _unityEvent.Invoke();
}
