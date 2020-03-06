using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    private BoxCollider2D cameraBox;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        cameraBox = GetComponent<BoxCollider2D> ();
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer ();
    }

    void FollowPlayer() {
        if (GameObject.Find("Boundary")) {
                if (transform.position.x >= player.position.x + 0.5f) {
                    transform.position = new Vector3 (Mathf.Clamp (player.position.x + 0.5f, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.min.x + cameraBox.size.x / 2, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.max.x - cameraBox.size.x / 2),
                                                  Mathf.Clamp (player.position.y, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.min.y + cameraBox.size.y / 2, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.max.y - cameraBox.size.y / 2),
                                                  transform.position.z);
                }
                else if (transform.position.x <= player.position.x - 0.5f) {
                    transform.position = new Vector3 (Mathf.Clamp (player.position.x - 0.5f, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.min.x + cameraBox.size.x / 2, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.max.x - cameraBox.size.x / 2),
                                                  Mathf.Clamp (player.position.y, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.min.y + cameraBox.size.y / 2, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.max.y - cameraBox.size.y / 2),
                                                  transform.position.z);
                }
                else {
                    transform.position = new Vector3 (transform.position.x,
                                                  Mathf.Clamp (player.position.y, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.min.y + cameraBox.size.y / 2, GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ().bounds.max.y - cameraBox.size.y / 2),
                                                  transform.position.z);
                }
        }
    }
}
