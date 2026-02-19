using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private ItemData _item;
    public ItemData Item => _item;
    private Camera _cam;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;
    private Collider2D _collider;

    private int _quantity;

    public void IncreaseQuantity()
    {
        _quantity++;
        _text.text = _quantity.ToString();
    }

    public void Init(ItemData item)
    {
        _item = item;
        IncreaseQuantity();
        _image.sprite = Item.Sprite;
        _image.rectTransform.sizeDelta = new Vector2(_image.sprite.rect.width, _image.sprite.rect.height);
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void LogItemLabel()
    {
        if (_item is not null)
        {
            // Position �cran -> monde (z g�r� par la cam�ra)
            Vector3 worldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;

            GameObject spawned = Instantiate(_item.Prefab, worldPos, Quaternion.identity);
            _collider = spawned.GetComponent<IItemHolder>().GetCollider();

            // Demander au GridManager de commencer le grab imm�diatement, si pr�sent
            if (GridManager.Instance is not null)
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
            _text.text = _quantity.ToString();
            if (_quantity == 0) Destroy(gameObject);
        }

        GridManager.Instance.Spawn.RemoveListener(ResultSpawn);
    }

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         if (_cam is null)
    //             return; // Cam�ra non trouv�e, on quitte
    //         
    //         Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    //         if (_collider is not null && _collider.OverlapPoint(mousePos))
    //         {
    //             LogItemLabel();
    //         }
    //     }
    // }
}