using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private ItemData _item;
    public ItemData Item => _item;

    public void Init(ItemData item)
    {
        _item = item;
        // ... autres initialisations
    }

    public void LogItemLabel()
    {
        if (_item != null)
        {
            Debug.Log(_item.Label);
        }
    }
}
