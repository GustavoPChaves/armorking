using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combo System/Create Combo", order = 1)]
public class Combo : ScriptableObject
{
    public List<Hit> hits;
    public float recoverTime;
    public List<AnimationClip> animations;

    public int Count(){
        return hits.Count;
    }
}
