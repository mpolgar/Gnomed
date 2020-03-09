using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    private BoxCollider2D cameraBox;
    public float lerpRatio;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        cameraBox = GetComponent<BoxCollider2D> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer() {
        
        GameObject boundary = GameObject.Find("Boundary");
        if (boundary != null) {
            
            BoxCollider2D boundBox = boundary.GetComponent<BoxCollider2D>();
            float desiredX = Mathf.Clamp(player.position.x, boundBox.bounds.min.x + cameraBox.size.x / 2f, boundBox.bounds.max.x - cameraBox.size.x / 2f);
            float desiredY = Mathf.Clamp(player.position.y, boundBox.bounds.min.y + cameraBox.size.y / 2f, boundBox.bounds.max.y - cameraBox.size.y / 2f);
            
            Vector3 desiredPos = new Vector3(desiredX, desiredY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, desiredPos, lerpRatio);
            
        }
        
    }
}
