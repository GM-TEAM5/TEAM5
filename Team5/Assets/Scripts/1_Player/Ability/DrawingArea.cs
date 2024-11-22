using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class DrawingArea : MonoBehaviour, ITimeScaleable
{
    [SerializeField] float targetRadius;
    [SerializeField] float activationTime;
    [SerializeField] float deactivationTime;
    [SerializeField] bool isActivated;
    [SerializeField] SpriteRenderer sr_area;
    Sequence seq_activation;
    private float timeScale = 1f;

    public void Init()
    {
        transform.localScale = targetRadius * Vector3.one;
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        PlaySeq_Activation(activationTime);
    }

    public void Deactivate()
    {
        PlaySeq_Deactivation(deactivationTime);
    }

    public void SetTimeScale(float scale)
    {
        timeScale = scale;
    }

    void PlaySeq_Activation(float duration)
    {
        if (seq_activation != null && seq_activation.IsActive())
        {
            seq_activation.Kill();
        }

        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        sr_area.color = new Color(sr_area.color.r, sr_area.color.g, sr_area.color.b, 0);

        seq_activation = DOTween.Sequence()
            .SetUpdate(true)
            .Append(transform.DOScale(targetRadius, duration).SetEase(Ease.OutCirc))
            .Join(sr_area.DOFade(1, duration * 0.5f))
            .Play();
    }

    void PlaySeq_Deactivation(float duration)
    {
        if (seq_activation != null && seq_activation.IsActive())
        {
            seq_activation.Kill();
        }

        seq_activation = DOTween.Sequence()
            .SetUpdate(true)
            .Append(sr_area.DOFade(0, duration))
            .Join(transform.DOScale(targetRadius * 2, duration).SetEase(Ease.OutCirc))
            .AppendCallback(() => gameObject.SetActive(false))
            .Play();
    }
}
