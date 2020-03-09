using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointUpdate : MonoBehaviour
{
    public bool entered = false; 
    void OnTriggerEnter2D(Collider2D other){
        if((other.tag == "Player") && (!entered)){
            entered = true;
            PlayerRespawn pr = other.gameObject.GetComponent<PlayerRespawn>();
            pr.updateCheckpoint(this.transform.position);           
        }
    }
}
