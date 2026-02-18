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
        
        _players = FindObjectsByType<CharacterController>(FindObjectsSortMode.None);
        
        _inventoryContainer = FindFirstObjectByType<InventoryContainer>();
        
        _transitionAnimator = _transitionScreen.GetComponent<Animator>();
        _transitionScreen.gameObject.SetActive(false);
    }

    public void StartLevelAttempt()
    {
        _controlPanelAnimator.SetTrigger(_moveDown);
        foreach (var player in _players)
        {
            player.StartMoving = true;
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
        _players = FindObjectsByType<CharacterController>(FindObjectsSortMode.None);
        _inventoryContainer.Build(LevelManager.Instance.CurrentLevel.AvailableItems);
        _controlPanelAnimator.SetTrigger(_reset);
    }

    public void HandlePlayerLoss()
    {
        Instantiate(_lossPanel);
    }
}