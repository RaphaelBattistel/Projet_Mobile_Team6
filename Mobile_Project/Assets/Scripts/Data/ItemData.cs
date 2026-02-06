using UnityEngine;

public enum Weight { Volatile, Light, Heavy };

[CreateAssetMenu(menuName = "Scriptable Object/Item Data", order = 2)]
public class ItemData : ScriptableObject
{
    public string Label;
    public Weight Weight;
    public Sprite Sprite;
    public UnityEngine.GameObject Prefab;
}
