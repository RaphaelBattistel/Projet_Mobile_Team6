using System;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public static event Action OnLevelEnd;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.HandlePlayerWin();
        }
    }
}