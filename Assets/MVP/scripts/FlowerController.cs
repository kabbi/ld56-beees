using UnityEngine;
using System.Collections;

public class FlowerController : MonoBehaviour
{
    public float pickDelay = 3;
    public GameObject[] itemsToEnable;
    public GameObject[] itemsToDisable;
    private bool done;

    void OnTriggerEnter(Collider other)
    {
        if (done)
        {
            return;
        }
        if (!other.GetComponent<PlayerController>())
        {
            return;
        }
        StartCoroutine(PickAfterDelay());
    }

    void OnTriggerExit(Collider other)
    {
        if (done)
        {
            return;
        }
        if (!other.GetComponent<PlayerController>())
        {
            return;
        }
        StopAllCoroutines();
        StopPicking();
    }

    IEnumerator PickAfterDelay()
    {
        StartPicking();
        yield return new WaitForSeconds(pickDelay);
        ConfirmPicking();
    }

    void StartPicking()
    {
        foreach (GameObject item in itemsToEnable)
        {
            ParticleSystem particleSystem = item.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                particleSystem.Play(true);
            }
            else
            {
                item.SetActive(true);
            }
        }
    }

    void StopPicking()
    {

        foreach (GameObject item in itemsToEnable)
        {
            ParticleSystem particleSystem = item.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                particleSystem.Stop(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    void ConfirmPicking()
    {
        done = true;
        GameManager.Instance.flowersPicked += 1;
        foreach (GameObject item in itemsToEnable)
        {
            ParticleSystem particleSystem = item.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                particleSystem.Stop(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
        foreach (GameObject item in itemsToDisable)
        {
            ParticleSystem particleSystem = item.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                particleSystem.Stop(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }
}
