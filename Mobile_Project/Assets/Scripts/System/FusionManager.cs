using System;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.Events;

public class FusionManager : MonoBehaviour
{
    // Le classique Singleton pour pouvoir l'appeler de n'importe quel autre script sans galérer
    public static FusionManager Instance;
    
    [Header("Configuration")]
    [SerializeField] private FusionDatabase _database; // N'oublie pas de glisser ton livre de recettes ici !
    public FusionDatabase Database { get { return _database; } }

    [SerializeField] private UnityEvent onFuse;

    [SerializeField] private GameObject _fusionEffect;

    public UnityEvent<ItemData> OnFusionItem;

    void Awake()
    {
        // Setup du Singleton : s'il y en a déjà un, on dégage le nouveau
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Tente de fusionner l'objet qu'on tient (heldObject) avec celui qu'on touche (targetObject)
    /// </summary>
    public bool TryToFuse(GameObject heldObject, GameObject targetObject)
    {
        // 1. On regarde les étiquettes pour savoir à qui on a affaire
        var dataA = heldObject.GetComponent<IItemHolder>().GetComponent()?.Data;
        var dataB = targetObject.GetComponent<IItemHolder>().GetComponent()?.Data;

        // Si l'un des mecs n'a pas de carte d'identité, on laisse tomber direct
        if (dataA is null || dataB is null) return false;

        // 2. On demande au livre de recettes s'il y a un résultat
        ItemData resultData = _database.GetResult(dataA, dataB);

        // Si on a trouvé un truc !
        if (resultData is not null)
        {
            Debug.Log($"FUSION ! {dataA.Label} + {dataB.Label} = {resultData.Label}");
            onFuse?.Invoke();
            OnFusionItem?.Invoke(resultData);
            PerformFusion(heldObject, targetObject, resultData); // On lance le spectacle
            return true;
        }

        return false; // Rien ne se passe
    }

    // Le tour de magie : on détruit les vieux et on fait pop le nouveau
    private void PerformFusion(GameObject objA, GameObject objB, ItemData resultData)
    {
        // On garde en mémoire là où l'objet était posé sur le sol
        Vector3 spawnPosition = Physics2D.Raycast(objB.transform.position, -objB.transform.up, 10, GridManager.Instance.Ground).point;

        // Ciao les ingrédients !
        Destroy(objA);
        Destroy(objB);

        // On fait apparaître le résultat tout neuf
        if (resultData.Prefab is not null)
        {
            GameObject newObj = Instantiate(resultData.Prefab, GridManager.Instance.Grid.transform);
            Instantiate(_fusionEffect).transform.position = spawnPosition;
            newObj.transform.position = spawnPosition;

            // On lance l'achievement pour avoir fusionné un objet
            if (GooglePlayManager.Instance.IsLoggedIn)
            {
                PlayGamesPlatform.Instance.ReportProgress("CggI4pyy0DgQAhAM", 100f, (bool success) => { });
            }
        }
        else
        {
            // Petit warning sympa si le GD a oublié de mettre un Prefab dans la recette
            Debug.LogError("Le résultat de la fusion n'a pas de Prefab assigné dans l'ItemData !");
        }
    }
}