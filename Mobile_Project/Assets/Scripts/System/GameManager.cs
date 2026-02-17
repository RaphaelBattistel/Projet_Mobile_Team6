using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    //Instancier le script
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        GameObject level = LevelManager.Instance.CurrentLevel.Level;
        Instantiate(level);
    }

    public void StartLevelAttempt()
    {
        
    }

    public void ResetLevel()
    {
        
    }

    private void HandlePlayerLoss()
    {
        
    }

    public void HandlePlayerWin()
    {
        LevelManager.Instance.UnloadCurrentLevel();
    }
}