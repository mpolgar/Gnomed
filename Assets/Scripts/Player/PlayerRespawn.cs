using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    //initial starting position of player
    Vector2 respawnPos = new Vector2(0.0f,0.0f);

    //changes the respawn position based on checkpoint, can be called by the checkpoint trigger objects
    public void updateCheckpoint(Vector2 position){
        respawnPos = position;
    }

    //moves the player to the respawn position, can be called by the killable script
    public void respawn(){
        this.transform.position = respawnPos;
    }

}
