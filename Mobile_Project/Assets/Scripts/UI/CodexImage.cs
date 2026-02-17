using UnityEngine;
using UnityEngine.UI;

public class CodexImage : MonoBehaviour
{

    [SerializeField] ItemData itemFusion;
    [SerializeField] Image fusionImage;
    [SerializeField] Image Item1Image;
    [SerializeField] Image Item2Image;

    private void Start()
    {
        FusionManager.Instance.OnFusionItem.AddListener(ChangeCodexImage);
    }

    public void ChangeCodexImage(ItemData item)
    {
        if (item == itemFusion && fusionImage.sprite == null)
        {
            fusionImage.sprite = item.Sprite;
            FusionDatabase.FusionRecipe recipe = FusionManager.Instance.Database.GetRecipe(item);
            Item1Image.sprite = recipe.ElementA.Sprite;
            Item2Image.sprite = recipe.ElementB.Sprite;
        }
    }

}
