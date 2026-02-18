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