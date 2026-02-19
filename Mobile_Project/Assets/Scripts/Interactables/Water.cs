using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour, IWater, ISnow, IFire
{
    [SerializeField] private Vector2 scaleLimit;

    [SerializeField] private ItemData SnowItemData;
    [SerializeField] private ItemData WaterItemData;
    [SerializeField] private Sprite _snowSprite;
    [SerializeField] private AudioClip _soundIce;
    [SerializeField] private Sprite _waterSprite;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private BoxCollider2D _waterCollider;
    [SerializeField] private BoxCollider2D _iceCollider;

    [SerializeField] private bool _startAsIce = false;


    [SerializeField] private float animDuration = 1f;

    bool isAnimating = false;

    [Button("Setup")]
    private void Start()
    {
        if (_startAsIce)
        {
            _sprite.transform.localPosition = new Vector3(0, -scaleLimit.y / 2, 0);
            _sprite.size = new Vector2(scaleLimit.x, scaleLimit.y);
            _waterCollider.size = new Vector2(scaleLimit.x, scaleLimit.y);
            _waterCollider.offset = new Vector2(0, (scaleLimit.y / 2));
            _iceCollider.size = new Vector2(scaleLimit.x, .5f);
            _iceCollider.offset = new Vector2(0, (scaleLimit.y / 2) - .25f);
            _iceCollider.isTrigger = false;
            _sprite.sprite = _snowSprite;
        }
        else
        {
            _iceCollider.isTrigger = true;
            _sprite.sprite = _waterSprite;
            _sprite.transform.localPosition = new Vector3(0, -scaleLimit.y / 2, 0);
            _sprite.size = new Vector2(scaleLimit.x, 0);
            _waterCollider.size = new Vector2(scaleLimit.x, 1);
            _waterCollider.offset = new Vector2(0, 0);
            _iceCollider.size = new Vector2(scaleLimit.x, .5f);
            _iceCollider.offset = new Vector2(0, (-scaleLimit.y / 2) - .5f);
        }
    }
    public void DoWaterInteraction()
    {
        if (Mathf.Abs(_sprite.size.y - scaleLimit.y) > .01f)
        {
            UpdateScale();
        }
    }

    public void DoSnowInteraction()
    {
        if (Mathf.Abs(_sprite.size.y - scaleLimit.y) > .01f) return;
        _iceCollider.isTrigger = false;
        _sprite.sprite = _snowSprite;
        SoundManager.Instance.PlayEffect(_soundIce);
        FusionManager.Instance.OnFusionItem.Invoke(SnowItemData);
    }

    public void DoFireInteraction()
    {
        if (Mathf.Abs(_sprite.size.y - scaleLimit.y) > .01f) return;
        _iceCollider.isTrigger = true;
        _sprite.sprite = _waterSprite;
        FusionManager.Instance.OnFusionItem.Invoke(WaterItemData);
    }

    //Modifie la scale en Y en prenant la scaleLimite en Y
    private void UpdateScale()
    {
        if (isAnimating) return;
        StartCoroutine(ScaleWater());
    }

    //Anime le scale de l'eau avec un Lerp
    private IEnumerator ScaleWater()
    {
        isAnimating = true;

        float elapsed = 0f;

        float newScale;
        while (elapsed < animDuration)
        {
            float ratio = elapsed / animDuration;

            newScale = Mathf.Lerp(0, scaleLimit.y, ratio);
            _sprite.size = new Vector2(scaleLimit.x, newScale);
            _waterCollider.offset = new Vector2(0, newScale / 2);
            _iceCollider.offset = new Vector2(0, (-scaleLimit.y / 2) + newScale - .25f);
            _waterCollider.size = new Vector2(scaleLimit.x, newScale);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isAnimating = false;
    }
}
