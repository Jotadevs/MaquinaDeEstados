using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElMalo : MonoBehaviour
{
    [SerializeField] private GameObject laser;

    public Transform Target { get; private set; }

    public StateMachine StateMachine => GetComponent<StateMachine>();


    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            {typeof(WanderState), new WanderState(this) },
            {typeof(ChaseState), new ChaseState(this) },
            {typeof(AttackState), new AttackState(this) }
        };
        GetComponent<StateMachine>().SetStates(states);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void FireWeapon()
    {
        laser.transform.position = (Target.position + transform.position) / 2f;

        float distance = Vector3.Distance(Target.position, transform.position);
        laser.transform.localScale = new Vector3(0.1f, 0.1f, distance);
        laser.SetActive(true);

        StartCoroutine(TurnOffLaser());
    }
    private IEnumerator TurnOffLaser()
    {
        yield return new WaitForSeconds(0.25f);
        laser.SetActive(false);

        if(Target != null)
        {
            Destroy(Target.gameObject);
        }
    }
}
