using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Combo System/Create Hit", order = 1)]
public class Hit : ScriptableObject
{
    public float damage;
    public float recoverTime;
    public float chainTime;
    public string key;

}
