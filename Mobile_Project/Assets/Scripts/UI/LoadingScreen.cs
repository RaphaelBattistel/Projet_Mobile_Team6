using UnityEngine;

public class LoadingScreen : MonoBehaviour
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
}