using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidTerrainLaserable : MonoBehaviour
{
    
    public float disappearTime;
    private BoxCollider2D coll;
    private SpriteRenderer rend;
    private bool solid = true;
    
    private void Awake() {
        coll = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        if (solid) {
            StartCoroutine(Disappear());
        }
    }
    
    private IEnumerator Disappear() {
        solid = false;
        coll.enabled = false;
        rend.enabled = false;
        yield return new WaitForSeconds(disappearTime);
        solid = true;
        coll.enabled = true;
        rend.enabled = true;
    }
    
}
