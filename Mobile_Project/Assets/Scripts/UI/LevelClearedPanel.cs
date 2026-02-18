using System;
using UnityEngine;

public class LevelClearedPanel : MonoBehaviour
{
    public static LevelClearedPanel Instance;
    
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

    public void NextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}