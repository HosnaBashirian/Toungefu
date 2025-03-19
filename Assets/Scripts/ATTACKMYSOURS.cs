using UnityEngine;

public class ATTACKMYSOURS : MonoBehaviour
{
    private Handler handler;

    void Start()
    {
      handler = GameObject.Find("GridManager").GetComponent<Handler>();
    }

    private void OnTriggerEnter(Collider other)
      {
        if (other.CompareTag("Player"))
        {
          handler.LevelReset();
        }
      }

  }
