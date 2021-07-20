using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataController
{
    public const int maxStage = 49;
    public static List<Factory> Factories;
    public static List<PlayerMagic> PlayerMagic;
    public static List<string> UnitOrder = new List<string>();     // 출전 유닛 순서

    public static int CurrentStage; // 현재 스테이지
    public static bool isContinue;
    public static bool checkWorldClear; //월드 클리어 체크
    public static bool checkGameLoad;   //게임씬에서 왔는지 체크
    public static Item getItem; //스테이지 클리어하고 얻는 아이템

    public static int rewardGold;
    public static int rewardLuga;
}
