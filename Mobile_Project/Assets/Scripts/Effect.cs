using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] Animation _animation;


    private void Start()
    {
        StartCoroutine(CountDown(_animation.clip.averageDuration));
    }

    IEnumerator CountDown(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
