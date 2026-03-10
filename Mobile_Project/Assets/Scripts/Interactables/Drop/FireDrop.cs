using UnityEngine;

public class FireDrop : Drop
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IFire fire))
        {
            fire.DoFireInteraction();
        }
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IFire fire))
        {
            fire.DoFireInteraction();
        }
        base.OnCollisionEnter2D(collision);
    }
}
