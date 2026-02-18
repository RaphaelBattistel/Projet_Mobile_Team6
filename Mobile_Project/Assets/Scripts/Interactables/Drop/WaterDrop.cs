using UnityEngine;

public class WaterDrop : Drop
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IWater water))
        {
            water.DoWaterInteraction();
        }
        base.OnTriggerEnter2D(collision);
    }
}
