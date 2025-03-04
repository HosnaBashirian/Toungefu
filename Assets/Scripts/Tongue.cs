using UnityEngine;
using DG.Tweening;

public class Tongue : MonoBehaviour
{
    private Rigidbody rb;
    
    public float extendedDistance = 2f;
    public float extendDuration  = 0.3f;
    public float retractDuration  = 0.1f;
    
    private Vector3 originalTonguePosition;
    private Vector3 lickTarget;
    
    private Tween lickTween;
    private Tween moveTween;
    
    public float maxDistance = 5f;
    
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
                if (CheckForObstacles(lickTarget))
                {
                    
                }
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
        });
        
        lickTween = sequence;
    }

    private bool CheckForObstacles(Vector3 position)
    {

        return isObstacleThere;
    }

    void PickObject(GameObject obj)
     {
            Debug.Log(obj.name);
            Destroy(obj);
     }

    
}
