using UnityEngine;

[CreateAssetMenu(fileName = "SkillItem_2002_E", menuName = "SO/SkillItem/2002_ESkill", order = int.MaxValue)]
public class ItemSO_ESkill : SkillItemSO
{
    public override string id => "2002";
    public override string dataName => "ESkill";

    private PlayerDraw playerDraw;
    private Player player;

    public override void OnEquip()
    {
        player = Player.Instance;
        playerDraw = player.GetComponentInChildren<PlayerDraw>();
    }

    public override void Use()
    {
        Debug.Log("Toggling draw mode");
        playerDraw.ToggleDraw();
    }

    public override void OnUnEquip()
    {
        if (playerDraw.isDrawing)
        {
            playerDraw.FinishDraw();
        }
    }
}