using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : Menu {
    //public CustomEventArgs eventArgs;
    [SerializeField] private string eventName;

    public string EventName {
        get { return eventName; }
    }

    public MenuTrigger(string eventName) {
        this.eventName = eventName;
    }

    public override void Awake() {
        base.Awake();
    }

    private void Start() {
        
    }

    public override bool Trigger() {
        EventManager.TriggerEvent(eventName);
        return true;
    }
}
