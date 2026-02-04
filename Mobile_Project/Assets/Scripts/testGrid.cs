using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class testGrid : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask objectMask;

    private GameObject selectedObject;
    private bool isDragging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DragAndDrop();
        }

        if (isDragging)
        {
            if(selectedObject.TryGetComponent(out Collider2D objectCollider))
            {
                objectCollider.isTrigger = true;
            }
            if(selectedObject.TryGetComponent(out Rigidbody2D objectRB2D))
            {
                objectRB2D.simulated = false;
            }
            Vector3 pos = mousePos();
            //selectedObject.transform.position = pos;
            selectedObject.transform.position = FindCellCenter();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject.TryGetComponent(out Collider2D objectCollider))
            {
                objectCollider.isTrigger = false;
            }
            if (selectedObject.TryGetComponent(out Rigidbody2D objectRB2D))
            {
                objectRB2D.simulated = true;
            }
            isDragging = false;
            //selectedObject.transform.position = FindCellCenter();
        }
    }

    private Vector2 FindCellCenter()
    {
        Vector2 mouseCellPlacement = new Vector2();

        switch (mousePos().x)
        {
            case < 0:
                mouseCellPlacement.x = (grid.cellSize.x * (int)(mousePos().x / grid.cellSize.x)) - grid.cellSize.x;
                break;
            default:
                mouseCellPlacement.x = grid.cellSize.x * (int)(mousePos().x / grid.cellSize.x);
                break;
        }
        switch (mousePos().y)
        {
            case < 0:
                mouseCellPlacement.y = (grid.cellSize.y * (int)(mousePos().y / grid.cellSize.y)) - grid.cellSize.y;
                break;
            default:
                mouseCellPlacement.y = grid.cellSize.y * (int)(mousePos().y / grid.cellSize.y);
                break;
        }

        Vector2 mouseCellCenter = new Vector2(mouseCellPlacement.x + (grid.cellSize.x / 2), mouseCellPlacement.y + (grid.cellSize.y / 2));

        //Debug.DrawLine(mouseCellPlacement, new Vector2(mouseCellPlacement.x + grid.cellSize.x, mouseCellPlacement.y), Color.red, 1.0f);
        //Debug.DrawLine(mouseCellPlacement, new Vector2(mouseCellPlacement.x, mouseCellPlacement.y + grid.cellSize.y), Color.red, 1.0f);
        //Debug.DrawLine(mouseCellPlacement, mouseCellCenter, Color.forestGreen, 1.0f);

        return mouseCellCenter;
    }

    private void DragAndDrop()
    {
        Vector2 mousePosition = Input.mousePosition;

        Ray ray = camera.ScreenPointToRay(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000, objectMask);
        if (hit.collider != null)
        {
            Debug.Log("Hit!");
            selectedObject = hit.collider.gameObject;
            isDragging = true;
        }
        else
        {
            Debug.Log("HELL NAH");
        }
    }

    Vector2 mousePos()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(camera.transform.position.z);
        Vector2 mouseWorldPosition = camera.ScreenToWorldPoint(mouseScreenPosition);

        return mouseWorldPosition;
    }

}
