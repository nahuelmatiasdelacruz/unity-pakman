using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float globalSpeed = 0.1f;
    private Vector2 destination = Vector2.zero;

    private void Start()
    {
        destination = this.transform.position;
    }
    private void FixedUpdate()
    {
        Vector2 newPosition = Vector2.MoveTowards(this.transform.position, destination, globalSpeed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(newPosition);

        float distanceToDestination = Vector2.Distance((Vector2)this.transform.position, destination);
        if(distanceToDestination < 0.01f)
        {
            if (Input.GetKey(KeyCode.UpArrow) && CanMoveTo(Vector2.up))
            {
                destination = (Vector2)this.transform.position + Vector2.up;
            }
            if (Input.GetKey(KeyCode.DownArrow) && CanMoveTo(Vector2.down))
            {
                destination = (Vector2)this.transform.position + Vector2.down;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && CanMoveTo(Vector2.left))
            {
                destination = (Vector2)this.transform.position + Vector2.left;
            }
            if (Input.GetKey(KeyCode.RightArrow) && CanMoveTo(Vector2.right))
            {
                destination = (Vector2)this.transform.position + Vector2.right;
            }
        }
        Vector2 dir = destination - (Vector2)this.transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    bool CanMoveTo(Vector2 direction)
    {
        Debug.DrawLine(this.transform.position, direction);
        Vector2 start = this.transform.position;
        Vector2 end = start + direction;
        RaycastHit2D hit = Physics2D.Linecast(start, end, LayerMask.GetMask("MazeWall"));
        return hit.collider == null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Phantom")) return;
        GetComponent<Animator>().SetBool("GameOver", true);
        
    }

}
