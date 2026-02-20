using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private CharacterController[] _players;
    private GameObject _level;
    private InventoryContainer _inventoryContainer;
    [SerializeField] private Canvas _sceneUI;
    [SerializeField] private Image _transitionScreen;
    private Animator _transitionAnimator;
    [SerializeField] private Animator _controlPanelAnimator;

    private readonly int _transitionStart = Animator.StringToHash("TransitionStart");
    private bool _canShowLevel;

    private readonly int _moveDown = Animator.StringToHash("MoveDown");
    private readonly int _reset = Animator.StringToHash("Reset");

    [SerializeField] private LossPanel _lossPanel;

    private int _currentLevelId;
    private int _resetCountForCurrentLevel;

    private bool _hasPlacedAnItem;

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
        _currentLevelId = LevelManager.Instance.CurrentLevel.LevelId;

        _players = FindObjectsByType<CharacterController>(FindObjectsSortMode.None);

        _inventoryContainer = FindFirstObjectByType<InventoryContainer>();

        _transitionAnimator = _transitionScreen.GetComponent<Animator>();
        _transitionScreen.gameObject.SetActive(false);
        
        MainSceneCanvas.Instance.RebindCamera();
    }

    public void StartLevelAttempt()
    {
        _controlPanelAnimator.SetTrigger(_moveDown);
        foreach (var player in _players)
        {
            player.StartMoving = true;
        }

        if (!_hasPlacedAnItem)
        {
            if (GooglePlayManager.Instance.IsLoggedIn)
            {
                PlayGamesPlatform.Instance.ReportProgress("CggI4pyy0DgQAhAU", 100f, (bool success) => { });
            }
        }
    }

    public void ResetLevel()
    {
        if (!_transitionAnimator.gameObject.activeInHierarchy)
        {
            _transitionAnimator.gameObject.SetActive(true);
        }

        _transitionAnimator.SetTrigger(_transitionStart);
    }

    public void ReloadLevel()
    {
        Destroy(_level);

        _level = Instantiate(LevelManager.Instance.CurrentLevel.Level);
        CountResetForAchievement();

        _currentLevelId = LevelManager.Instance.CurrentLevel.LevelId;
        _players = FindObjectsByType<CharacterController>(FindObjectsSortMode.None);
        _inventoryContainer.Build(LevelManager.Instance.CurrentLevel.AvailableItems);
        _controlPanelAnimator.SetTrigger(_reset);
        MainSceneCanvas.Instance.RebindCamera();
        
        _hasPlacedAnItem =  false;
    }

    public void HandlePlayerLoss()
    {
        Instantiate(_lossPanel);
    }

    private void CountResetForAchievement()
    {
        if (_currentLevelId == LevelManager.Instance.CurrentLevel.LevelId)
        {
            _resetCountForCurrentLevel++;
            if (_resetCountForCurrentLevel == 10)
            {
                if (GooglePlayManager.Instance.IsLoggedIn)
                {
                    PlayGamesPlatform.Instance.ReportProgress("CggI4pyy0DgQAhAN", 100f, (bool progress) => { });
                }
            }
        }
    }

    public void ValidateItemPlaced()
    {
        _hasPlacedAnItem = true;
    }
}