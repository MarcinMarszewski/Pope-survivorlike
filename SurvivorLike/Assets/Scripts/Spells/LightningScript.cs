using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningScript : MonoBehaviour
{
    [SerializeField]
    float damage;
    [SerializeField]
    float delayTime;

    float spawnTime;
    // Start is called before the first frame update

    private void Awake()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos) + new Vector3(0f, 1.0f);
    }
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnTime+delayTime<Time.time) GetComponent<CapsuleCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) collision.gameObject.GetComponent<DummyScript>().Damage(damage);
    }
}
