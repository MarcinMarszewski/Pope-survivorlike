using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{

    [SerializeField]
    float speed;

    GameObject player;
    System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Rigidbody2D>().velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position) * speed;
        GetComponent<Rigidbody2D>().angularVelocity = rnd.Next(-1000, 1000) / 10.0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
