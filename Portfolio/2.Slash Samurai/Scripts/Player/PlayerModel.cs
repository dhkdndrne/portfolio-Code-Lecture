using UnityEngine;

[System.Serializable]
public class PlayerModel
{
	public float minDragLength;  // 최소 드래그 길이
	public float atkRange;       // 공격 범위
	public float speed;          // 이동속도
	public bool isInAction;      // 현재 행동(이동)중 인지 체크
	public Vector3 v2Dir;        // 방향
	public Vector3 v2FirstTouch; // 첫 터치 좌표

}