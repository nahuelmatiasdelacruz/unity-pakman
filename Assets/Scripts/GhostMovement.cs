using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public Transform[] waypoints;
    int currentWaypoint = 0;
    public float speed = 2f;
    public AudioClip vulnerableAudio;
    public AudioClip normalAudio;
    private AudioSource audioSource;
    public bool shouldWaitHome = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(GameManager.sharedInstance.invincibleTime > 0)
        {
            if(audioSource.clip != vulnerableAudio)
            {
                audioSource.clip = vulnerableAudio;
                audioSource.Play();
            }
        }
        else
        {
            if(audioSource.clip != normalAudio)
            {
                audioSource.clip = normalAudio;
                audioSource.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.gamePaused || !GameManager.sharedInstance.gameStarted)
        {
            GetComponent<AudioSource>().volume = 0;
            return;
        }
        GetComponent<AudioSource>().volume = 0.4f;
        if (shouldWaitHome) return;
        // Distance between the phantom and destination
        float distanceToWaypoint = Vector2.Distance((Vector2)this.transform.position, (Vector2)waypoints[currentWaypoint].position);
        if (distanceToWaypoint < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            Vector2 newDirection = waypoints[currentWaypoint].position - this.transform.position;
            if(GameManager.sharedInstance.invincibleTime == 0)
            {
                GetComponent<Animator>().SetFloat("DirX", newDirection.x);
                GetComponent<Animator>().SetFloat("DirY", newDirection.y);
            }
        }
        else
        {
            Vector2 newPosition = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(newPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(GameManager.sharedInstance.invincibleTime <= 0)
            {
                GameManager.sharedInstance.gameStarted = false;
                PlayerController playerController = collision.GetComponent<PlayerController>();
                playerController.PlayDeathAnimation();
                StartCoroutine(RestartGame());
            }
            else
            {
                UIManager.sharedInstance.IncreaseScore(1500);
                GameObject home = GameObject.Find("GhostsHome");
                this.transform.position = home.transform.position;
                this.currentWaypoint = 0;
                this.shouldWaitHome = true;
                StartCoroutine(WaitOnHome());
            }
        }
    }
    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        GameManager.sharedInstance.RestartGame();
    }

    IEnumerator WaitOnHome()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        this.speed = speed *= 1.1f;
        this.shouldWaitHome = false;
    }
}
