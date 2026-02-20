using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private LevelDatabase _levelDatabase;

    public LevelData CurrentLevel { get; private set; }

    private AsyncOperation _loadingOperation;

    [SerializeField] private Animator _loadingScreenAnimator;

    private readonly int _loadingStart = Animator.StringToHash("LoadingStart");
    private readonly int _loadingDone = Animator.StringToHash("LoadingDone");
    private readonly int _resetLoadingScreen = Animator.StringToHash("ResetLoadingScreen");

    private bool _canLoadLevel;
    private bool _isLoadingNextLevel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _loadingScreenAnimator.gameObject.SetActive(false);
    }

    public void LoadLevel(int worldId, int levelId)
    {
        CurrentLevel = _levelDatabase.GetLevel(worldId, levelId);

        if (CurrentLevel == null)
        {
            Debug.LogError($"Level {levelId} not found");
            return;
        }

        // Ca devrait charger sauf si un petit malin décide de faire n'importe quoi avec les build settings
        _loadingOperation = SceneManager.LoadSceneAsync(2);
        if (_loadingOperation is null)
        {
            throw new UnassignedReferenceException("Scene 2 not found in build settings");
        }

        _loadingOperation.allowSceneActivation = false;
        StartCoroutine(AnimateLevelLoading());
    }

    private IEnumerator AnimateLevelLoading()
    {
        // On laisse tourner l'anim tant que la scène charge, puis on lui indique de s'achever
        if (!_loadingScreenAnimator.gameObject.activeInHierarchy)
        {
            _loadingScreenAnimator.gameObject.SetActive(true);
        }

        _loadingScreenAnimator.SetTrigger(_loadingStart);

        while (_loadingOperation.progress < .89f || !_canLoadLevel)
        {
            yield return new WaitForEndOfFrame();
        }

        _loadingScreenAnimator.SetTrigger(_loadingDone);
    }

    // Pour laisser l'animation boucler au moins une fois
    public void SetCanLoadLevel(bool canLoad) => _canLoadLevel = canLoad;

    public void EndLevelLoading()
    {
        // On appelle ça dans une fonction grâce à une notify dans l'animation de fin déclenchée plus haut
        if (!_isLoadingNextLevel)
        {
            _loadingOperation.allowSceneActivation = true;
        }
        else
        {
            GameManager.Instance.ReloadLevel();
            _isLoadingNextLevel = false;
        }

        _loadingScreenAnimator.SetTrigger(_resetLoadingScreen);
        _canLoadLevel = false;
    }

    public async void UnloadCurrentLevel()
    {
        try
        {
            // Pareil, normalement la scène devrait se charger sauf dans le cas d'un sabotage
            _loadingOperation = SceneManager.LoadSceneAsync(1);
            if (_loadingOperation is null)
            {
                throw new UnassignedReferenceException("Scene 1 not found in build settings");
            }

            _loadingOperation.allowSceneActivation = false;
            StartCoroutine(AnimateLevelLoading());

            // On attend que le chargement soit achevé pour faire le ménage
            await _loadingOperation;

            CurrentLevel = null;
        }
        catch
        {
            throw new Exception("Either something went wrong or the playmode was ended while loading");
        }
    }
    public async void LoadMainMenu()
    {
        try
        {
            // Pareil, normalement la scène devrait se charger sauf dans le cas d'un sabotage
            _loadingOperation = SceneManager.LoadSceneAsync(0);
            if (_loadingOperation is null)
            {
                throw new UnassignedReferenceException("Scene 0 not found in build settings");
            }

            _loadingOperation.allowSceneActivation = false;
            StartCoroutine(AnimateLevelLoading());

            // On attend que le chargement soit achevé pour faire le ménage
            await _loadingOperation;

            CurrentLevel = null;
        }
        catch
        {
            throw new Exception("Either something went wrong or the playmode was ended while loading");
        }
    }

    public void LoadNextLevel()
    {
        _isLoadingNextLevel = true;
        CurrentLevel = _levelDatabase.GetNextLevel(CurrentLevel.LevelId);
        if(CurrentLevel == null)
        {
            SceneManager.LoadSceneAsync(2);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(AnimateNextLevelLoading());
        }
    }

    private IEnumerator AnimateNextLevelLoading()
    {
        if (!_loadingScreenAnimator.gameObject.activeInHierarchy)
        {
            _loadingScreenAnimator.gameObject.SetActive(true);
        }

        _loadingScreenAnimator.SetTrigger(_loadingStart);

        while (!_canLoadLevel)
        {
            yield return new WaitForEndOfFrame();
        }

        _loadingScreenAnimator.SetTrigger(_loadingDone);
    }
}