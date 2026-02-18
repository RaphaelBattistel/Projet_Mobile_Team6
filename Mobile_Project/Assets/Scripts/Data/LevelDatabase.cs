using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct WorldLevels
{
    public List<LevelData> LevelsFromWorld;
    public Sprite WorldSprite;
}

[CreateAssetMenu(menuName = "Scriptable Object/Level Database", order = 0)]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> Levels = new List<LevelData>();

    public List<WorldLevels> worldLevelsList;

    public LevelData GetLevel(int id)
    {
        return Levels.Find(l => l.LevelId == id);
    }
}
