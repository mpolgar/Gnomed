using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            PlayerRespawn pr = other.gameObject.GetComponent<PlayerRespawn>();
            pr.respawn();
        }
    }
}
