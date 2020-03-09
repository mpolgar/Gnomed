using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTerrainLaserable : MonoBehaviour
{
    
    public Vector2 moveOffset;
    public GameObject shadowPrefab;
    public float moveSpeed;
    public float retractSpeed;
    public float pauseTime;
    
    private Vector2 startPos;
    private Vector2 endPos;
    private bool moving = false;
    
    private void Awake() {
        
        GameObject shadow = Instantiate(shadowPrefab, transform.position + (Vector3)moveOffset / 2, Quaternion.identity);
        if (moveOffset.x == 0f) {
            shadow.transform.localScale = new Vector3(shadow.transform.localScale.x, moveOffset.magnitude, shadow.transform.localScale.z);
        } else {
            shadow.transform.localScale = new Vector3(moveOffset.magnitude, shadow.transform.localScale.y, shadow.transform.localScale.z);
        }
        
        startPos = transform.position;
        endPos = (Vector2)transform.position + moveOffset;
        
    }
    
    private void OnTriggerEnter2D() {
        if (!moving) {
            StartCoroutine(DoMovement());
        }
    }
    
    private IEnumerator DoMovement() {
        
        moving = true;
        
        while ((Vector2)transform.position != endPos) {
            transform.position = Vector2.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(pauseTime);
        
        while ((Vector2)transform.position != startPos) {
            transform.position = Vector2.MoveTowards(transform.position, startPos, retractSpeed * Time.deltaTime);
            yield return null;
        }
        
        moving = false;
        
    }
    
}
