using UnityEngine;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField] private int _levelId;

    public void LoadLevelScene()
    {
        LevelManager.Instance.LoadLevel(_levelId, "Level");
    }
}
