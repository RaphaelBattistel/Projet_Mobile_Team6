using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask draggableLayer; 
    
    [Header("Debug")]
    private Camera _mainCamera;
    private GameObject _selectedObject;
    private bool _isDragging;
    private Vector3 _offset; 

    void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null) _mainCamera = FindFirstObjectByType<Camera>();
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        bool isPressing = Input.GetMouseButton(0) || (Input.touchCount > 0);
        bool isDown = Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
        bool isUp = Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
        
        Vector3 inputPos = Vector3.zero;

        if (Input.touchCount > 0)
        {
            inputPos = Input.GetTouch(0).position;
        }
        else
        {
            inputPos = Input.mousePosition;
        }

        Vector3 worldPos = GetWorldPosition(inputPos);

        if (isDown)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 100f, draggableLayer);
            
            if (hit.collider != null)
            {
                _selectedObject = hit.collider.gameObject;
                _isDragging = true;
                
                SetObjectPhysics(_selectedObject, false);
            }
        }

        if (_isDragging && _selectedObject != null)
        {
            _selectedObject.transform.position = new Vector3(worldPos.x, worldPos.y, 0);
        }

        if (isUp && _isDragging && _selectedObject != null)
        {
            DropObject();
            _isDragging = false;
            _selectedObject = null;
        }
    }

    private void DropObject()
    {
        Vector3 finalPos = FindCellCenter(_selectedObject.transform.position);
        _selectedObject.transform.position = finalPos;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(finalPos, 0.2f, draggableLayer);
        
        bool fusionHappened = false;

        foreach (var hit in hitColliders)
        {
            if (hit.gameObject != _selectedObject)
            {
                if (FusionManager.Instance.TryToFuse(_selectedObject, hit.gameObject))
                {
                    fusionHappened = true;
                    break; 
                }
            }
        }

        if (!fusionHappened)
        {
            SetObjectPhysics(_selectedObject, true);
        }
    }


    private Vector3 GetWorldPosition(Vector3 screenPos)
    {
        screenPos.z = Mathf.Abs(_mainCamera.transform.position.z);
        return _mainCamera.ScreenToWorldPoint(screenPos);
    }

    private void SetObjectPhysics(GameObject obj, bool isActive)
    {
        if (obj.TryGetComponent(out Rigidbody2D rb))
        {
            rb.simulated = isActive;
        }
        
        if (obj.TryGetComponent(out Collider2D col))
        {
            col.isTrigger = !isActive; 
        }
    }

    private Vector3 FindCellCenter(Vector3 targetPos)
    {
        Vector3Int cellPos = grid.WorldToCell(targetPos);
        return grid.GetCellCenterWorld(cellPos);
    }
}