using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // Ajout�

public class InventoryContainer : MonoBehaviour
{
    [SerializeField] private GameObject _uiItemPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private LayerMask itemLayerUI;
    [SerializeField] private GraphicRaycaster graphicRaycaster; // Ajout�

    private Dictionary<ItemData, InventoryButton> _images = new Dictionary<ItemData, InventoryButton>();

    void Awake()
    {
        Build(LevelManager.Instance.CurrentLevel.AvailableItems);
    }

    public void Build(List<ItemData> items)
    {
        Clear();
        foreach (var item in items)
        {
            if (_images.ContainsKey(item))
            {
                _images[item].IncreaseQuantity();
            }
            else
            {
                GameObject gameObject = Instantiate(_uiItemPrefab, _container);
                InventoryButton button = gameObject.GetComponent<InventoryButton>();

                button.Init(item);

                // Ajoute l'�v�nement de log sur le clic
                //Button uiButton = gameObject.GetComponent<Button>();
                //if (uiButton != null)
                //{
                //    uiButton.onClick.AddListener(button.LogItemLabel);
                //}
                _images[item] = button;
            }
        }
    }

    public void Clear()
    {
        foreach (var button in _images)
        {
            if (button.Value != null)
            {
                Destroy(button.Value.gameObject);
            }
        }
        
        _images.Clear();
    }

    private void Update()
    {
        HandleInputUI();
    }

    private void HandleInputUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 inputPos = Input.mousePosition;

            // Pr�parer les donn�es pour le raycast UI
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = inputPos
            };

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerData, results);

            foreach (var result in results)
            {
                InventoryButton button = result.gameObject.GetComponent<InventoryButton>();
                if (button is not null)
                {
                    button.LogItemLabel();
                    break; // On ne prend que le premier bouton touch�
                }
            }
        }
    }
}
