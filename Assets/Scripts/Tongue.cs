using System;
using UnityEngine;
using DG.Tweening;

public class Tongue : MonoBehaviour
{
    private Rigidbody rb;
    public PlayerTileMovements2 player;
    
    public float extendedDistance = 2f;
    public float extendDuration  = 0.3f;
    public float retractDuration  = 0.1f;
    
    private Vector3 originalTonguePosition;
    private Vector3 lickTarget;

    public LayerMask eatable;
    public LayerMask lickableDoor;
    
    private Tween lickTween;
    private Tween moveTween;
    
    public float maxDistance = 5f;
    public float maxHitDistance = 10f;
    // public int numberOfLicks;
    
    private bool isInputAllowed = true;
    private bool isObstacleThere = false;
    
    void Start()
    {
        originalTonguePosition = transform.localPosition;
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && isInputAllowed)
        {
            isInputAllowed = false;
            
            if (lickTween != null && lickTween.IsActive())
            {
                lickTween.Kill();
                lickTween = null;
            }
            else
            {
                LickAnimation();
            }
        }
    }

    private void LickAnimation()  // scale and shoot forward
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalMoveZ(originalTonguePosition.z + extendedDistance, extendDuration)
            .SetEase(Ease.OutQuad));
        sequence.Append(transform.DOPunchScale(new Vector3(0, 1f, 0), extendDuration, 2, 0).SetEase(Ease.OutQuad));
        
        sequence.OnComplete(() =>
        {
            transform.DOLocalMoveZ(originalTonguePosition.z, retractDuration).SetEase(Ease.OutQuad);
            lickTween = null;
            isInputAllowed = true;
            // Debug.Log(originalTonguePosition);
            PerformRaycast();
        });
        
        lickTween = sequence;
    }
    
    private void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, maxHitDistance, 
                eatable))
        {
            if (hit.collider == null)
            {
                Debug.LogWarning("Raycast hit an object without a collider!");
                return;
            }

            // Get the parent of the eatable GameObject
            GameObject parentEatable = hit.collider.transform.parent.gameObject;

            if (((1 << parentEatable.layer) & eatable) != 0) // Check if the parent is on the Eatable layer
            {
                // If the parent is eatable, destroy it and its children
                Eat(parentEatable);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    void Eat(GameObject obj)
    {
        // Ensure the object is on the Eatable layer
        if (((1 << obj.layer) & eatable) != 0)
        {
            // Destroy the object immediately
            Destroy(obj);
            Debug.Log(obj.name + " has been eaten!");
        }
        else
        {
            Debug.LogWarning("Cannot eat " + obj.name + " because it's not on the Eatable layer!");
        }
    }
    
    void OpenDoor(GameObject door)
    {
        // // I have to write a script for the door with an Open function
        // Door doorScript = door.GetComponent<Door>();
        // if (doorScript != null)
        // {
        //     doorScript.Open();
        // }
    }
	
	// It started freaking out on me so I commented it out for now...
	/*
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(player.transform.position, player.transform.forward * maxHitDistance);
        Gizmos.color = Color.blue;
    }
	*/
}
