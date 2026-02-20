using GooglePlayGames;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private LevelClearedPanel _panel;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && LossPanel.Instance is null)
        {
            Instantiate(_panel);
        }

        if (LevelManager.Instance.CurrentLevel.LevelId == 0)
        {
            if (GooglePlayManager.Instance.IsLoggedIn)
            {
                PlayGamesPlatform.Instance.ReportProgress("CggI4pyy0DgQAhAP", 100f, (bool success) => { });
            }
        }
        
        if (LevelManager.Instance.CurrentLevel.LevelId == 6)
        {
            if (GooglePlayManager.Instance.IsLoggedIn)
            {
                PlayGamesPlatform.Instance.ReportProgress("CggI4pyy0DgQAhAO", 100f, (bool success) => { });
            }
        }
        
        if (LevelManager.Instance.CurrentLevel.LevelId == 12)
        {
            if (GooglePlayManager.Instance.IsLoggedIn)
            {
                PlayGamesPlatform.Instance.ReportProgress("CggI4pyy0DgQAhAQ", 100f, (bool success) => { });
            }
        }
    }
}