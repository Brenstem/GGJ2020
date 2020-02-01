using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuCategory : Menu {
    //[HideInInspector]
    public List<Menu> menus;
    private int currentIndex;
    public int CurrentIndex {
        set {
            if (value > menus.Count - 1)
                currentIndex = 0;
            else if (value < 0)
                currentIndex = menus.Count - 1;
            else
                currentIndex = value;

        }
        get {
            return currentIndex;
        }
    }

    public MenuCategory(params Menu[] menu) {
        menus = new List<Menu>();
        foreach (Menu item in menu) {
            menus.Add(item);
        }
    }

    public override void Awake() {
        base.Awake();
    }

    private void Start() {
        Sort();
    }

    public void Sort() {
        menus = menus.OrderBy(o => o.transform.GetSiblingIndex()).ToList();
        // Source: https://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object
    }

    public override bool Trigger() {
        if (menus[currentIndex].GetType() == typeof(MenuCategory)) {
            return false;
        }
        else {
            menus[currentIndex].Trigger();
            return true;
        }
    }

    public void AddCategory(Menu menu) {
        menus.Add(menu);
    }

    public void RemoveCategory(Menu menu) {
        menus.Remove(menu);
    }
}
