using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class AbilityEffectLibrary
{
    public static void PlayAbilityEffect(Ability ability)
    {
        switch (ability.EffectToTrigger)
        {
            case PlayerAbilityType.None: return;
            case PlayerAbilityType.FireSmash: Ability_FireSmash(ability); return;
            case PlayerAbilityType.IceSmash: Ability_IceSmash(); return;
        }
    }
    private static void Ability_FireSmash(Ability ability)
    {
        //Get Player Location
        Vector2 PlayerLocation = GameManager.Instance.getPlayer().transform.position;

        //Get Object From Pool
        GameObject Explosion = PlayerEffectPoolManager.Instance.getObjectFromPool(PlayerEffectObjectType.AbilityFlame);

        if (Explosion != null)
        {
            EffectBaseStats Stats = ability.BaseStats;
            Explosion.GetComponent<BaseEffectSpawn>().Spawn(PlayerLocation,
                Stats.Area,
                Stats.Damage,
                Stats.Freq,
                Stats.Duration);
        }
    }
    private static void Ability_IceSmash()
    {
        Debug.Log("Spawn Ice Smash");
    }
  
}
