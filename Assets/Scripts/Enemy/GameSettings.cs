using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private float maloSpeed = 2f;
    public static float MaloSpeed => Instance.maloSpeed;

    [SerializeField] private float aggroRadius = 4f;
    public static float AggroRadius => Instance.aggroRadius;

    [SerializeField] private float attackRange = 3f;
    public static float AttackRange => Instance.attackRange;

    [SerializeField] private GameObject maloProjectilPrefab;
    public static GameObject MaloProjectilPrefab => Instance.maloProjectilPrefab;

    public static GameSettings Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
