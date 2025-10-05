using System.Collections.Generic;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    private Dictionary <Events, UnityEvent<object>> _eventDictionary = new Dictionary<Events, UnityEvent<object>>();
    
    public void Initialize()
    {
        _eventDictionary ??= new Dictionary<Events, UnityEvent<object>>();
    }

    public static void StartListening (Events eventName, UnityAction<object> listener)
    {
        if (Instance._eventDictionary.TryGetValue (eventName, out UnityEvent<object> thisEvent))
        {
            thisEvent.AddListener (listener);
        } 
        else
        {
            thisEvent = new UnityEvent<object>();
            thisEvent.AddListener (listener);
            Instance._eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (Events eventName, UnityAction<object> listener)
    {
        if (Instance._eventDictionary.TryGetValue (eventName, out UnityEvent<object> thisEvent))
        {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void TriggerEvent (Events eventName, object data = null)
    {
        if (Instance._eventDictionary.TryGetValue (eventName, out UnityEvent<object> thisEvent))
        {
            thisEvent.Invoke (data);
        }
    }
}