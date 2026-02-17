using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private ItemData _item;
    public ItemData Item => _item;

    private int _quantity = 0;
    public void IncreaseQuantity()
    {
        _quantity++;
        GetComponentInChildren<TMP_Text>().text = _quantity.ToString();
    }

    public void Init(ItemData item)
    {
        _item = item;
        IncreaseQuantity();

        // Affecte le sprite du bouton à partir de l'item lié
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = Item.Sprite;
        }
        // ... autres initialisations
    }

    public void LogItemLabel()
    {
        if (_item != null)
        {
            Camera cam = Camera.main;
            if (cam == null)
                return;

            // Position écran -> monde (z géré par la caméra)
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;

            GameObject spawned = Instantiate(_item.Prefab, worldPos, Quaternion.identity);
            Debug.Log(_item.Label);

            // Demander au GridManager de commencer le grab immédiatement, si présent
            if (GridManager.Instance != null)
            {
                GridManager.Instance.StartGrabAtScreenPosition(spawned, Input.mousePosition);
                GridManager.Instance.Spawn.AddListener(ResultSpawn);
            }
        }
    }

    private void ResultSpawn(bool result)
    {
        if (result)
        {
            _quantity--;
            GetComponentInChildren<TMP_Text>().text = _quantity.ToString();
            if (_quantity == 0) Destroy(gameObject);
        }
        GridManager.Instance.Spawn.RemoveListener(ResultSpawn);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = Camera.main;
            if (cam == null)
                return; // Caméra non trouvée, on quitte

            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null && collider.OverlapPoint(mousePos))
            {
                LogItemLabel();
            }
        }
    }
}
