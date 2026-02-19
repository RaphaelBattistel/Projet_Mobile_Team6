using System;
using GooglePlayGames;
using UnityEngine;

public class HideOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (!didStart)
        {
            return;
        }
        
        if (GooglePlayManager.Instance.IsLoggedIn)
        {
            PlayGamesPlatform.Instance.ReportProgress("CggI4pyy0DgQAhAR", 100f, (bool success) => { });
        }
    }
}
