using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public Material mat;
    public bool isStart = true;
    // Start is called before the first frame update
    void Start()
    {
        mat = transform.GetComponent<Renderer>().material;
        mat.SetFloat("_DissolveAmount", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isStart)
        {
            UpdateHideOff();
        }
        
    }

    public void UpdateHideOff()
    {
        float dissolveAmount = mat.GetFloat("_DissolveAmount");
        if (dissolveAmount > 0.0f)
        {
            mat.SetFloat("_DissolveAmount", dissolveAmount - (0.5f * Time.deltaTime));
        }
        else
        {
            mat.SetFloat("_DissolveAmount", 0.0f);
            isStart = false;
        }
    }
}
