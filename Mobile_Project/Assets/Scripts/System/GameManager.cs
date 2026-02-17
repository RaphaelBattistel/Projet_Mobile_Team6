using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        
        GameObject level = LevelManager.Instance.CurrentLevel.Level;
        Instantiate(level);
    }
}