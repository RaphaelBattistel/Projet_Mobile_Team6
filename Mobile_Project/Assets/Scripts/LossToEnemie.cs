using UnityEngine;

public class LossToEnemie : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.HandlePlayerLoss();
        }
    }
}
