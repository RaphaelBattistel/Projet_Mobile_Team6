using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    [SerializeField] GameObject _effectForSplash;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(_effectForSplash);
        Destroy(gameObject);
    }
}
