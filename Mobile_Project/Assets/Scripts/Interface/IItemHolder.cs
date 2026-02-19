using UnityEngine;

public interface IItemHolder
{
    public ItemHolder GetComponent();
    public Collider2D GetCollider();
}