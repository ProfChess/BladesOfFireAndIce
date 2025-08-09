using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTankMovement : BaseBoss
{
    protected override void AttackSelection()
    {

    }

    protected override void MoveUpdate()
    {
        BossAgent.SetDestination(playerLocation.position);
    }

}
