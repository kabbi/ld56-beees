using UnityEngine;
using System;
using Lomont;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int flowersPicked;
    public int flowersTotal;

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
}
