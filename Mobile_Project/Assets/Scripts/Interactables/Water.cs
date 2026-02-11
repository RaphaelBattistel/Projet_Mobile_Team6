using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private Vector2 scaleLimit;
    [SerializeField] private Vector2 waterDropCheck;

    [SerializeField] private float animDuration = 1f;

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



    private bool IsDropOfWater()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, waterDropCheck, 0f, Vector2.up, 0f, waterLayer);

        if (hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, scaleLimit);
        Gizmos.DrawWireCube(transform.position + transform.up, waterDropCheck);
    }




    private void UpdateScale(float scaleValue, float maxScaleValue)
    {

        if (isAnimating) return;

        startValue = transform.localScale.y;
        endValue = maxScaleValue;
        StartCoroutine(ScaleWater());
    }

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
