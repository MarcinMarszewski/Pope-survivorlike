using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBombScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    float damage;
    [SerializeField]
    float knockbackForce;

    private void Awake()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<CircleCollider2D>().radius += 0.27f * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<DummyScript>().Damage(damage);
            collision.gameObject.GetComponent<DummyScript>().AddForce((collision.gameObject.transform.position-transform.position).normalized * knockbackForce);
        }
    }
}
