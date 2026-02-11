using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButton : MonoBehaviour
{
    //[SerializeField] private Image _icon;
    //[SerializeField] private TMP_Text _label;

    private ItemData _item;

    public void Init(ItemData item)
    {
        _item = item;
        //_icon.sprite = item.Sprite;
        //_label.text = item.Label;
    }

    public ItemData Item => _item;
}
