using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/enemyModel",fileName = "EnemyModelSO")]
public class EnemyModelSO : ScriptableObject
{
	[field: SerializeField] public string ID;
	[field:SerializeField] public int Hp;
}
