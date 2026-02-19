using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationUI : MonoBehaviour
{
    [SerializeField] private AnimationEvents _eventsSender;
    [SerializeField] private Animator _animator;
    private AsyncOperation _asyncOperation;

    private void Start()
    {
        _eventsSender.OnLoadLevelSelection += EndLevelSceneLoading;
    }

    public void LoadLevelScene()
    {
        _asyncOperation = SceneManager.LoadSceneAsync("TitleScreen");
        if (_asyncOperation != null)
        {
            _asyncOperation.allowSceneActivation = false;
        }
        
        _animator.SetTrigger("FadeIn");
    }

    private void EndLevelSceneLoading()
    {
        if (_asyncOperation != null)
        {
            _asyncOperation.allowSceneActivation = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
