using TMPro;
using UnityEngine;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField, HideInInspector] private int _levelId;
    [SerializeField] private TextMeshProUGUI _nameText;

    public void SetLevel(int id, string label)
    {
        _levelId = id;
        _nameText.text = label;
    }
    
    public void LoadLevelScene()
    {
        //LevelManager.Instance.LoadLevel(_levelId, "Level");
        LevelManager.Instance.LoadLevel(_levelId, "ScenePrincipale");
    }
}
