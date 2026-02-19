using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayManager : MonoBehaviour
{
    public static GooglePlayManager Instance;
    public bool IsLoggedIn => _isLoggedIn;
    private bool _isLoggedIn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(HandleLogin);
    }

    private void HandleLogin(SignInStatus status)
    {
        switch (status)
        {
            case SignInStatus.Success:
                _isLoggedIn = true;
                break;
            default:
                break;
        }
    }
}