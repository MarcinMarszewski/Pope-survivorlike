using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float damage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized);
        if (GetComponent<Rigidbody2D>().velocity.magnitude > movementSpeed) GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * (movementSpeed / Mathf.Sqrt(Mathf.Pow(GetComponent<Rigidbody2D>().velocity.normalized.x, 2) + Mathf.Pow(GetComponent<Rigidbody2D>().velocity.normalized.y, 2)));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) collision.gameObject.GetComponent<PlayerController>().Damage(damage);
    }

}
