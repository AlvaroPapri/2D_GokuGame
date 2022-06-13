using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int speed;
    public Vector2 leftLimit;
    public Vector2 rightLimit;

    public float distanceToFollowingPlayer;
    public float xFactor;
    public float yFactor;
    
    public enum State {Patrol, Following};
    public State stateEnemy;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private Vector2 posToGo;
    private GameObject player;
    private float distanceToPlayer;
    private bool isFollowingPlayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        posToGo = new Vector2(rightLimit.x, transform.position.y);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        switch (stateEnemy)
        {
            case State.Patrol:
                if (distanceToPlayer < distanceToFollowingPlayer)
                    stateEnemy = State.Following;
                Patrol();
                break;
            
            case State.Following:
                FollowPlayer();
                break;
        }

        Flip();
    }

    private void Patrol()
    {
        isFollowingPlayer = false;

        if (transform.position.x == posToGo.x)
        {
            if (posToGo == rightLimit)
                posToGo = new Vector2(leftLimit.x, transform.position.y);
            else
                posToGo = new Vector2(rightLimit.x, transform.position.y);
        }

        transform.position = Vector2.MoveTowards(
            transform.position, posToGo, speed * Time.deltaTime);
        
    }

    private void FollowPlayer()
    {
        isFollowingPlayer = true;
        float direction;

        if (player.transform.position.x > transform.position.x)
            direction = -1;
        else
            direction = 1;

        posToGo = new Vector2(player.transform.position.x + (xFactor * direction), player.transform.position.y + yFactor);
        transform.position = Vector2.MoveTowards(
            transform.position, posToGo, speed * Time.deltaTime);
    }

    private void Flip()
    {
        Vector2 target;

        if (isFollowingPlayer)
            target = player.transform.position;
        else
            target = posToGo;

        if (target.x > transform.position.x)
            spriteRenderer.flipX = true;
        else if (target.x < transform.position.x)
            spriteRenderer.flipX = false;
    }
}
