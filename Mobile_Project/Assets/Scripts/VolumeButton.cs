using UnityEngine;

public class VolumeButton : MonoBehaviour
{
    public void ChangeVolume(float volume)
    {
        SoundManager.Instance.SetVolumeEffect(volume);
        SoundManager.Instance.SetVolumeMusic(volume);
    }
}
