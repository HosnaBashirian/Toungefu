using UnityEngine;
using DG.Tweening;

public class Tongue : MonoBehaviour
{
    private Rigidbody rb;
    
    public float extendedDistance = 2f;
    public float extendDuration  = 0.3f;
    public float retractDuration  = 0.1f;
    
    private Vector3 originalTonguePosition;
    private Vector3 originalTongueScale;
    
    private Tween lickTween;
    private Tween moveTween;
    
    private bool isInputAllowed = true;
    
    void Start()
    {
        originalTonguePosition = transform.localPosition;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInputAllowed)
        {
            isInputAllowed = false;
            
            if (lickTween != null && lickTween.IsActive())
            {
                lickTween.Kill();
                lickTween = null;
            }
            else
            {
                Lick();
            }
        }
    }

    private void Lick()  // scale and shoot forward
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
    
    
}
