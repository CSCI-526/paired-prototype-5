using UnityEngine;
using System.Collections.Generic;

public class JellyBoss : MonoBehaviour
{
    [Header("References")]
    public Transform[] players;
    private Transform currentTarget;
    public GameObject jellyProjectilePrefab;
    public Transform firePoint;

    [Header("Attack Settings")]
    // --- 【新增】攻击范围 ---
    [Tooltip("The boss will only attack if a player is within this distance.")]
    public float attackRange = 9f; 
    
    [Tooltip("Time in seconds between attacks.")]
    public float attackCooldown = 3f;
    private float lastAttackTime;

    [Header("Breathing Effect")]
    public float breathSpeed = 1f;
    public float breathAmount = 0.1f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 1. 寻找最近的玩家
        FindClosestPlayer();

        // 2. 呼吸效果持续播放
        HandleBreathing(); 

        // 3. 如果没有找到目标（比如玩家都阵亡了），就什么也不做
        if (currentTarget == null)
        {
            return; 
        }

        // --- 【核心修改】---
        // 4. 计算与最近目标的距离
        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

        // 5. 检查两个条件：
        //    a) 目标是否在攻击范围之内？
        //    b) 攻击冷却时间是否结束？
        if (distanceToTarget <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            // 只有两个条件都满足时，才发动攻击
            Attack();
        }
        // --------------------
    }

    void FindClosestPlayer()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (Transform playerTransform in players)
        {
            if (playerTransform.gameObject.activeInHierarchy)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer;
                    closestPlayer = playerTransform;
                }
            }
        }
        currentTarget = closestPlayer;
    }

    private void HandleBreathing()
    {
        float scaleOffset = 1 + Mathf.Sin(Time.time * breathSpeed) * breathAmount;
        transform.localScale = originalScale * scaleOffset;
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        Debug.Log("Jelly Boss is attacking " + currentTarget.name);

        if (jellyProjectilePrefab != null && firePoint != null)
        {
            GameObject projectileGO = Instantiate(jellyProjectilePrefab, firePoint.position, firePoint.rotation);
            JellyProjectile projectileScript = projectileGO.GetComponent<JellyProjectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(currentTarget);
            }
        }
        else
        {
            Debug.LogError("Projectile Prefab or Fire Point is not assigned in the JellyBoss script!");
        }
    }

    // 【推荐添加】在场景视图中画出攻击范围，方便调试
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}