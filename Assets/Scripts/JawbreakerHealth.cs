using UnityEngine;

public class JawbreakerHealth : MonoBehaviour
{
    public GameObject jawbreaker3HP;
    public GameObject jawbreaker2HP;
    public GameObject jawbreaker1HP;

    private int lickCount = 0;
    public string triggeringTag = "Tongue";

    void Start()
    {
        jawbreaker3HP = transform.Find("JAWBREAKER3HP").gameObject;
        jawbreaker2HP = transform.Find("JAWBREAKER2HP").gameObject;
        jawbreaker1HP = transform.Find("JAWBREAKER1HP").gameObject;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(triggeringTag))
        {
            JawBreakerDestruction();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            JawBreakerDestruction();
        }
    }

    void JawBreakerDestruction()
    {
        if (lickCount == 0 && jawbreaker3HP != null)
        {
            Destroy(jawbreaker3HP);
            lickCount++;
        }
        else if (lickCount == 1 && jawbreaker2HP != null)
        {
            Destroy(jawbreaker2HP);
            lickCount++;
        }
        else if (lickCount == 2 && jawbreaker1HP != null)
        {
            Destroy(jawbreaker1HP);
            lickCount++;
        }
    }
}
