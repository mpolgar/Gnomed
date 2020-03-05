using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class meant to be used for the movement of the laser monster.
 */ 
public class LaserMonsterMovement : MonoBehaviour
{
    public float MaxXPosition;
    public float MaxYPosition;
    public bool startsOnTop = true;
    public float moveSpeed;
    public GameObject laser;
    public GameObject effectSprite;
    public int debugVal;

    private Camera mainCam;
    private bool alongTop;
    private bool moving;
    private float reverse;
    private Vector2 topStartPos;
    private Vector2 sideStartPos;

    // initiallizes private values and starts movement based on editor bool
    private void Start()
    {
        moving = false;
        reverse = 1;
        mainCam = Camera.main;
        transform.parent = mainCam.transform;

        topStartPos = new Vector2(0, MaxYPosition);
        sideStartPos = new Vector2(-MaxXPosition, 0);
        MovementPath(startsOnTop);

        moving = true;
    }

    // movement and raycast detection handling
    private void Update()
    {
        if (moving)
        {
            if (DetectPlayer())
            {
                moving = false;
                StartCoroutine(FireLaser());
            }

            if (alongTop)
            {
                transform.position = new Vector2(transform.position.x + reverse * moveSpeed * Time.deltaTime, transform.position.y);

                //reverse movement if we hit the bounds
                if (Mathf.Abs(transform.position.x) > MaxXPosition)
                {
                    transform.position = new Vector2(reverse * MaxXPosition, transform.position.y);
                    reverse = -reverse;
                }
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + reverse * moveSpeed * Time.deltaTime);
                if (Mathf.Abs(transform.position.y) > MaxYPosition)
                {
                    transform.position = new Vector2(transform.position.x, reverse * MaxYPosition);
                    reverse = -reverse;
                }
            }
        }
    }
    
    // stops the following of the camera (ex. if we want to leave the monster behind in a room)
    public void StopCameraFollow()
    {
        transform.parent = null;
    }

    // restarts the camera following, requires the path to be followed.
    public void StartCameraFollow(bool moveAlongTop)
    {
        transform.parent = mainCam.transform;
        MovementPath(moveAlongTop);
    }
    
    //Set the movement path of the monster
    public void MovementPath(bool moveAlongTop)
    {
        alongTop = moveAlongTop;
        if (alongTop)
        {
            transform.position = topStartPos;
            laser.transform.localScale = new Vector3(.5f, 50f);
            laser.transform.localPosition = new Vector3(0, -25f);
        }
        else
        {
            transform.position = sideStartPos;
            laser.transform.localScale = new Vector3(50f, .5f);
            laser.transform.localPosition = new Vector3(25f, -1f);
        }
    }

    // returns true if the player is aligned with the source of the laser
    private bool DetectPlayer()
    {
        Vector2 direction;
        if (alongTop) direction = Vector2.down;
        else direction = Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1), direction);
        if (hit.collider != null) return true;
        return false;
    }

    // Stops movement of monster, adds an effect like the monster is charging the laser,
    // and then fires it. After firing there is a cooldown before the monster can move again
    private IEnumerator FireLaser()
    {
        moving = false;
        yield return new WaitForSeconds(.5f);
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = new Vector3(1.2f, 1.2f, 1f);

        float currentTime = 0.0f;

        effectSprite.SetActive(true);
        do
        {
            effectSprite.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / .6f);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= .6f);

        currentTime = 0;
        do
        {
            effectSprite.transform.localScale = Vector3.Lerp(destinationScale, originalScale, currentTime / .15f);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= .15f);
        effectSprite.SetActive(false);

        laser.SetActive(true);
        yield return new WaitForSeconds(1f);
        laser.SetActive(false);
        yield return new WaitForSeconds(1f);
        moving = true;
    }
}
