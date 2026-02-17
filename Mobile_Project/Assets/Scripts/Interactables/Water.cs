using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Water : MonoBehaviour
{
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private Vector2 scaleLimit;
    [SerializeField] private Vector2 waterDropCheck;

    [SerializeField] private float animDuration = 1f;
    [SerializeField] private UnityEvent onProgress;

    float startValue;
    float endValue;
    bool isAnimating = false;

    void Start()
    {
        startValue = transform.localScale.y;
        endValue = scaleLimit.y;

    }
    void Update()
    {
        if (IsDropOfWater())
        {
            UpdateScale(transform.localScale.y, scaleLimit.y);
        }
    }


    //Check si il y a interraction avec goutte d'eau
    //Détruis la goutte d'eau si il y a collision
    private bool IsDropOfWater()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, waterDropCheck, 0f, Vector2.up, 0f, waterLayer);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, scaleLimit);
        Gizmos.DrawWireCube(transform.position + transform.up, waterDropCheck);
    }



    //Modifie la scale en Y en prenant la scaleLimite en Y
    private void UpdateScale(float scaleValue, float maxScaleValue)
    {

        if (isAnimating) return;

        startValue = transform.localScale.y;
        endValue = maxScaleValue;
        onProgress?.Invoke();
        StartCoroutine(ScaleWater());
    }

    //Anime le scale de l'eau avec un Lerp
    private IEnumerator ScaleWater()
    {
        isAnimating = true;

        float elapsed = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsed < animDuration)
        {
            float ratio = elapsed / animDuration;

            Vector3 newScale = startScale;
            newScale.y = Mathf.Lerp(startValue, endValue, ratio);
            transform.localScale = newScale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        isAnimating = false;
    }
}
