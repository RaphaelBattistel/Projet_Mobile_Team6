using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private float groundCastDistance;
    [SerializeField] private float test = 1;
    
    void Start()
    {
        
    }


    void Update()
    {
        if (IsInGround())
        {
            Debug.Log("TUEZ-MOI RAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH");
        }
    }

    private bool IsInGround()
    {
        if (Physics2D.BoxCast(transform.position, groundCheck, 0, transform.up, groundCastDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.up * groundCastDistance, groundCheck);
    }
}
