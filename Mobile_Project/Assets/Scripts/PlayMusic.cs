using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField] private AudioClip _musicToPlay;

    void Start()
    {
        SoundManager.Instance.PlayMusic(_musicToPlay);
    }
}
