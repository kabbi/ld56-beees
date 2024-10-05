using UnityEngine;

public class BeeBonker : MonoBehaviour
{
    public float power;
    public AudioClip defaultClip;
    public bool onlyTagged;
    private AudioSource audioSource;
    private PlayerController playerController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider collider)
    {
        Bonkable tag = collider.GetComponent<Bonkable>();
        if (!tag && onlyTagged)
        {
            return;
        }
        audioSource.PlayOneShot(tag ? tag.clip : defaultClip);
        Vector3 direction = (collider.transform.position - transform.position).normalized;
        playerController.Bonk(direction * power);
    }
}
