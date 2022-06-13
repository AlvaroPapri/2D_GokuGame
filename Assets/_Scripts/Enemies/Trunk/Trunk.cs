using System;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    private Animator anim; 
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private float distanceToPlayer;
    private float distanceToAttackPlayer;

    public enum State {Idle, Attacking}
    public State stateEnemy;

    public GameObject bulletPrefab;
    public Transform posRotBullet;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        switch (stateEnemy)
        {
            case State.Idle:
                if (distanceToPlayer < distanceToAttackPlayer)
                    stateEnemy = State.Attacking;
                break;
            
            case State.Attacking:
                if (distanceToPlayer > distanceToAttackPlayer)
                    stateEnemy = State.Idle;
                Attack();
                break;
        }
    }

    private void Attack()
    {
        Instantiate(bulletPrefab, posRotBullet.position, posRotBullet.rotation);
    }
}
