using UnityEngine;
using System.Collections.Generic;

// Ça, ça permet de créer le fichier direct depuis le menu "Create" d'Unity (Clic droit). Pratique !
[CreateAssetMenu(fileName = "NewFusionDB", menuName = "Steal Egg Run/Fusion Database")]
public class FusionDatabase : ScriptableObject
{
    // La recette de base : un truc + un autre truc = un nouveau truc
    [System.Serializable]
    public struct FusionRecipe
    {
        public ItemData ElementA;
        public ItemData ElementB;
        public ItemData Resultat;
    }

    // La liste où on va ranger toutes nos recettes
    public List<FusionRecipe> Recipes = new List<FusionRecipe>();

    /// <summary>
    /// Cherche si une recette existe pour deux objets donnés.
    /// L'ordre (A+B ou B+A) n'a pas d'importance, on gère les deux cas.
    /// </summary>
    public ItemData GetResult(ItemData item1, ItemData item2)
    {
        // On fouille dans toute la liste...
        foreach (var recipe in Recipes)
        {
            // ... pour voir si nos deux ingrédients matchent avec une recette.
            // Que ce soit Eau+Terre ou Terre+Eau, on s'en fout, ça marche !
            if ((recipe.ElementA == item1 && recipe.ElementB == item2) ||
                (recipe.ElementA == item2 && recipe.ElementB == item1))
            {
                return recipe.Resultat; // Bingo !
            }
        }
        return null; // Pas de bol, ces deux-là ne fusionnent pas.
    }
}