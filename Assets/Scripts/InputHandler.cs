using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    public float jumpHeight = 4.0f;

    bool onGround;

    Subscription<InputEvent> _inputListener;

    Rigidbody rb;

    private void Awake()
    {
        _inputListener = EventBus.Subscribe<InputEvent>(_handleInputEvent);

        rb = this.GetComponent<Rigidbody>();
        onGround = true;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<InputEvent>(_inputListener);
    }

    void _handleInputEvent(InputEvent e)
    {
        if(e.action == Actions.Move)
        {
            Vector3 curVelocity = rb.velocity;
            curVelocity.x = e.axis.x * moveSpeed;
            rb.velocity = curVelocity;
        }
        if(e.action == Actions.Jump && onGround)
        {
            Vector3 curVelocity = rb.velocity;
            curVelocity.y = jumpHeight;
            rb.velocity = curVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
}
