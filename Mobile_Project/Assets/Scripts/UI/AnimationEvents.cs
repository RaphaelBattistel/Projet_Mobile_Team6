using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void AllowLoading()
    {
        LevelManager.Instance.SetCanLoadLevel(true);

        if (LevelClearedPanel.Instance != null)
        {
            Destroy(LevelClearedPanel.Instance.gameObject);
        }

        if (LossPanel.Instance != null)
        {
            Destroy(LossPanel.Instance.gameObject);
        }
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

        if (LevelClearedPanel.Instance != null)
        {
            Destroy(LevelClearedPanel.Instance.gameObject);
        }

        if (LossPanel.Instance != null)
        {
            Destroy(LossPanel.Instance.gameObject);
        }
    }

    public event Action OnLoadLevelSelection;
    public void LoadLevelSelectionScene()
    {
        OnLoadLevelSelection?.Invoke();
    }
}