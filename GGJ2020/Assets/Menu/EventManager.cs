using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    /// <summary>
    /// Dictionary containing all the non-args events
    /// </summary>
    private Dictionary<string, UnityEvent> eventDictionary;

    // Static instances to enable EventManager to be a semi-singelton script
    private static EventManager _instance;
    private static EventManager instance {
        get {
            if (!_instance) {
                _instance = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (_instance) { // Will return null if there are more than one EventManager classes
                    _instance.Initialize();
                }
                else {
                    Debug.LogWarning("There can only be one EventManager script on a GameObject active at a time");
                }
            }
            return _instance;
        }
    }

    private void Initialize() {
        if (eventDictionary == null) {
            eventDictionary = new Dictionary<string, UnityEvent>();
            EventManager.StartListening("Quit", Quit);
        }
    }
    private void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Enable a method to be triggered using the EventManager
    /// </summary>
    /// <param name="eventName">Event name that is being used to trigger an args-less event</param>
    /// <param name="listener">Method that is being called</param>
    public static void StartListening(string eventName, UnityAction listener) {
        if (instance != null) {
            UnityEvent unityEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out unityEvent)) {
                unityEvent.AddListener(listener);
            }
            else {
                unityEvent = new UnityEvent();
                unityEvent.AddListener(listener);
                instance.eventDictionary.Add(eventName, unityEvent);
            }
        }
        else {
            Debug.LogWarning("EventManager has not yet been initialized!");
        }
    }

    /// <summary>
    /// Disable a method to be triggered using the EventManager
    /// </summary>
    /// <param name="eventName">Event name that is being used to trigger an args-less event</param>
    /// <param name="listener">Method that is being called</param>
    public static void StopListening(string eventName, UnityAction listener) {
        if (instance != null) {
            UnityEvent unityEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out unityEvent)) {
                unityEvent.RemoveListener(listener);
            }
        }
        else {
            Debug.LogWarning("EventManager has not yet been initialized!");
        }
    }


    /// <summary>
    /// Trigger an args-less event using it's event name
    /// </summary>
    /// <param name="eventName">The name of the event</param>
    public static void TriggerEvent(string eventName) {
        UnityEvent unityEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out unityEvent)) {
            unityEvent.Invoke();
        }
        else {
            Debug.Log("The event '" + eventName + "' does not exist");
        }
    }
}