using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private LevelDatabase _levelDatabase;

    public LevelData CurrentLevel { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int levelId, string sceneName)
    {
        CurrentLevel = _levelDatabase.GetLevel(levelId);
        
        if (CurrentLevel == null)
        {
            Debug.LogError($"Level {levelId} not found");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void UnloadCurrentLevel()
    {
        
    }
}
