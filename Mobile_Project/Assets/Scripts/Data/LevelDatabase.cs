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

    public LevelData GetLevel(int idWorld, int idLevel)
    {
        return worldLevelsList[idWorld].LevelsFromWorld.Find(l => l.LevelId == idLevel);
        //return Levels.Find(l => l.LevelId == idLevel);
    }

    public LevelData GetNextLevel(int idLevel)
    {
        foreach (var world in worldLevelsList)
        {
            LevelData level =  world.LevelsFromWorld.Find(l => l.LevelId == idLevel + 1);
            if(level != null) return level;
        }
        return null;
        //return Levels.Find(l => l.LevelId == idLevel);
    }
}
