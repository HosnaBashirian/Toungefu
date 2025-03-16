using System;
using UnityEngine;
using DG.Tweening;

public class Tongue : MonoBehaviour
{
    private Rigidbody rb;
    public PlayerTileMovements2 player;

    public float extendedDistance = 2f;
    public float extendDuration = 0.3f;
    public float retractDuration = 0.1f;

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

    private Handler _handler;

    void Start()
    {
        originalTonguePosition = transform.localPosition;

        _handler = GameObject.Find("GridManager").GetComponent<Handler>();
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

            _handler.ProcessPlayerAttack();
        }
    }

    private void LickAnimation() // scale and shoot forward
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

            // get the parent of the eatable GameObject
            GameObject parentEatable = hit.collider.transform.parent.gameObject;

            if (((1 << parentEatable.layer) & eatable) != 0) // Check if the parent is on the Eatable layer
            {
                // if the parent is eatable, destroy it and its children
                Eat(parentEatable);
            }

            Lollipop lollipop = hit.collider.GetComponent<Lollipop>();
            if (lollipop != null)
            {
                hit.collider.enabled = false;
                lollipop.OnTriggerEnter(hit.collider);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    void Eat(GameObject obj)
    {
        if (((1 << obj.layer) & eatable) != 0)
        {
            Destroy(obj);
            Debug.Log(obj.name + " has been eaten!");
        }
        else
        {
            Debug.LogWarning("Cannot eat " + obj.name + " because it's not on the Eatable layer!");
        }
    }
}

// It started freaking out on me so I commented it out for now...
	// 🫡🫡🫡
