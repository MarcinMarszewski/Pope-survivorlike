using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float speed;

    public float damage = 1;
    private PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(Input.mousePosition.y - objectPos.y, Input.mousePosition.x - objectPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        transform.position += transform.up;
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(transform.up.x, transform.up.y) * speed;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<DummyScript>().Damage(damage /*+ player.CurrentKillBoost * player.killBoostMultiplier*/);
            Destroy(gameObject);
        }
    }
}
