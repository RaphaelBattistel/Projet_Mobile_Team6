using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void AllowLoading()
    {
        LevelManager.Instance.SetCanLoadLevel(true);
    }
    
    public void EndLoading()
    {
        LevelManager.Instance.EndLevelLoading();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    public void ReloadLevel()
    {
        GameManager.Instance.ReloadLevel();
    }
}