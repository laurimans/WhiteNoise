using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent eventToListen;    
    public UnityEvent response;

    private void OnEnable() => eventToListen.RegisterListener(this);
    private void OnDisable() => eventToListen.UnregisterListener(this);

    public void OnEventRaised() => response.Invoke();
}
