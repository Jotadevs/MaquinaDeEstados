using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElMaloController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private float attackRange = 3f;
    private float rayDistance = 5f;
    private float stoppingDistance = 5f;

    private Vector3 destination;
    private Quaternion desireRotation;
    private Vector3 direction;
    private ElMaloController target;
    private ElMaloState currentState;

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case ElMaloState.Wander:
                {
                    if (NeedsDestination())
                    {
                        GetDestination();
                    }

                    transform.rotation = desireRotation;
                    transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                    var rayColor = IsPathBlocked() ? Color.red : Color.green;
                    Debug.DrawRay(transform.position, direction * rayDistance, rayColor);

                    while (IsPathBlocked())
                    {
                        Debug.Log("Path Blocked");
                        GetDestination();
                    }

                    var targetToAggro = CheckForAggro();
                    if (targetToAggro != null)
                    {
                        target = targetToAggro.GetComponent<ElMaloController>();
                        currentState = ElMaloState.Chase;
                    }
                    break;
                }
            case ElMaloState.Chase:
                {
                    if(target == null)
                    {
                        currentState = ElMaloState.Wander;
                        return;
                    }

                    transform.LookAt(target.transform);
                    transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                    if(Vector3.Distance( transform.position, target.transform.position) < attackRange)
                    {
                        currentState = ElMaloState.Attack;
                    }
                    break;
                }
            case ElMaloState.Attack:
                {
                    if(target != null)
                    {
                        Destroy(target.gameObject);
                    }
                    currentState = ElMaloState.Wander;
                    break;
                }
                
                
        }
    }
    private bool IsPathBlocked()
    {
        Ray ray = new Ray(transform.position, direction);
        var hitSomething = Physics.RaycastAll(ray, rayDistance, _layerMask);
        return hitSomething.Any();
    }
    public void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) + new Vector3(Random.Range(-4.5f, 4.5f), 0f, Random.Range(-4.5f, 4.5f));
        destination = new Vector3(testPosition.x, 1f, testPosition.z);
        direction = Vector3.Normalize(destination - transform.position);
        direction = new Vector3(direction.x, 0f, direction.z);
        desireRotation = Quaternion.LookRotation(direction);
    }
    public bool NeedsDestination()
    {
        if (destination == Vector3.zero) return true;
        var distance = Vector3.Distance(transform.position, destination);
        if (distance <= stoppingDistance) return true;
        return false;
    }

    Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    private Transform CheckForAggro()
    {
        float aggroRadius = 5f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for(var i = 0; i < 24; i++)
        {
            if(Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                var elMalo = hit.collider.GetComponent<ElMaloController>();
                if(elMalo != null && hit.collider.name == "Player")
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return elMalo.transform;
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
public enum ElMaloState
{
    Wander,
    Chase,
    Attack
}
