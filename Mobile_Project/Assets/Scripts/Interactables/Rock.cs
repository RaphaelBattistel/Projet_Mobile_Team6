using UnityEngine;
using UnityEngine.EventSystems;

public class Rock : MonoBehaviour
{
    private Rigidbody2D rb2d;
    
    [SerializeField] private float speedLimit;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask boxLayer;

    void Start()
    {
        TryGetComponent(out rb2d);
    }

    void Update()
    {
        if(rb2d.linearVelocityX >= speedLimit || rb2d.linearVelocityX <= -speedLimit)
        {
            DestroyBox();
            Debug.Log("vitesse");
        }
    }

    private void DestroyBox()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector2.up, 0, boxLayer);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            hit.collider.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
