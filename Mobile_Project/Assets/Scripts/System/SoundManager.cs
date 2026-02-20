using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectSource;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void PlayMusic(AudioClip music)
    {
        _musicSource.clip = music;
        _musicSource.Play();
    }

    public void PlayEffect(AudioClip effect)
    {
        _effectSource.PlayOneShot(effect);
    }

    public void SetVolumeMusic(float value)
    {
        _musicSource.volume = value;
    }

    public void SetVolumeEffect(float value)
    {
        _effectSource.volume = value;
    }
}
