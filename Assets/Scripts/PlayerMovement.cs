using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    // The player has a horizontal desired velocity that they have inputted, and they LERP
    // to it every fixed update
    public float maxSpeed;
    public float maxFallSpeed;
    public float fastFallSpeed;
    public float speedLerpRatio;
    private float desiredSpeed = 0f;
    private float currentSpeed = 0f;
    
    // Jumping can be variable-length, but has a maximum number of frames
    public int maxJumpFrames;
    public float jumpSpeed;
    private bool jumping = false;
    private bool fastFalling = false;
    
    // When the player is grounded, they can jump, and their velocity matches that
	// of whatever they are grounded to
	public float groundRaycastDistance;
	public Transform leftGroundRaycast;
	public Transform centerGroundRaycast;
	public Transform rightGroundRaycast;
	private bool grounded = false;
	private Transform groundedTransform;
	private Vector2 prevGroundedPos;
    
	private Rigidbody2D body;
	private SpriteRenderer rend;
	private Animator anim;
    private Subscription<InputEvent> inputListener;
    private void Awake() {
		body = GetComponent<Rigidbody2D>();
		rend = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
    }
    
    private void Start() {
        inputListener = EventBus.Subscribe<InputEvent>(HandleInputEvent);
    }
    
    private void FixedUpdate() {
        
        // Determine if the player is grounded, and what object it is grounded to
        if (grounded || body.velocity.y < 0.05f) {
            
            RaycastHit2D leftHit = Physics2D.Raycast(leftGroundRaycast.position, Vector2.down, groundRaycastDistance, LayerMask.GetMask("SolidTerrain"));
			RaycastHit2D centerHit = Physics2D.Raycast(centerGroundRaycast.position, Vector2.down, groundRaycastDistance, LayerMask.GetMask("SolidTerrain"));
			RaycastHit2D rightHit = Physics2D.Raycast(rightGroundRaycast.position, Vector2.down, groundRaycastDistance, LayerMask.GetMask("SolidTerrain"));
            
            if (leftHit || centerHit || rightHit) {
                
				Transform prevGroundedTransform = groundedTransform;
				grounded = true;
                
				if (leftHit && rightHit) {
					if (leftHit.collider.gameObject == rightHit.collider.gameObject) {
						groundedTransform = leftHit.collider.gameObject.transform;
					} else if (centerHit) {
						groundedTransform = centerHit.collider.gameObject.transform;
					} else {
						groundedTransform = leftHit.collider.gameObject.transform;
					}
				} else if (centerHit) {
					groundedTransform = centerHit.collider.gameObject.transform;
				} else {
					groundedTransform = (leftHit) ? leftHit.collider.gameObject.transform : rightHit.collider.gameObject.transform;
				}
                
				if (groundedTransform != prevGroundedTransform) {
                    prevGroundedPos = groundedTransform.position;
                }
                
			} else {
                
				groundedTransform = null;
				grounded = false;
                
			}
            
        }
        
        // When jumping, add the jump component to our movement
		Vector2 jumpComponent = Vector2.zero;
		if (jumping) {
			jumpComponent = jumpSpeed * Vector2.up;
		}
        
        // Move the current speed towards the desired speed, matching the speed of the
		// attached object if applicable
		if (body.velocity.x == 0f) {
			currentSpeed = 0f;
		}
		if (grounded) {
			currentSpeed = Mathf.Lerp(currentSpeed, desiredSpeed, speedLerpRatio);
			Vector2 attachedVelocity = Vector2.zero;
			if (groundedTransform != null && jumpComponent == Vector2.zero) {
				attachedVelocity = ((Vector2)groundedTransform.position - prevGroundedPos) / Time.deltaTime;
				prevGroundedPos = groundedTransform.position;
			}
			body.velocity = jumpComponent + attachedVelocity + new Vector2(currentSpeed, 0f);
		} else {
			currentSpeed = Mathf.Lerp(currentSpeed, desiredSpeed, speedLerpRatio);
            float ySpeed = Mathf.Max(body.velocity.y, fastFalling ? -fastFallSpeed : -maxFallSpeed);
			body.velocity = jumpComponent + new Vector2(currentSpeed, ySpeed);
		}
        
    }
    
    private void HandleInputEvent(InputEvent e) {
        
        if (e.action == Actions.MoveHorizontal) {
            desiredSpeed = e.axis * maxSpeed;
        } else if (e.action == Actions.JumpPressed && grounded) {
            jumping = true;
            body.gravityScale = 0f;
            StartCoroutine(JumpTimeout());
        } else if (e.action == Actions.JumpReleased) {
            jumping = false;
            body.gravityScale = 1f;
        } else if (e.action == Actions.MoveVertical) {
            fastFalling = (e.axis == -1f);
        }
        
    }
    
    private IEnumerator JumpTimeout() {
        for (int i = 0; i < maxJumpFrames; ++i) {
            if (!jumping) {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        jumping = false;
        body.gravityScale = 1f;
    }
    
}
