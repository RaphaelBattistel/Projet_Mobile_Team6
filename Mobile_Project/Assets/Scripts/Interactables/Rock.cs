using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Rock : MonoBehaviour
{
    private Rigidbody2D rb2d;
    
    [SerializeField] private float speedLimit;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private UnityEvent onDestroyBox;

    [SerializeField] private GameObject effectForDestroy;

    void Start()
    {
        TryGetComponent(out rb2d);
    }

    void Update()
    {
        if(rb2d.linearVelocityX >= speedLimit || rb2d.linearVelocityX <= -speedLimit)
        {
            DestroyBox();
        }
    }

    private void DestroyBox()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector2.up, 0, boxLayer);

        if (hit.collider != null && !hit.collider.isTrigger)
        {
            onDestroyBox?.Invoke();
            hit.collider.isTrigger = true;
            Instantiate(effectForDestroy).transform.position = hit.collider.transform.position;
            StartCoroutine(DestroyBox(hit.collider.gameObject));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }

    //Désolé tu avait raison
    IEnumerator DestroyBox(GameObject box)
    {
        yield return new WaitForSeconds(.25f);
        Destroy(box);
    }
}
