using System.Collections;
using UnityEngine;

public class BurnEffect : MonoBehaviour, IFire
{
    [SerializeField] private Material _fireMat;
    [SerializeField] private SpriteRenderer[] _renderers;
    [SerializeField] private float _time;
    [SerializeField] private AudioClip _sound;

    private Material _material;

    private void Start()
    {
        _material = new Material(_fireMat);
        foreach (SpriteRenderer spriteRenderer in _renderers)
        {
            spriteRenderer.material = _material;
        }
    }

    public void DoFireInteraction()
    {
        StartCoroutine(Burn());
    }
   
    private IEnumerator Burn()
    {
        float burnAmount = _time;
        SoundManager.Instance.PlayEffect(_sound);

        while (burnAmount > 0f)
        {
            burnAmount -= Time.deltaTime;
            _material.SetFloat("_Power", burnAmount / _time);
            _material.SetFloat("_EffectStrength", 1 - burnAmount / _time);
            yield return null;
        }
        Destroy(gameObject);
    }

}
