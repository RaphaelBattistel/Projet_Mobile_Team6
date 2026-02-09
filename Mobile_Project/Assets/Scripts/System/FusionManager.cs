using UnityEngine;

public class FusionManager : MonoBehaviour
{
    public static FusionManager Instance;
    
    [Header("Configuration")]
    [SerializeField] private FusionDatabase _database; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Tente de fusionner l'objet qu'on tient (heldObject) avec celui qu'on touche (targetObject)
    /// </summary>
    public bool TryToFuse(GameObject heldObject, GameObject targetObject)
    {
        var dataA = heldObject.GetComponent<ItemHolder>()?.Data;
        var dataB = targetObject.GetComponent<ItemHolder>()?.Data;

        if (dataA == null || dataB == null) return false;

        ItemData resultData = _database.GetResult(dataA, dataB);

        if (resultData != null)
        {
            Debug.Log($"FUSION ! {dataA.Label} + {dataB.Label} = {resultData.Label}");
            PerformFusion(heldObject, targetObject, resultData);
            return true;
        }

        return false;
    }

    private void PerformFusion(GameObject objA, GameObject objB, ItemData resultData)
    {
        Vector3 spawnPosition = objB.transform.position;

        Destroy(objA);
        Destroy(objB);

        if (resultData.Prefab != null)
        {
           GameObject newObj = Instantiate(resultData.Prefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Le résultat de la fusion n'a pas de Prefab assigné dans l'ItemData !");
        }
    }
}