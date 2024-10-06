using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    public MonoBehaviour[] componentsToDisable;
    public MonoBehaviour[] componentsToEnable;

    void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>())
        {
            return;
        }
        if (!GameManager.Instance.IsDone)
        {
            return;
        }
        TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        foreach (MonoBehaviour component in componentsToDisable)
        {
            component.enabled = false;
        }
        foreach (MonoBehaviour component in componentsToEnable)
        {
            component.enabled = true;
        }
    }
}
