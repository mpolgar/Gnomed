using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public bool usingKeyboard = false;

    static PlayerInput instance;

    Gamepad _gamepad;

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

            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        _gamepad = Gamepad.current;

        if(_gamepad == null)
        {
            usingKeyboard = true;
        }
    }

    void Update()
    {
        if (usingKeyboard)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            EventBus.Publish<InputEvent>(new InputEvent(Actions.Move, x, y));

            if (Input.GetKeyDown(KeyCode.Z))
            {
                EventBus.Publish<InputEvent>(new InputEvent(Actions.Jump, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                EventBus.Publish<InputEvent>(new InputEvent(Actions.Interact, 0, 0));
            }

        }
        else
        {
            float x = _gamepad.leftStick.x.ReadValue();
            float y = _gamepad.rightStick.y.ReadValue();
            EventBus.Publish<InputEvent>(new InputEvent(Actions.Move, x, y));

            if (_gamepad.aButton.wasPressedThisFrame)
            {
                EventBus.Publish<InputEvent>(new InputEvent(Actions.Jump, 0, 0));
            }
            if (_gamepad.bButton.wasPressedThisFrame)
            {
                EventBus.Publish<InputEvent>(new InputEvent(Actions.Interact, 0, 0));
            }
        }

        if (_gamepad == null)
        {
            usingKeyboard = true;
        }
    }
}

public enum Actions
{
    Move,
    Jump,
    Interact,
};

public class InputEvent
{
    public Actions action;
    public Vector2 axis;

    public InputEvent(Actions _in, float x, float y)
    {
        action = _in;
        axis = new Vector2(x, y);
    }

    public string ToString()
    {
        return "Inputed " + action.ToString();
    }
}
