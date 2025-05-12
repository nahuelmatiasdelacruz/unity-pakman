using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDead = false;
    public AudioSource playerAudioSource;
    public float globalSpeed = 0.1f;
    private Vector2 destination = Vector2.zero;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
    }

    public void StopAudio()
    {
        playerAudioSource.Stop();
    }

    public void PlayAudio()
    {
        playerAudioSource.Play();
    }

    private void Start()
    {
        destination = this.transform.position;
    }

    public void PlayDeathAnimation()
    {
        StopAudio();
        GameManager.sharedInstance.PlayDeathMusic();
        isDead = true;
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isDead", true);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        if (GameManager.sharedInstance.gamePaused || !GameManager.sharedInstance.gameStarted) 
        {
            GetComponent<AudioSource>().volume = 0;
        };
        GetComponent<AudioSource>().volume = 0.5f;
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
        Vector2 start = this.transform.position;
        Vector2 end = start + direction;
        RaycastHit2D hit = Physics2D.Linecast(start, end, LayerMask.GetMask("MazeWall"));
        return hit.collider == null;
    }
}
