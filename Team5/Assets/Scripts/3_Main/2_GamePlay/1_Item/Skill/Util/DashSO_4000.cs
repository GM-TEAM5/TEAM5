using UnityEngine;

[CreateAssetMenu(fileName = "4000", menuName = "SO/SkillItem/4000_Dash", order = int.MaxValue)]
public class DashSO_4000 : DashSO
{
    public override string id => $"4000_{property}";
    public override string dataName => $"DashSkill_{property}";

    public DashSO_4000()
    {
        dashMultiplier = 3f;
        dashDuration = 0.2f;

        trailStartColor = new Color(0.2f, 0.5f, 1f, 1f);
        trailEndColor = new Color(0.2f, 0.5f, 1f, 0.7f);
        trailStartWidth = 1f;
        trailEndWidth = 0.5f;
    }
}