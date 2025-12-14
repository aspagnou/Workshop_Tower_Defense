using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] VolumeSettings audioPanel;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    

    public AudioClip background;

    //Where I want to play sfx = AudioManager audioManager; awake : audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>(); where : audioManager.PlaySfx(audioManager.clipName);
    public void Start()
    {
        audioPanel.LoadVolume();
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
