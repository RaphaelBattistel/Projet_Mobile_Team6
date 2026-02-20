using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Slider>().SetValueWithoutNotify(SoundManager.Instance.GetVolumeMusic());
    }

    public void ChangeVolume(float volume)
    {
        SoundManager.Instance.SetVolumeEffect(volume);
        SoundManager.Instance.SetVolumeMusic(volume);
    }
}
