using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] VolumeSettings audioPanel;
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    [Header ("Musics")]
    public AudioClip background;
    public AudioClip backgroundInGame;
    public AudioClip winJingle;
    public AudioClip loseJingle;
    public AudioClip uiButton;

    [Header("Sfx")]
    public AudioClip arbaletShoot;

    //Where I want to play sfx = AudioManager audioManager; awake : audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>(); where : audioManager.PlaySfx(audioManager.clipName);
    public void Start()
    {
        audioPanel.LoadVolume();
    //    musicSource.clip = background;
        
    //    musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }
}
