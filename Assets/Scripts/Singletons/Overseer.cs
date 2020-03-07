using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple component to tell the overseer object that it should not be destroyed on load
// All singleton objects are added as sub-objects of the overseer
public class Overseer : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
