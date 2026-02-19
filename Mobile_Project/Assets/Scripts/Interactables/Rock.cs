using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Rock : MonoBehaviour
{
    private Rigidbody2D rb2d;
    
    [SerializeField] private float speedLimit;
    [SerializeField] private float speedLossInMudPerFrame;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField, Layer] private int mudLayer;
    [SerializeField] private UnityEvent onDestroyBox;

    [SerializeField] private GameObject effectForDestroy;

    private bool isStuckInMud;

    void Start()
    {
        TryGetComponent(out rb2d);
    }

    void Update()
    {
        if (isStuckInMud)
        {
            if (rb2d.linearVelocity.magnitude >= speedLimit * 3 / 4)
            {
                rb2d.linearVelocity -= rb2d.linearVelocity.normalized / 10 * speedLossInMudPerFrame;
            }
        }
        
        if(rb2d.linearVelocity.magnitude >= speedLimit)
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

    //D�sol� tu avait raison
    IEnumerator DestroyBox(GameObject box)
    {
        yield return new WaitForSeconds(.25f);
        Destroy(box);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == mudLayer)
        {
            isStuckInMud = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == mudLayer)
        {
            isStuckInMud = false;
        }
    }
}