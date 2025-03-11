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
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, maxHitDistance))
        {
            if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Eatable")
            {
                // if the object is eatable, destroy it
                Eat(hit.collider.gameObject);
            }
            else if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "LickableDoor")
            {
                // if the object is a lickable door, open it
                OpenDoor(hit.collider.gameObject);
            }
        }
    }

    void Eat(GameObject obj)
    {
        if (((1 << obj.layer) & eatable) != 0)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.DOFade(0f, 1f).OnComplete(() =>
                {
                    Destroy(obj);
                    Debug.Log(obj.name + " has been eaten!");
                });
            }
            else
            {
                Destroy(obj);
                Debug.Log(obj.name + " has been eaten (no fade)!");
            }
        }
        else
        {
            Debug.LogWarning("Cannot eat cuz it's not on the Eatable layer!");
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
