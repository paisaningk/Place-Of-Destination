using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Controller;

public class Arrow : MonoBehaviour
{

    public float speed;

    private Transform player;
    private Vector2 target;

    private PlayerController playerController;
 
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerHitBox"))
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
