using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private Vector2 dropCheck;
    [SerializeField] private float groundCastDistance;

    [SerializeField] private float animDuration = 1f;
    [SerializeField] private float compteur = 0;

    float startValue;
    float endValue;
    bool isAnimating = false;

    void Start()
    {
        startValue = transform.localScale.y;
        endValue = dropCheck.y;

    }

    private void UpdateHealthDisplay(float scaleValue, float maxScaleValue)
    {
        if (isAnimating) return;

        startValue = transform.localScale.y;
        endValue = maxScaleValue;
        StartCoroutine(ScaleWater());
    }

    void Update()
    {
        if (IsInGround())
        {
            UpdateHealthDisplay(transform.localScale.y, dropCheck.y);
        }
    }

    private bool IsInGround()
    {
        if (Physics2D.BoxCast(transform.position, dropCheck, 0, transform.up, groundCastDistance, waterLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.up * groundCastDistance, dropCheck);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInGround())
        {
            Debug.Log("Yipi");
            Destroy(collision.gameObject);
        }
        Debug.Log("WompWomp");
    }
}
