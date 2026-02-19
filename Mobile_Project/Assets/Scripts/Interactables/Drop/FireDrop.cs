using UnityEngine;

public class FireDrop : Drop
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IFire fire))
        {
            fire.DoFireInteraction();
        }
        base.OnTriggerEnter2D(collision);
    }
}
