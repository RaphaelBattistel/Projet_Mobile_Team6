using System.Collections;
using UnityEngine;

public class BurnEffect : MonoBehaviour, IFire
{
    [SerializeField] private Shader _shader;
    [SerializeField] private SpriteRenderer[] _renderers;
    [SerializeField] private float _time;

    private Material _material;

    private void Start()
    {
        _material = new Material(_shader);
        foreach (SpriteRenderer renderer in _renderers)
        {
            renderer.material = _material;
        }
    }

    public void DoFireInteraction()
    {
        StartCoroutine(Burn());
    }
   
    private IEnumerator Burn()
    {
        float burnAmount = _time;

        while (burnAmount > 0f)
        {
            burnAmount -= Time.deltaTime;
            _material.SetFloat("_Power", (burnAmount / _time));
            _material.SetFloat("_EffectStrength", 1 - (burnAmount / _time));
            yield return null;
        }
        Destroy(gameObject);
    }

}
