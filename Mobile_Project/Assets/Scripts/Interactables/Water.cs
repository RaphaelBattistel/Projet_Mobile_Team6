using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private LayerMask snowLayer;
    [SerializeField] private LayerMask fireLayer;
    [SerializeField] private Vector2 scaleLimit;
    [SerializeField] private Vector2 waterDropCheck;

    [SerializeField] private ItemData SnowItemData;
    [SerializeField] private ItemData WaterItemData;
    [SerializeField] private Sprite _snowSprite;
    [SerializeField] private Sprite _waterSprite;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private BoxCollider2D _waterCollider;
    [SerializeField] private BoxCollider2D _iceCollider;

    [SerializeField] private bool _startAsIce = false;


    [SerializeField] private float animDuration = 1f;

    bool isAnimating = false;

    private void OnValidate()
    {
        if (_startAsIce)
        {
            _sprite.transform.localPosition = new Vector3(0, -scaleLimit.y / 2, 0);
            _sprite.size = new Vector2(scaleLimit.x, scaleLimit.y);
            _waterCollider.size = new Vector2(scaleLimit.x, scaleLimit.y);
            _waterCollider.offset = new Vector2(0, (scaleLimit.y / 2));
            _iceCollider.size = new Vector2(scaleLimit.x, .5f);
            _iceCollider.offset = new Vector2(0, (scaleLimit.y / 2) - .5f);
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

    void Update()
    {
        if (Mathf.Abs(_sprite.size.y - scaleLimit.y) > .01f && IsDropOfWater())
        {
            UpdateScale();
        }
        else if (IsDropOfSnow())
        {
            _iceCollider.isTrigger = false;
            _sprite.sprite = _snowSprite;
            FusionManager.Instance.OnFusionItem.Invoke(SnowItemData);
        }
        else if (IsDropOfFire())
        {
            _iceCollider.isTrigger = true;
            _sprite.sprite = _waterSprite;
            FusionManager.Instance.OnFusionItem.Invoke(WaterItemData);
        }
    }


    //Check si il y a interraction avec goutte d'eau
    //Détruis la goutte d'eau si il y a collision
    private bool IsDropOfWater()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_sprite.transform.position + transform.up * _sprite.size.y, waterDropCheck, 0f, Vector2.up, 0f, waterLayer);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private bool IsDropOfSnow()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_sprite.transform.position + transform.up * _sprite.size.y, waterDropCheck, 0f, Vector2.up, 0f, snowLayer);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private bool IsDropOfFire()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_sprite.transform.position + transform.up * _sprite.size.y, waterDropCheck, 0f, Vector2.up, 0f, fireLayer);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_sprite.transform.position + (transform.up * _sprite.size.y), waterDropCheck);
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
            _iceCollider.offset = new Vector2(0, (-scaleLimit.y / 2) + newScale - .5f);
            _waterCollider.size = new Vector2(scaleLimit.x, newScale);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isAnimating = false;
    }
}
