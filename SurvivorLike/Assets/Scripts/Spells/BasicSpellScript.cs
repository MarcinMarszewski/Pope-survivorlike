using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellScript : MonoBehaviour
{
    [SerializeField]
    float lifetime;
    [SerializeField]
    GameObject toSpawn;
    [SerializeField]
    Sprite icon;

    public Skills skillNeeded;

    public Sprite Icon { get { return icon; } }

    public float manaCost;

    float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnTime+lifetime<Time.time)
        {
            if (toSpawn != null)
            {
                Instantiate<GameObject>(toSpawn,transform.position,Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
