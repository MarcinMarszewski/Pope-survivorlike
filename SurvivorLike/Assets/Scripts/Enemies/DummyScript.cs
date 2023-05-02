using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DummyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float maxHealth;
    [SerializeField]
    float damageIndicationTime;
    [SerializeField]
    GameObject damageText;
    [SerializeField]
    Slider healthBar;

    [SerializeField]
    List<float> dropRates;
    [SerializeField]
    List<GameObject> drops;


    GameObject player;

    float lastDamageTime=0;
    System.Random rnd = new System.Random();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    void Update()
    {
        if (lastDamageTime+damageIndicationTime > Time.time) GetComponent<SpriteRenderer>().color = Color.red;
        else GetComponent<SpriteRenderer>().color = Color.white;

        if (transform.position.y > player.transform.position.y) GetComponent<SpriteRenderer>().sortingOrder = 9;
        else GetComponent<SpriteRenderer>().sortingOrder = 11;
    }

    public void Damage(float damage)
    {
        GameObject obj = Instantiate<GameObject>(damageText,transform);
        obj.GetComponentInChildren<UnityEngine.UI.Text>().text = damage.ToString();
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(rnd.Next(-10,10)/10.0f, 3.5f);
        Destroy(obj, 0.5f);

        healthBar.value -= damage;
        if (healthBar.value <= 0) Death();
        lastDamageTime = Time.time;
    }

    public void AddForce(Vector2 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force);
    }

    private void Death()
    {
        player.GetComponent<PlayerController>().KillBoost();
        for (int i = 0; i < dropRates.Count; i++)
        {
            while (dropRates[i]>1)
            {
                dropRates[i]--;
                Instantiate<GameObject>(drops[i], transform.position + new Vector3(rnd.Next(-10, 10) / 10f, rnd.Next(-10, 10) / 10f), Quaternion.identity);
            }
            if(rnd.Next(0,1000)/1000f<=dropRates[i]) Instantiate<GameObject>(drops[i], transform.position + new Vector3(rnd.Next(-10, 10) / 10f, rnd.Next(-10, 10) / 10f), Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
