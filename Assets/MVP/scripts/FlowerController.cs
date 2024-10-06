using UnityEngine;
using System.Collections;

public class FlowerController : MonoBehaviour
{
    public float pickDelay = 3;
    public GameObject[] itemsToDisable;
    private bool done;

    void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>())
        {
            return;
        }
        StartCoroutine(PickAfterDelay());
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<PlayerController>())
        {
            return;
        }
        StopAllCoroutines();
    }

    IEnumerator PickAfterDelay()
    {
        yield return new WaitForSeconds(pickDelay);
        Pick();
    }

    void Pick()
    {
        if (done)
        {
            return;
        }

        done = true;
        GameManager.Instance.flowersPicked += 1;
        foreach (GameObject item in itemsToDisable)
        {
            item.SetActive(false);
        }
    }
}
