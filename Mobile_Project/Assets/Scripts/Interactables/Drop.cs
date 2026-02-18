using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] GameObject _effectForSplash;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(_effectForSplash).transform.position = transform.position;
        Destroy(gameObject);
    }
}
