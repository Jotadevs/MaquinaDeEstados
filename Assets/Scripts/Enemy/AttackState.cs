using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float attackReadyTimer;
    private ElMalo malo;

    public AttackState(ElMalo elMalo) : base(elMalo.gameObject)
    {
        malo = elMalo;
    }

    public override Type Tick()
    {
        if (malo.Target == null) return typeof(WanderState);

        attackReadyTimer -= Time.deltaTime;

        if(attackReadyTimer <= 0f)
        {
            Debug.Log("Attack");
            malo.FireWeapon();
        }
        return null;
    }
}
