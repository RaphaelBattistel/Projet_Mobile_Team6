using Unity.VisualScripting;
using UnityEngine;


public enum PointState { Run, Climb, Fall, End};

[CreateAssetMenu(menuName = "Scriptable Object/Point Data", order = 0)]

public class PointData : ScriptableObject
{
    public PointState PointState;
    [SerializeField] IconAttribute icons;
}
