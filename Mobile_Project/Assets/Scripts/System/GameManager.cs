using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private CharacterController _player;
    private GameObject _level;
    private Canvas _sceneUI;
    
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
        
        _level = Instantiate(LevelManager.Instance.CurrentLevel.Level);
        
        _player = _level.GetComponentInChildren<CharacterController>();
    }

    public void StartLevelAttempt()
    {
        _player.StartMoving = true;
    }

    public void ResetLevel()
    {
        Destroy(_level);
        _level = Instantiate(LevelManager.Instance.CurrentLevel.Level);
        _player = _level.GetComponentInChildren<CharacterController>();
    }

    public void HandlePlayerLoss()
    {
        
    }

    public void HandlePlayerWin()
    {
        LevelManager.Instance.UnloadCurrentLevel();
    }
}