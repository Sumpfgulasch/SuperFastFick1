using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource runAudio;      // Assign the running sound AudioSource in the inspector
    public AudioSource attackAudio;   // Assign the attack sound AudioSource

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRunSound()
    {
        if (runAudio != null && !runAudio.isPlaying)
            runAudio.Play();
    }

    public void StopRunSound()
    {
        if (runAudio != null && runAudio.isPlaying)
            runAudio.Stop();
    }

    public void PlayAttackSound()
    {
        if (attackAudio != null)
            attackAudio.PlayOneShot(attackAudio.clip);
    }
}