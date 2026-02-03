using UnityEngine;
using System.Collections.Generic;

public class InventoryContainer : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _container;

    private readonly List<GameObject> _buttons = new ();

    void Awake()
    {
        Build(LevelManager.Instance.CurrentLevel.AvailableItems);
    }

    public void Build(List<ItemData> items)
    {
        Clear();

        foreach (var item in items)
        {
            var gameObject = Instantiate(_buttonPrefab, _container);
            var button = gameObject.GetComponent<InventoryButton>();

            button.Init(item);
            _buttons.Add(gameObject);
        }
    }

    public void Clear()
    {
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }

        _buttons.Clear();
    }
}
