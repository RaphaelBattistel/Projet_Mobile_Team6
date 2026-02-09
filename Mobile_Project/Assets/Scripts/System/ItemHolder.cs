using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemHolder : MonoBehaviour
{
    public ItemData Data;

    private SpriteRenderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UpdateVisual();
    }

    void OnValidate()
    {
        _renderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (Data != null && _renderer != null)
        {
            if (Data.Sprite != null)
            {
                _renderer.sprite = Data.Sprite;
            }
        }
    }
}