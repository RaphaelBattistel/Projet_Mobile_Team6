using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenuManager : MonoBehaviour
{
    public static LevelsMenuManager Instance;
    
    [SerializeField, BoxGroup("References")] private Transform _buttonOrganiser;
    
    [SerializeField, BoxGroup("Data")] private LevelDatabase _levelDatabase;
    [SerializeField, BoxGroup("Data")] private int _worldNumber;
    
    //[SerializeField, BoxGroup("Data")] private WorldLevels _worldLevels;

    [SerializeField, BoxGroup("Objects")] private LevelSelectionButton _buttonPrefab;
    [SerializeField, BoxGroup("Objects")] private Image _image;
    [SerializeField, BoxGroup("Objects")] private Button _buttonNextWorld;
    [SerializeField, BoxGroup("Objects")] private Button _buttonPreviousWorld;
    [SerializeField, BoxGroup("Objects")] private TMP_Text _text;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        
        SpawnButtonsFromDatabase();
    }

    private void SpawnButtonsFromDatabase()
    {
        //foreach (LevelData levelData in _levelDatabase.Levels)
        //{
        //    Instantiate(_buttonPrefab, _buttonOrganiser).SetLevel(levelData.LevelId, levelData.Level.name);
        //}

        //foreach (WorldLevels worldLevels in _levelDatabase.worldLevelsList)
        //{
        //    foreach (LevelData levelData in worldLevels.LevelsFromWorld)
        //    {
        //        Instantiate(_buttonPrefab, _buttonOrganiser).SetLevel(_levelDatabase.worldLevelsList.IndexOf(worldLevels), levelData.LevelId, levelData.Level.name);
        //    }
        //}

        foreach (Transform item in _buttonOrganiser)
        {
            Destroy(item.gameObject);
        }
        foreach (LevelData levelData in _levelDatabase.worldLevelsList[_worldNumber].LevelsFromWorld)
        {
            Instantiate(_buttonPrefab, _buttonOrganiser).SetLevel(_worldNumber, levelData.LevelId, levelData.Level.name);
        }

        _buttonNextWorld.gameObject.SetActive(true);
        _buttonPreviousWorld.gameObject.SetActive(true);

        if (_worldNumber - 1 < 0)
        {
            _buttonPreviousWorld.gameObject.SetActive(false);
        }
        if (_worldNumber + 1 >= _levelDatabase.worldLevelsList.Count)
        {
            _buttonNextWorld.gameObject.SetActive(false);
        }

        _image.sprite = _levelDatabase.worldLevelsList[_worldNumber].WorldSprite;
        _text.text = _levelDatabase.worldLevelsList[_worldNumber].WorldName;
    }

    public void ChangeToNextWorld()
    {
        _worldNumber++;
        
        SpawnButtonsFromDatabase();
    }
    public void ChangeToPreviousWorld()
    {
        _worldNumber--;
        SpawnButtonsFromDatabase();
    }
}


