using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public enum Actions
{
    MoveHorizontal,
    MoveVertical,
    JumpPressed,
    JumpReleased,
    Interact,
};

public class InputEvent
{
    public Actions action;
    public float axis;

    public InputEvent(Actions _in, float _axis)
    {
        action = _in;
        axis = _axis;
    }

    public override string ToString()
    {
        return "Inputed " + action.ToString();
    }
}

public class PlayerInput : MonoBehaviour
{
    
    public const bool DEBUG_MODE = false;

    static PlayerInput instance;

    private void Awake()
    {
        //Singleton setup
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;

            //DontDestroyOnLoad(this.gameObject);
        }
    }
    
    private void OnMoveHorizontal(InputValue value) {
        float newValRaw = value.Get<float>();
        float newVal = (newValRaw == 0f) ? 0f : Mathf.Sign(newValRaw);
        DebugLog("moving horizontal " + newVal);
        EventBus.Publish(new InputEvent(Actions.MoveHorizontal, newVal));
    }
    
    private void OnMoveVertical(InputValue value) {
        float newValRaw = value.Get<float>();
        float newVal = (newValRaw == 0f) ? 0f : Mathf.Sign(newValRaw);
        DebugLog("moving vertical " + newVal);
        EventBus.Publish(new InputEvent(Actions.MoveVertical, newVal));
    }
    
    private void OnJump(InputValue value) {
        float val = value.Get<float>();
        Actions status = (val == 0f) ? Actions.JumpReleased : Actions.JumpPressed;
        DebugLog((val == 0f) ? "jump released" : "jump pressed");
        EventBus.Publish(new InputEvent(status, 0f));
    }
    
    private void OnInteract() {
        DebugLog("interact");
        EventBus.Publish(new InputEvent(Actions.Interact, 0f));
    }
    
    private void DebugLog(string message) {
        if (DEBUG_MODE) {
            Debug.Log("PlayerInput: " + message);
        }
    }
    
}
