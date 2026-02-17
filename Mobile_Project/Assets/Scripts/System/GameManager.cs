using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public CharacterController Player => _player;
    private CharacterController _player;
    
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
        
        _player = level.GetComponentInChildren<CharacterController>();
    }

    public void StartLevelAttempt()
    {
        _player.StartMoving = true;
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