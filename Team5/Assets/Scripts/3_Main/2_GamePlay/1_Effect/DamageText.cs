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

    // 등장 딜레이
    private const float APPEAR_DELAY_BASE = 0.05f;

    // Fade In 시간
    private const float FADE_IN_TIME = 0.15f;

    // 표시 지속 시간
    private const float DISPLAY_TIME_BASE = 0.5f;

    // Fade Out 시간
    private const float FADE_OUT_TIME = 0.3f;

    // 위로 올라가는 거리 (위로 쌓임)
    private const float MOVE_UP_DISTANCE = 1f;

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

        float appearDelay = APPEAR_DELAY_BASE * (activeTextCount - 1);
        float displayTime = DISPLAY_TIME_BASE + (APPEAR_DELAY_BASE * (activeTextCount - 1));
        PlayAnim_MoveAndFade(appearDelay, displayTime, textColor);
    }

    public void Init(Vector3 hitPoint, float damage, DamageType type = DamageType.DMG_NORMAL)
    {
        Init(hitPoint, damage.ToString("0"), type);
    }

    void PlayAnim_MoveAndFade(float appearDelay, float displayTime, Color textColor)
    {
        DOTween.Sequence()
            .OnComplete(() =>
            {
                PoolManager.Instance.TakeToPool<DamageText>(this);
                activeTextCount--;
            })
            .AppendInterval(appearDelay)
            .Append(text.DOColor(textColor, FADE_IN_TIME))
            .AppendInterval(displayTime)
            .Append(transform.DOLocalMoveY(transform.localPosition.y + MOVE_UP_DISTANCE, FADE_OUT_TIME))
            .Join(text.DOFade(0, FADE_OUT_TIME))
            .Play();
    }
}
