using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParticleColor : MonoBehaviour
{
    public struct SVA
    {
        public float S;
        public float V;
        public float A;
    }

    public ParticleSystem[] particleSystems = new ParticleSystem[0];
    private List<SVA> svList = new List<SVA>();
    private float H;
    void Counter()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
       
        svList.Clear();
        foreach (var ps in particleSystems)
        {
            Color baseColor = ps.main.startColor.color;          
            SVA baseSVA = new SVA();
            Color.RGBToHSV(baseColor, out H, out baseSVA.S, out baseSVA.V);
            baseSVA.A = baseColor.a;
            svList.Add(baseSVA);
        }       
    }

    public void Change(float _HueColor)
    {
        Counter();
        int i = 0;
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            Color colorHSV = Color.HSVToRGB(_HueColor + H * 0, svList[i].S, svList[i].V);
            main.startColor = new Color(colorHSV.r, colorHSV.g, colorHSV.b, svList[i].A);
            i++;
        }

    }
}
