using Unity.VisualScripting;
using UnityEngine;

// Ça, c'est la petite sécu : on force Unity à coller un SpriteRenderer sur l'objet. 
// Comme ça, impossible d'avoir un objet invisible par erreur !
[RequireComponent(typeof(SpriteRenderer))]
public class ItemHolder : MonoBehaviour
{
    // C'est ici que tu glisses la "carte d'identité" de l'objet (le ScriptableObject)
    public ItemData Data;

    private SpriteRenderer _renderer;

    void Awake()
    {
        // On chope le composant qui dessine l'image au réveil
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Dès que le jeu se lance, on met à jour le visuel
        UpdateVisual();
    }

    // Petite astuce magique : OnValidate s'active tout seul quand tu modifies un truc dans l'inspecteur Unity.
    // Ça permet de voir l'image changer en direct sans même avoir besoin de lancer le jeu (Play).
    void OnValidate()
    {
        _renderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    // La fonction qui fait le taf pour rafraîchir l'image
    public void UpdateVisual()
    {
        // On check qu'on a bien nos données et de quoi dessiner
        if (Data != null && _renderer != null)
        {
            // Si le Game Designer a bien mis une image dans les données, on l'applique !
            if (Data.Sprite != null)
            {
                _renderer.sprite = Data.Sprite;
            }
        }
    }

    private bool _fuze = false;
    public void HasFuse() => _fuze = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_fuze) return;
        if(collision.collider.TryGetComponent(out ItemHolder itemHolder)){
            if(FusionManager.Instance.TryToFuse(gameObject, collision.gameObject))
            {
                itemHolder.HasFuse();
            }
        }
    }
}