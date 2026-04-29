using System;
using UnityEngine;

public class LossPanel : MonoBehaviour
{
    public static LossPanel Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            return;
        }

        Destroy(gameObject);
    }

    public void Replay()
    {
        GameManager.Instance.ResetLevel();
    }

    public void BackToMenu()
    {
        LevelManager.Instance.UnloadCurrentLevel();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}