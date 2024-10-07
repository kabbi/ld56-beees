using UnityEngine;
using System;
using Lomont;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int flowersPicked;
    public int flowersTotal;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    private bool done;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Two game managers, killing myself");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool IsDone
    {
        get
        {
            return flowersPicked >= flowersTotal;
        }
    }

    void Update()
    {
        if (flowersPicked >= flowersTotal && !done)
        {
            done = true;
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in objectsToDeactivate)
            {
                obj.SetActive(false);
            }
        }
    }
}
