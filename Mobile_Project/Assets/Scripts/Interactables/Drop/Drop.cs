using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] GameObject _effectForSplash;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(_effectForSplash).transform.position = transform.position;
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(_effectForSplash).transform.position = transform.position;
        Destroy(gameObject);
    }
}
