using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;


public enum DamageType
{
    DMG_NORMAL,
    DMG_CRITICAL,
    DMG_TICK,
    DMG_PLAYER,
    HEAL_PLAYER
}

public class DamageText : MonoBehaviour, IPoolObject
{
    TextMeshPro text;
    private static float lastYOffset = 0f;
    private static float yOffsetIncrement = 1.0f;
    private static float resetTime = 0.5f;
    private static float lastTextTime;
    private static int activeTextCount = 0;

    public void OnCreatedInPool()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void OnGettingFromPool()
    {
        text.color = Color.white;
    }

    public void Init(Vector3 hitPoint, string content, DamageType type = DamageType.DMG_NORMAL)
    {
        if (Time.time - lastTextTime > resetTime)
        {
            lastYOffset = 0f;
            activeTextCount = 0;
        }

        lastYOffset += yOffsetIncrement;
        lastTextTime = Time.time;
        activeTextCount++;

        // 초기에는 투명하게 시작
        text.color = new Color(1, 1, 1, 0);
        transform.position = hitPoint + new Vector3(0, lastYOffset, 0);

        text.SetText(content);
        Color textColor = Color.white;
        switch (type)
        {
            case DamageType.DMG_CRITICAL:
                textColor = new Color(1, 1, 0.5f);
                break;
            case DamageType.DMG_PLAYER:
                textColor = new Color(1, 0.2f, 0.2f);
                break;
            case DamageType.HEAL_PLAYER:
                textColor = new Color(0, 1, 0.5f);
                break;
        }

        float appearDelay = 0.1f * (activeTextCount - 1);
        float fadeDelay = 1.0f + (0.1f * (activeTextCount - 1));
        PlayAnim_MoveAndFade(appearDelay, fadeDelay, textColor);
    }

    public void Init(Vector3 hitPoint, float damage, DamageType type = DamageType.DMG_NORMAL)
    {
        Init(hitPoint, damage.ToString("0"), type);
    }

    void PlayAnim_MoveAndFade(float appearDelay, float fadeDelay, Color textColor)
    {
        DOTween.Sequence()
            .OnComplete(() =>
            {
                PoolManager.Instance.TakeToPool<DamageText>(this);
                activeTextCount--;
            })
            .AppendInterval(appearDelay * 0.5f)
            .Append(text.DOColor(textColor, 0.15f))
            .AppendInterval(fadeDelay * 0.5f)
            .Append(transform.DOLocalMoveY(transform.localPosition.y + 1f, 0.3f))
            .Join(text.DOFade(0, 0.3f))
            .Play();
    }
}
