using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public Transform[] waypoints;
    int currentWaypoint = 0;
    public float speed = 2f;

    private void FixedUpdate()
    {

        // Distance between the phantom and destination
        float distanceToWaypoint = Vector2.Distance((Vector2)this.transform.position, (Vector2)waypoints[currentWaypoint].position);
        if (distanceToWaypoint < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            Vector2 newDirection = waypoints[currentWaypoint].position - this.transform.position;
            GetComponent<Animator>().SetFloat("DirX", newDirection.x);
            GetComponent<Animator>().SetFloat("DirY", newDirection.y);
        }
        else
        {
            Vector2 newPosition = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(newPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
