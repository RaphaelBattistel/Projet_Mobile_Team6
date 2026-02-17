using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] AnimationClip _animation;


    private void Start()
    {
        StartCoroutine(CountDown(_animation.averageDuration));
    }

    IEnumerator CountDown(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
