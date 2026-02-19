using UnityEngine;
using UnityEngine.UI;

public class CodexImage : MonoBehaviour
{

    [SerializeField] ItemData itemFusion;
    [SerializeField] Image fusionImage;
    [SerializeField] Image Item1Image;
    [SerializeField] Image Item2Image;

    private Sprite _original;

    private void Start()
    {
        FusionManager.Instance.OnFusionItem.AddListener(ChangeCodexImage);
        _original = fusionImage.sprite;
    }

    public void ChangeCodexImage(ItemData item)
    {
        if (item == itemFusion && fusionImage.sprite == _original)
        {
            fusionImage.sprite = item.Sprite;
            FusionDatabase.FusionRecipe recipe = FusionManager.Instance.Database.GetRecipe(item);
            Item1Image.sprite = recipe.ElementA.Sprite;
            Item2Image.sprite = recipe.ElementB.Sprite;
        }
    }

    private void OnDestroy()
    {
        FusionManager.Instance.OnFusionItem.RemoveListener(ChangeCodexImage);
    }

}
