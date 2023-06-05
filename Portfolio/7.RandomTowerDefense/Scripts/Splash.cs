using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public Collider col;
    public float offTime;
    public bool longTime;
    public float damage;
    float time = 0.0f;

    private void Start()
    {
        col = GetComponent<Collider>();
        
    }

    public void Update()
    {
        time += (1f * Time.deltaTime);
        if (time >= offTime)
        {
            col.enabled = false;
        }      
    }
}
