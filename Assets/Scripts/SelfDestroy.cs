using System.Collections;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float timeForDestruction;

    public void Start()
    {
        StartCoroutine(DestroySelf(timeForDestruction));
    }

    private IEnumerator DestroySelf(float timeForDestruction)
    {
        yield return new WaitForSeconds(timeForDestruction);
        Destroy(gameObject);
    }
}
