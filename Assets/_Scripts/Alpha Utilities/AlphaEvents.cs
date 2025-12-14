using System;
using System.Collections.Generic;
using UnityEngine;

public class AlphaEvents : MonoBehaviour
{
    private Dictionary<string, Delegate> eventDictionary;
    private static AlphaEvents eventManager;

    public static AlphaEvents Instance
    {
        get
        {
            if (!eventManager)
            {
                GameObject eventManagerGO = new("AlphaEvents");
                DontDestroyOnLoad(eventManagerGO);
                eventManager = eventManagerGO.AddComponent<AlphaEvents>();
                eventManager.Init();
            }

            return eventManager;
        }
    }

    private void Init()
    {
        eventDictionary ??= new Dictionary<string, Delegate>();
    }

    public static void StartListening<T>(string eventName, Action<T> listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            Instance.eventDictionary[eventName] = Delegate.Combine(thisEvent, listener);
        }
        else
        {
            Instance.eventDictionary.Add(eventName, listener);
        }
    }

    public static void StartListening(string eventName, Action listener)
    {
        StartListening(eventName, (Delegate)listener);
    }

    private static void StartListening(string eventName, Delegate listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            Instance.eventDictionary[eventName] = Delegate.Combine(thisEvent, listener);
        }
        else
        {
            Instance.eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening<T>(string eventName, Action<T> listener)
    {
        if (eventManager == null) return;

        if (Instance.eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            Instance.eventDictionary[eventName] = Delegate.Remove(thisEvent, listener);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        StopListening(eventName, (Delegate)listener);
    }

    private static void StopListening(string eventName, Delegate listener)
    {
        if (eventManager == null) return;

        if (Instance.eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            Instance.eventDictionary[eventName] = Delegate.Remove(thisEvent, listener);
        }
    }

    public static void TriggerEvent<T>(string eventName, T param)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            if (thisEvent is Action<T> callback)
            {
                callback.Invoke(param);
            }
            else
            {
                Debug.LogWarning($"Event {eventName} is not of type {typeof(T)}");
            }
        }
        else
        {
            Debug.LogWarning($"No event found for {eventName}");
        }
    }

    public static void TriggerEvent(string eventName)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            if (thisEvent is Action callback)
            {
                callback.Invoke();
            }
            else
            {
                Debug.LogWarning($"Event {eventName} is not a parameterless action");
            }
        }
        else
        {
            Debug.LogWarning($"No event found for {eventName}");
        }
    }

    public const string InitServicesEvent = "InitServices";
    public const string UpdateCurrency = "UpdateCurrency";
}
