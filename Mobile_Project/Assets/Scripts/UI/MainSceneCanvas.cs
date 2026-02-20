using System;
using UnityEngine;

public class MainSceneCanvas : MonoBehaviour
{
    public static MainSceneCanvas Instance;

    private Canvas _canvas;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _canvas = GetComponent<Canvas>();
            return;
        }

        Destroy(gameObject);
    }

    public void RebindCamera()
    {
        _canvas.worldCamera = Camera.main;
    }
}