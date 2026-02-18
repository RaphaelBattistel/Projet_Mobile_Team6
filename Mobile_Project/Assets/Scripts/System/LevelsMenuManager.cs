using System;
using NaughtyAttributes;
using UnityEngine;

public class LevelsMenuManager : MonoBehaviour
{
    public static LevelsMenuManager Instance;
    
    [SerializeField, BoxGroup("References")] private Transform _buttonOrganiser;
    
    [SerializeField, BoxGroup("Data")] private LevelDatabase _levelDatabase;

    [SerializeField, BoxGroup("Objects")] private LevelSelectionButton _buttonPrefab;

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
        foreach (WorldLevels worldLevels in _levelDatabase.worldLevelsList)
        {
            foreach(LevelData levelData in worldLevels.LevelsFromWorld)
            {
                Instantiate(_buttonPrefab, _buttonOrganiser).SetLevel(_levelDatabase.worldLevelsList.IndexOf(worldLevels), levelData.LevelId, levelData.Level.name);
            }
        }
    }
}