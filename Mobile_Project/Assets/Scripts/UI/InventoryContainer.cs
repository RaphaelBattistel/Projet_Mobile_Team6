using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Ajout�

public class InventoryContainer : MonoBehaviour
{
    [SerializeField] private GameObject _uiItemPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private LayerMask itemLayerUI;
    [SerializeField] private Camera camera;
    [SerializeField] private GraphicRaycaster graphicRaycaster; // Ajout�

    private readonly List<GameObject> _images = new ();

    void Awake()
    {
        Build(LevelManager.Instance.CurrentLevel.AvailableItems, LevelManager.Instance.CurrentLevel.Level);
    }

    public void Build(List<ItemData> items, GameObject level)
    {
        Clear();
        foreach (var item in items)
        {
            GameObject gameObject = Instantiate(_uiItemPrefab, _container);
            InventoryButton button = gameObject.GetComponent<InventoryButton>();

            button.Init(item);

            // Affecte le sprite du bouton � partir de l'item li�
            Image image = gameObject.GetComponent<Image>();
            if (image != null && button.Item != null)
            {
                image.sprite = button.Item.Sprite;
            }

            // Ajoute l'�v�nement de log sur le clic
            //Button uiButton = gameObject.GetComponent<Button>();
            //if (uiButton != null)
            //{
            //    uiButton.onClick.AddListener(button.LogItemLabel);
            //}

            _images.Add(gameObject);
        }
    }

    public void Clear()
    {
        foreach (var button in _images)
        {
            Destroy(button.gameObject);
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
                if (button != null)
                {
                    button.LogItemLabel();
                    Debug.Log("oui");
                    break; // On ne prend que le premier bouton touch�
                }
                Debug.Log("pas touche");
            }
        }
    }
}
