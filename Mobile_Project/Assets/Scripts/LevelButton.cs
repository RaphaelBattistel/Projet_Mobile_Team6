using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public void ChangeLevel()
    {
        LevelManager.Instance.UnloadCurrentLevel();
    }
}
