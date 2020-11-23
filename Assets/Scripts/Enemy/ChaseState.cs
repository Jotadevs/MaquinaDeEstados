using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    private ElMalo malo;

    public ChaseState(ElMalo elMalo) : base(elMalo.gameObject)
    {
        malo = elMalo;
    }

    public override Type Tick()
    {
        if (malo.Target == null) return typeof(WanderState);

        transform.LookAt(malo.Target);
        transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.MaloSpeed);

        var Distance = Vector3.Distance(transform.position, malo.Target.transform.position);
        if (Distance <= GameSettings.AttackRange) return typeof(AttackState);
        return null;
    }
}
