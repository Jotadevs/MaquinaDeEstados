using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WanderState : BaseState
{
    private Vector3? destination;
    private float stopDistance = 1f;
    private float turnSpeed = 1f;
    private readonly LayerMask layerMask = LayerMask.NameToLayer("Walls");
    private float rayDistance = 3.5f;
    private Quaternion desiredRotation;
    private Vector3 direction;
    private ElMalo malo;
    public Transform _player;
    public PlayerMovement playerGameobj;


    
    public WanderState(ElMalo elMalo): base(elMalo.gameObject)
    {
        malo = elMalo;
    }

    public override Type Tick()
    {
        
        var chaseTarget = CheckForAggro();
        if(chaseTarget != null)
        {
            malo.SetTarget(chaseTarget);
            return typeof(ChaseState);
        }
        if(destination.HasValue == false || Vector3.Distance(transform.position, destination.Value) <= stopDistance)
        {
            FindRandomDestination();
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);

        if (IsForwardBlocked())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 0.2f);
        }
        else{
            transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.MaloSpeed);
        }

        Debug.DrawRay(transform.position, direction * rayDistance, Color.red);
        while (IsPathBlocked())
        {
            FindRandomDestination();
            Debug.Log("WALL");
        }
        return null;
    }
    private bool IsForwardBlocked()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.SphereCast(ray, 0.5f, rayDistance, layerMask);
    }
    private bool IsPathBlocked()
    {
        Ray ray = new Ray(transform.position, direction);
        return Physics.SphereCast(ray, 0.5f, rayDistance, layerMask);
    }
    private void FindRandomDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) + new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f, UnityEngine.Random.Range(-4.5f, 4.5f));
        destination = new Vector3(testPosition.x, 1f, testPosition.z);
        direction = Vector3.Normalize(destination.Value - transform.position);
        direction = new Vector3(direction.x, 0f, direction.z);
        desiredRotation = Quaternion.LookRotation(direction);
        Debug.Log("Got Direction");
    }
    
    Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    private Transform CheckForAggro()
    {
        _player = playerGameobj.playerTrasform;
        float aggroRadius = 5f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for (var i = 0; i < 24; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                //var elMalo = hit.collider.GetComponent<ElMaloController>();
                if (/*elMalo != null &&*/ hit.collider.tag == "Player")
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return _player;
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(pos, direction * aggroRadius, Color.white);
            }
            direction = stepAngle * direction;
        }
        return null;
    }
}
