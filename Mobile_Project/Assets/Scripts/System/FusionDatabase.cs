using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewFusionDB", menuName = "Steal Egg Run/Fusion Database")]
public class FusionDatabase : ScriptableObject
{
    [System.Serializable]
    public struct FusionRecipe
    {
        public ItemData ElementA;
        public ItemData ElementB;
        public ItemData Resultat;
    }

    public List<FusionRecipe> Recipes = new List<FusionRecipe>();

    /// <summary>
    /// Cherche si une recette existe pour deux objets donnés.
    /// L'ordre (A+B ou B+A) n'a pas d'importance.
    /// </summary>
    public ItemData GetResult(ItemData item1, ItemData item2)
    {
        foreach (var recipe in Recipes)
        {
            if ((recipe.ElementA == item1 && recipe.ElementB == item2) ||
                (recipe.ElementA == item2 && recipe.ElementB == item1))
            {
                return recipe.Resultat;
            }
        }
        return null;
    }
}