using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum LogType
    {
        Log,
        LogError,
        Warning,
        Try,
        Success
    }

    public static readonly string WALL_TAG = "Wall";
    public static readonly string ENEMY_TAG = "Enemy";
    public static readonly string SWITCH_TAG = "Switch";
    public static readonly string SWITCHWALL_TAG = "SwitchWall";
    public static readonly string PLAYER_TAG = "Player";
    public static readonly string ITEM_TAG = "Item";
}
