using UnityEngine;

public class NPCBattleManager : BattleManager
{
    public static NPCBattleManager instanceNPC;

    protected override void Start()
    {
        instanceNPC = this;
    }

    protected override void CheckCoin()
    {
        int card = 3;

        if (firstAttack)
        {
            crystalTotal = 1;
            crystal = 1;
            card = 4;
        }

        Crystal();
    }
}
