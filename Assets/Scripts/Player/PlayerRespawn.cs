using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public int checkpointNum = 0;
    List<Vector2> respawnPos;
    void Start()
    {
         respawnPos = new List<Vector2>();
    }

    public void updateCheckpoint(){
        checkpointNum++;
    }

    public void respawn(){
        this.transform.position = respawnPos[checkpointNum];
    }

}
