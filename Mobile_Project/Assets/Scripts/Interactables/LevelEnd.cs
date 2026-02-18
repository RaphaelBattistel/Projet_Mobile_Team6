using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private LevelClearedPanel _panel;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(_panel);
        }
    }
}