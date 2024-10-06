using UnityEngine;
using TMPro;

public class FlowerCollectionStatusDisplay : MonoBehaviour
{
    public TextMeshProUGUI label;

    void Update()
    {
        label.text = $"{GameManager.Instance.flowersPicked} / {GameManager.Instance.flowersTotal}";
    }
}
