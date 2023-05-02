using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPushScript : MonoBehaviour
{
    [SerializeField]
    float damage;
    [SerializeField]
    float knockbackForce;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(Input.mousePosition.y - objectPos.y, Input.mousePosition.x - objectPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        transform.position += transform.up * 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().size += new Vector2(Time.deltaTime * 0.19f, 0f);
        GetComponent<BoxCollider2D>().offset += new Vector2(0f, Time.deltaTime * 0.45f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<DummyScript>().Damage(damage);
            collision.gameObject.GetComponent<DummyScript>().AddForce(transform.up*knockbackForce);
        }
    }
}
