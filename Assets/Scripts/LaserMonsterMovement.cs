using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class meant to be used for the movement of the laser monster.
 */ 
public class LaserMonsterMovement : MonoBehaviour
{
    public bool startsOnTop = true;
    public Transform playerTransform;
    public Transform cameraTransform;
    public GameObject laser;
    public GameObject effectSprite;
    public float laserFireDuration;
    public float cooldownDuration;
    public float chargeDuration;
    public float fireInterval;
    public float moveSpeed;
    public float lerpRatio;

    private bool alongTop;
    private bool moving;
    private float reverse;

    Subscription<InputEvent> inputListener;

    public float timeSlowDown;
    float axisInput = 0f;

    bool manual = false;

    // initiallizes private values and starts movement based on editor bool
    private void Start()
    {
        inputListener = EventManager.Subscribe<InputEvent>(HandleInputEvents);
        moving = false;
        reverse = 1;
        
        //MovementPath(startsOnTop);
        StartCoroutine("FirePeriodically");

        moving = true;
    }

    void HandleInputEvents(InputEvent e)
    {
        //check to see if this object currently has control
        if(!manual && e.action == Actions.Switch)
        {
            StopCoroutine("FirePeriodically");
            StartCoroutine(MakeManualWhenReady());
        }
        else if (!manual)
        {
            //no new actions should happen at this time
            return;
        }
        else if(manual && e.action == Actions.Switch)
        {
            manual = false;
            StartCoroutine("FirePeriodically");
            return;
        }


        //collect correct axis input per mode
        if(e.action == Actions.MoveHorizontal && startsOnTop)
        {
            axisInput = e.axis;
        }
        else if(e.action == Actions.MoveVertical && !startsOnTop)
        {
            axisInput = e.axis;
        }

        if(e.action == Actions.Interact || e.action == Actions.JumpPressed)
        {
            //player loses control after firing
            moving = false;
            StartCoroutine(FireLaser());
            EventManager.Publish<InputEvent>(new InputEvent(Actions.Switch, 0f));
        }
    }
    
    private IEnumerator MakeManualWhenReady() {
        while (!moving) {
            yield return null;
        }
        Time.timeScale = timeSlowDown;
        manual = true;
    }

    // movement and raycast detection handling
    private void Update()
    {
        if (moving && !manual)
        {
            // if (DetectPlayer())
            // {
            //     moving = false;
            //     StartCoroutine(FireLaser());
            // }

            if (startsOnTop)
            {
                float newX = Mathf.Lerp(transform.position.x, playerTransform.position.x, lerpRatio);
                transform.position = new Vector2(newX, cameraTransform.position.y + 9f);
            }
            else
            {
                float newY = Mathf.Lerp(transform.position.y, playerTransform.position.y, lerpRatio);
                transform.position = new Vector2(cameraTransform.position.x - 12f, newY);
            }
        }
        else if(manual)
        {
            if (startsOnTop)
            {
                float plusDeltaX = transform.position.x + axisInput * moveSpeed * Time.deltaTime / timeSlowDown;
                transform.position = new Vector2(plusDeltaX, cameraTransform.position.y + 9f);
            }
            else
            {
                float plusDeltaY = transform.position.y + axisInput * moveSpeed * Time.deltaTime / timeSlowDown;
                transform.position = new Vector2(cameraTransform.position.x - 12f, plusDeltaY);
            }
        } else {
            if (startsOnTop)
            {
                transform.position = new Vector2(transform.position.x, cameraTransform.position.y + 9f);
            }
            else
            {
                transform.position = new Vector2(cameraTransform.position.x - 12f, transform.position.y);
            }
        }
    }
    
    // stops the following of the camera (ex. if we want to leave the monster behind in a room)
    public void StopCameraFollow()
    {
        transform.parent = null;
    }

    // restarts the camera following, requires the path to be followed.
    // public void StartCameraFollow(bool moveAlongTop)
    // {
    //     transform.parent = mainCam.transform;
    //     MovementPath(moveAlongTop);
    // }
    
    // //Set the movement path of the monster
    // public void MovementPath(bool moveAlongTop)
    // {
    //     alongTop = moveAlongTop;
    //     if (alongTop)
    //     {
    //         transform.position = topStartPos;
    //         laser.transform.localScale = new Vector3(.5f, 50f);
    //         laser.transform.localPosition = new Vector3(0, -25f);
    //     }
    //     else
    //     {
    //         transform.position = sideStartPos;
    //         laser.transform.localScale = new Vector3(50f, .5f);
    //         laser.transform.localPosition = new Vector3(25f, -1f);
    //     }
    // }

    // returns true if the player is aligned with the source of the laser
    private bool DetectPlayer()
    {
        Vector2 direction;
        if (alongTop) direction = Vector2.down;
        else direction = Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1), direction);
        if (hit.collider != null && hit.collider.CompareTag("Player")) return true;
        return false;
    }

    // Stops movement of monster, adds an effect like the monster is charging the laser,
    // and then fires it. After firing there is a cooldown before the monster can move again
    private IEnumerator FireLaser()
    {
        moving = false;
        Vector3 originalScale = effectSprite.transform.localScale;
        Vector3 destinationScale = new Vector3(1.2f, 1.2f, 1f);

        float currentTime = 0.0f;

        effectSprite.SetActive(true);
        do
        {
            effectSprite.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / .6f);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= chargeDuration);

        currentTime = 0;
        do
        {
            effectSprite.transform.localScale = Vector3.Lerp(destinationScale, originalScale, currentTime / .15f);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= .15f);
        effectSprite.SetActive(false);

        laser.SetActive(true);
        yield return new WaitForSeconds(laserFireDuration);
        laser.SetActive(false);
        yield return new WaitForSeconds(cooldownDuration);
        moving = true;
    }
    
    private IEnumerator FirePeriodically() {
        while (true) {
            yield return new WaitForSeconds(fireInterval);
            if (!manual) {
                moving = false;
                StartCoroutine(FireLaser());
            }
        }
    }
    
}
