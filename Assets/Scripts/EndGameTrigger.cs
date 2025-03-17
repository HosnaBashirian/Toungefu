using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
    public string endGameSceneName = "EndGameScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger. Loading end game scene...");
            
            SceneManager.LoadScene(endGameSceneName);
        }
    }
}