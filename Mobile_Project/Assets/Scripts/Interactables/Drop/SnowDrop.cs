using UnityEngine;

public class SnowDrop : Drop
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.TryGetComponent(out ISnow snow))
        {
            snow.DoSnowInteraction();
        }
        base.OnTriggerEnter2D(collision);
    }
}
