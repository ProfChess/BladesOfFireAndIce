using UnityEngine;

public abstract class BaseTempBuff : InteractableObject
{
    //Buff Duration
    public float BuffDuration = 0f;
    public TempBuffType BuffType;

    //Apply Bonus To Player on Interact
    public override void Interact()
    {
        base.Interact();
        if (GameManager.Instance == null) { return; }
        GameManager.Instance.getPlayer().GetComponentInChildren<PlayerBuffStorage>().AddBuff(this);
    }

    public abstract void ApplyBuff(PlayerStatSetting Stats);
    public abstract void DeactivateBuff(PlayerStatSetting Stats);
}
public enum TempBuffType { Vitality, Strength, Stamina, Dexterity, Luck}