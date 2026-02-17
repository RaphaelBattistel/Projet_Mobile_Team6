using UnityEngine;

public class Fire : MonoBehaviour
{

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (collision.gameObject.tag == "inflammable")
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
