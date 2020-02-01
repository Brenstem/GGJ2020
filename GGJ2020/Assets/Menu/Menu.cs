using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour {
    public virtual void Awake() {
        if (transform.parent != null) {
            try {
                transform.parent.gameObject.GetComponent<MenuCategory>().AddCategory(this);
            }
            catch {
                Debug.LogWarning("Could not add category to the parents MenuCategory component, is the component missing?", this);
            }
        }
    }

    public abstract bool Trigger();
}
