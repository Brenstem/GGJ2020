using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {
    [SerializeField] private Transform arrow;
    protected List<Menu> history;
    public Menu getCurrentMenu {
        get {
            return history[currentIndex];
        }
    }

    protected MenuCategory getCurrentCategory {
        get {
            if (getCurrentMenu.GetType() == typeof(MenuCategory)) {
                return getCurrentMenu as MenuCategory;
            }
            else {
                return null;
            }
        }
    }
    protected int currentIndex = 0;
    protected int CurrentIndex {
        set {
            if (value > history.Count - 1) {
                Debug.LogWarning("CurrentIndex set value is higher than the max value");
                currentIndex = history.Count - 1;
            }
            else if (value < 0) {
                Debug.LogWarning("CurrentIndex was set to a value lower than 0, correcting this error");
                currentIndex = 0;
            }
            else
                currentIndex = value;
        }
        get {
            if (currentIndex < history.Count)
                return currentIndex;
            else
                return history.Count - 1;
        }
    }

    protected virtual void Awake() {
        history = new List<Menu>();
        history.Add(GetComponent<MenuCategory>());
    }

    protected virtual void Update() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            UpdateCurrentIndex(1);
            PingCurrentCategory();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            UpdateCurrentIndex(-1);
            PingCurrentCategory();
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            TriggerCategory();
            PingCurrentCategory();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            CurrentIndex--;
            PingCurrentCategory();
        }
    }

    protected virtual void PingCurrentCategory() {
        try {
            if (getCurrentCategory != null)
            {
#if UNITY_EDITOR
                UnityEditor.EditorGUIUtility.PingObject(getCurrentCategory.menus[getCurrentCategory.CurrentIndex]);
#endif
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorGUIUtility.PingObject(getCurrentMenu);
#endif
            }
            arrow.position = new Vector3(arrow.position.x, getCurrentCategory.menus[getCurrentCategory.CurrentIndex].transform.position.y, arrow.position.z);
        }
        catch (Exception e) {
            Debug.LogWarning(e, this);
        }
    }

    protected virtual void UpdateCurrentIndex(int incrementalValue) {
        if (getCurrentCategory != null)
            getCurrentCategory.CurrentIndex += incrementalValue;
    }

    protected virtual void TriggerCategory() {
        if (!getCurrentMenu.Trigger()) { // if the menu is not a trigger
            if (history.Count - 1 != currentIndex) { // if the currentIndex is lower than the history maximum
                for (int i = history.Count - 1; i > currentIndex; i++) { // deletes history up until the point of the currentIndex
                    history.RemoveAt(i);
                }
            }
            history.Add(getCurrentCategory.menus[getCurrentCategory.CurrentIndex]);
            currentIndex = history.Count - 1;
            PingCurrentCategory();
        }
    }
}