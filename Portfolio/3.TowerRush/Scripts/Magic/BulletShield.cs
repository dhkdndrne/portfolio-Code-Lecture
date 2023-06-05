using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShield : MonoBehaviour
{
    Collider2D col;
    private void Start()
    {
        col = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D _Col)
    {
        if (_Col.CompareTag("Bullet"))
        {
            _Col.gameObject.GetComponent<Bullet>().PlayerParitcleCor();
        }
    }

}
