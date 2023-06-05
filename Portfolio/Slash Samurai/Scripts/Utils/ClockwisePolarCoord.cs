using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ClockwisePolarCoord
{   
    /// <summary> 반지름 </summary>
    public float Radius { get; set; }
    /// <summary> 0 ~ 360 각도 </summary>
    public float Angle
    {
        get => _angle;
        set => _angle = ClampAngle(value);
    }
    private float _angle;
    
    public ClockwisePolarCoord(float radius, float angle)
    {
        Radius = radius;
        _angle = ClampAngle(angle);
    }
    private static float ClampAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f)
            angle += 360f;
        return angle;
    }
    private static float CovertAngle(float angle)
        => 90f - angle;
    
    public static ClockwisePolarCoord Zero => new ClockwisePolarCoord(0f, 0f);
    
    public static ClockwisePolarCoord FromVector2(in Vector2 vec)
    {
        if (vec == Vector2.zero)
            return Zero;

        float radius = vec.magnitude;
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

        return new ClockwisePolarCoord(radius, CovertAngle(angle));
    }
}
