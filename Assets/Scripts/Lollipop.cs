using UnityEngine;

public class Lollipop : MonoBehaviour
{
    public string color;
    public DoorLickPuzzle doorLickPuzzle;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tongue"))
        {
            doorLickPuzzle.LickLollipop(color);
            
            Destroy(gameObject);
        }
    }
}