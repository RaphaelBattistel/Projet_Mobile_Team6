using GooglePlayGames;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private Grid grid; // La grille Unity pour caler les objets proprement
    [SerializeField] private LayerMask draggableLayer; // Le layer des objets qu'on a le droit de bouger (ex: "Items")
    [SerializeField] private LayerMask ground;
    public LayerMask Ground => ground;
    [Header("Debug")] private Camera _mainCamera;
    private GameObject _selectedObject; // L'objet qu'on a entre les doigts
    private bool _isDragging; // Savoir si on est en train de glisser un truc
    private Vector3 _offset;

    [SerializeField] private GameObject _spawnSuccess;
    [SerializeField] private GameObject _spawnFail;

    public UnityEvent<bool> Spawn;

    // Singleton simple pour permettre à d'autres scripts de demander un grab
    public static GridManager Instance { get; private set; }
    public Grid Grid { get => grid;}

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // On chope la caméra. Le FindFirstObjectByType c'est au cas où MainCamera foire.
        _mainCamera = Camera.main;
        if (_mainCamera == null) _mainCamera = FindFirstObjectByType<Camera>();
    }

    private void Update()
    {
        // On gère les inputs à chaque frame
        HandleInput();
    }

    private void HandleInput()
    {
        // On gère à la fois le tactile (mobile) ET la souris (pour tester sur PC peinard)
        bool isPressing = Input.GetMouseButton(0) || (Input.touchCount > 0);
        bool isDown = Input.GetMouseButtonDown(0) ||
                      (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
        bool isUp = Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);

        Vector3 inputPos = Vector3.zero;

        // On prend la position du doigt ou de la souris selon ce qui est dispo
        if (Input.touchCount > 0)
        {
            inputPos = Input.GetTouch(0).position;
        }
        else
        {
            inputPos = Input.mousePosition;
        }

        // On transforme les pixels de l'écran en vraies coordonnées 2D
        Vector3 worldPos = GetWorldPosition(inputPos);

        //// --- QUAND ON APPUIE --- (utilisé si on clique directement sur un objet existant)
        //if (isDown)
        //{
        //    // On lance un petit laser invisible pour voir si on touche un objet de notre Layer
        //    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 100f, draggableLayer);
//
        //    if (hit.collider != null)
        //    {
        //        // On l'a attrapé !
        //        _selectedObject = hit.collider.gameObject;
        //        _isDragging = true;
//
        //        // On coupe sa physique pour qu'il devienne un "fantôme" le temps du voyage
        //        SetObjectPhysics(_selectedObject, false);
//
        //        // Calcul simple d'offset pour éviter un saut
        //        _offset = _selectedObject.transform.position - new Vector3(worldPos.x, worldPos.y, 0);
        //    }
        //}

        // --- PENDANT QU'ON GLISSE ---
        if (_isDragging && _selectedObject is not null)
        {
            // L'objet suit notre doigt sagement en tenant compte de l'offset
            _selectedObject.transform.position = new Vector3(worldPos.x + _offset.x, worldPos.y + _offset.y, 0);
        }

        // --- QUAND ON LÂCHE L'ÉCRAN ---
        if (isUp && _isDragging && _selectedObject is not null)
        {
            DropObject();
            _isDragging = false;
            _selectedObject = null;
            _offset = Vector3.zero;
        }
    }

    private void DropObject()
    {
        if (_selectedObject is null)
            return;

        // 1. On cale l'objet bien propre au centre de la case la plus proche
        Vector3 finalPos = FindCellCenter(_selectedObject.transform.position);
        _selectedObject.transform.position = finalPos;

        // 1.b Si on a posé sur du "ground", on détruit l'objet et on quitte
        Collider2D[] groundHits = Physics2D.OverlapCircleAll(finalPos, 0.2f, ground);
        if (groundHits != null && groundHits.Length > 0)
        {
            Spawn?.Invoke(false);
            Instantiate(_spawnFail).transform.position = _selectedObject.transform.position;
            Destroy(_selectedObject);
            return;
        }

        // 2. On regarde s'il y a du monde sous notre objet
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(finalPos, 0.2f, draggableLayer);

        bool fusionHappened = false;

        foreach (var hit in hitColliders)
        {
            // Si on touche un mec qui n'est pas nous-mêmes...
            if (hit.gameObject != _selectedObject)
            {
                // ... on tente la fusion !
                if (FusionManager.Instance.TryToFuse(_selectedObject, hit.gameObject))
                {
                    fusionHappened = true;
                    break; // Fusion réussie, on arrête de chercher
                }
            }
        }

        // 3. Si ça n'a pas fusionné, on lui remet sa physique pour qu'il soit solide
        if (!fusionHappened)
        {
            SetObjectPhysics(_selectedObject, true);

            // On lance l'achievement pour avoir placé un objet
            if (GooglePlayManager.Instance.IsLoggedIn)
            {
                PlayGamesPlatform.Instance.ReportProgress("CggI4pyy0DgQAhAL", 100f, (bool success) => { });
            }
        }
        _selectedObject.transform.SetParent(Grid.transform);
        Instantiate(_spawnSuccess).transform.position = _selectedObject.transform.position;
        Spawn?.Invoke(true);
    }

    // Méthode publique : démarre un "grab" depuis un GameObject déjà instancié
    // screenPos : position écran (Input.mousePosition ou touch.position)
    public void StartGrabAtScreenPosition(GameObject obj, Vector2 screenPos)
    {
        if (obj is null) return;

        Vector3 worldPos = GetWorldPosition(new Vector3(screenPos.x, screenPos.y, 0));
        _selectedObject = obj;
        _isDragging = true;

        // Couper la physique pour le "fantôme"
        SetObjectPhysics(_selectedObject, false);

        // Calculer offset pour que l'objet suive précisément le doigt/souris sans sauter
        _offset = _selectedObject.transform.position - new Vector3(worldPos.x, worldPos.y, 0);
    }

    // Petite moulinette pour gérer la caméra 2D
    private Vector3 GetWorldPosition(Vector3 screenPos)
    {
        screenPos.z = Mathf.Abs(_mainCamera.transform.position.z);
        return _mainCamera.ScreenToWorldPoint(screenPos);
    }

    // Fonction pratique pour allumer/éteindre la physique d'un coup
    private void SetObjectPhysics(GameObject obj, bool isActive)
    {
        if (obj.TryGetComponent(out Rigidbody2D rb))
        {
            rb.simulated = isActive;
        }

        // S'il est inactif, on le passe en Trigger (fantôme) pour qu'il passe à travers les trucs
        if (obj.TryGetComponent(out Collider2D col))
        {
            col.isTrigger = !isActive;
        }
    }

    // Un petit bout de code pour trouver le milieu exact d'une case de la grille
    private Vector3 FindCellCenter(Vector3 targetPos)
    {
        Vector3Int cellPos = Grid.WorldToCell(targetPos);
        return Grid.GetCellCenterWorld(cellPos);
    }
}