using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

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
            GameObject gameObject = Instantiate(_buttonPrefab, _container);
            InventoryButton button = gameObject.GetComponent<InventoryButton>();

            button.Init(item);

            // Affecte le sprite du bouton à partir de l'item lié
            Image image = gameObject.GetComponent<Image>();
            if (image != null && button.Item != null)
            {
                image.sprite = button.Item.Sprite;
            }

            // Ajoute l'événement de log sur le clic
            Button uiButton = gameObject.GetComponent<Button>();
            if (uiButton != null)
            {
                uiButton.onClick.AddListener(button.LogItemLabel);
            }

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
