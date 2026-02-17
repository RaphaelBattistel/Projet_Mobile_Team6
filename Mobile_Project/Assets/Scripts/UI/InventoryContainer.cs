using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // Ajouté

public class InventoryContainer : MonoBehaviour
{
    [SerializeField] private GameObject _uiItemPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private LayerMask itemLayerUI;
    [SerializeField] private GraphicRaycaster graphicRaycaster; // Ajouté

    private Dictionary<ItemData, InventoryButton> _images = new Dictionary<ItemData, InventoryButton>();

    void Awake()
    {
        Build(LevelManager.Instance.CurrentLevel.AvailableItems, LevelManager.Instance.CurrentLevel.Level);
    }

    public void Build(List<ItemData> items, GameObject level)
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

                // Ajoute l'évènement de log sur le clic
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
            Destroy(button.Value.gameObject);
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

            // Préparer les données pour le raycast UI
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = inputPos
            };

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerData, results);

            foreach (var result in results)
            {
                InventoryButton button = result.gameObject.GetComponent<InventoryButton>();
                if (button != null)
                {
                    button.LogItemLabel();
                    Debug.Log("oui");
                    break; // On ne prend que le premier bouton touché
                }
                Debug.Log("pas touche");
            }
        }
    }
}
