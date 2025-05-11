using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager sharedInstance;
    public bool gameStarted = false;
    public bool gamePaused = false;
    public AudioClip pauseAudio;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                PlayPauseMusic();
            }
            else
            {
                StopPauseMusic();
            }
        }
    }

    void PlayPauseMusic()
    {
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().Stop();
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = pauseAudio;
        audioSource.loop = true;
        audioSource.Play();
    }

    void StopPauseMusic()
    {
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().Stop();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(4.0f);

        gameStarted = true;
    }
}
