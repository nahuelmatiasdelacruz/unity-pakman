using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager sharedInstance;
    public bool gameStarted = false;
    public bool gamePaused = false;
    public AudioClip pauseAudio;
    public AudioClip deathAudio;
    public float invincibleTime = 0.0f;

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
        if(invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
        else
        {
            invincibleTime = 0.0f;
            ChangePhantomsVulnerability(false);
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

    public void PlayDeathMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = deathAudio;
        audioSource.loop = false;
        audioSource.Play();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(4.0f);

        gameStarted = true;
    }

    public void MakeInvincible(float seconds)
    {
        this.invincibleTime += seconds;
        ChangePhantomsVulnerability(true);
    }

    public void ChangePhantomsVulnerability(bool state)
    {
        GameObject[] phantoms = GameObject.FindGameObjectsWithTag("Phantom");
        foreach(GameObject phantom in phantoms)
        {
            Animator animator = phantom.GetComponent<Animator>();
            animator.SetBool("Vulnerable", state);
        }
    }

    public void StopPhantomsMusic()
    {
        GameObject[] phantoms = GameObject.FindGameObjectsWithTag("Phantom");
        foreach(GameObject phantom in phantoms)
        {
            AudioSource audioSource = phantom.GetComponent<AudioSource>();
            audioSource.Stop();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMapScene");
    }
}
