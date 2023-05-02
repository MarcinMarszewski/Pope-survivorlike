using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PulsatorScript : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float midSize;
    [SerializeField]
    float difference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3((Mathf.Sin(Time.unscaledTime*speed)/difference) +midSize, (Mathf.Sin(Time.unscaledTime * speed) / difference) + midSize);
    }
}
