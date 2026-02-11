using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Object/Path Data", order = 0)]

public class PathData: ScriptableObject
{
    public List<GameObject> PointsPath;
}
