using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Object/Level Database", order = 0)]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> Levels = new List<LevelData>();

    public LevelData GetLevel(int id)
    {
        return Levels.Find(l => l.LevelId == id);
    }
}
