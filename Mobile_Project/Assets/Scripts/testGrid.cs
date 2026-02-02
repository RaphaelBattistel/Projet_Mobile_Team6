using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class testGrid : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Camera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = Mathf.Abs(camera.transform.position.z);
            Vector2 mouseWorldPosition = camera.ScreenToWorldPoint(mouseScreenPosition);

            Vector2 mouseCellPlacement = new Vector2();

            switch (mouseWorldPosition.x)
            {
                case < 0:
                    mouseCellPlacement.x = (grid.cellSize.x * (int)(mouseWorldPosition.x / grid.cellSize.x)) - grid.cellSize.x;
                    break;
                default:
                    mouseCellPlacement.x = grid.cellSize.x * (int)(mouseWorldPosition.x/ grid.cellSize.x);
                    break;
            }
            switch (mouseWorldPosition.y)
            {
                case < 0:
                    mouseCellPlacement.y = (grid.cellSize.y * (int)(mouseWorldPosition.y / grid.cellSize.y)) - grid.cellSize.y;
                    break;
                default:
                    mouseCellPlacement.y = grid.cellSize.y * (int)(mouseWorldPosition.y/ grid.cellSize.y);
                    break;
            }

            Vector2 mouseCellCenter = new Vector2(mouseCellPlacement.x + (grid.cellSize.x / 2), mouseCellPlacement.y + (grid.cellSize.y / 2));

            Debug.DrawLine(mouseCellPlacement, new Vector2(mouseCellPlacement.x + grid.cellSize.x, mouseCellPlacement.y), Color.red, 1.0f);
            Debug.DrawLine(mouseCellPlacement, new Vector2(mouseCellPlacement.x, mouseCellPlacement.y + grid.cellSize.y), Color.red, 1.0f);
            Debug.DrawLine(mouseCellPlacement, mouseCellCenter, Color.forestGreen, 1.0f);
            Debug.Log(mouseCellPlacement);
        }
    }
}
