using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Object/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public int LevelId;
    public List<ItemData> AvailableItems;
    public GameObject Level;
}
