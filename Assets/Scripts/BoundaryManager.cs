using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{   
    private BoxCollider2D managerBox;
    private Transform player_transform;
    public GameObject boundary;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        managerBox = GetComponent<BoxCollider2D> ();
        player_transform = player.GetComponent<Transform> ();
        //source = audio.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageBoundary ();
    }

    void ManageBoundary ()
    {
        if (managerBox.bounds.min.x < player_transform.position.x && player_transform.position.x < managerBox.bounds.max.x &&
            managerBox.bounds.min.y < player_transform.position.y && player_transform.position.y < managerBox.bounds.max.y) {
                boundary.SetActive (true);
            }
            else {
                boundary.SetActive (false);
            }
    }
}
