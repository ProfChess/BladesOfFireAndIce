using UnityEngine;

public class CasterVisuals : MonoBehaviour
{
    [SerializeField] private CasterEnemy caster;
    private static readonly int AttackSpeedMod = Animator.StringToHash("NormalAttackSpeed");
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void TeleportRegular() { caster.TeleportRegular(); }
    public void TeleportAfterCast() { caster.TeleportAfterCast(); }
    public void ConsiderTeleportAfterCast() { caster.ConsiderTeleportAfterCast(); }
    public void MagicAttackSpawn() { caster.SpawnMagicAttack(); }

    //Attack Speed Changes
    public void SlowAttack()
    {
        anim.SetFloat(AttackSpeedMod, 0.25f);
    }
    public void RegularAttackSpeed()
    {
        anim.SetFloat(AttackSpeedMod, 1f);
    }

    //Attack Tiggers
    public void NormalAttack() { caster.NormalAttack(); }
}
