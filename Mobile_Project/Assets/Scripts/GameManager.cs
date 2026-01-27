using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score of player")]
    [SerializeField] private int playerScore = 0;
    [SerializeField] private TextMeshProUGUI scoreDisplay;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOver;



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
    }

    void Start()
    {
        gameOver.SetActive(false);
    }


    public void UpdateScore(int score)
    {
        scoreDisplay.text = "" + score + "$";
    }

    
    private void Win()
    {
        Debug.Log("WIN");
    }

    private void Lose()
    {
        Debug.Log("LOSE");
    }

    public void Replay()
    {
        Debug.Log("REPLAY");
    }


    private void CheckGameOver()
    {
        
    }
}


