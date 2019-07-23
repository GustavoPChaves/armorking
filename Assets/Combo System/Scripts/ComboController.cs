using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComboController : MonoBehaviour
{

    public List<Combo> Combos = new List<Combo>();
    
    private int indexHit = 0;

    private Coroutine timeOut, waitHit;

    private Combo combo;
    public List<string> InputKeys;

    //Bools
    private bool canCombo = true;

    void Start()
    {
        combo = Combos[0];
    }

    void Update()
    {
        if(!canCombo) return;
        foreach (var key in InputKeys)
        {
            if (Input.GetButtonDown(key))
            {
                
                ComboBuilder(key);
            }
        }
    }

    void ComboBuilder(string inputKey)
    {
        if(indexHit == 0){
            combo = StartCombo(inputKey);
            if(combo == null){
                return;
            }
        }

        Hit actualHit = combo.hits[indexHit];

        if(actualHit.key != inputKey && inputKey == "Wait"){
            return;
        }
        if(timeOut != null){
            StopCoroutine(timeOut);
        }
        if(waitHit != null){
            StopCoroutine(waitHit);
        }
        
        if(actualHit.key != inputKey){
            EndCombo(combo, false);
            return;
        }

        Debug.Log(actualHit.name);
        
        indexHit++;
        
        if(indexHit >= combo.Count()){
            EndCombo(combo);
        }
        else{
            StartCoroutine(EnableCombo(actualHit.recoverTime, false));
            timeOut = StartCoroutine(HitTimeOut(actualHit.chainTime, combo));
            waitHit = StartCoroutine(WaitHit(actualHit.recoverTime));
        }
    }

    Combo StartCombo(string inputKey)
    {
        foreach (var combo in Combos)
        {
            if(combo.hits[0].key == inputKey){
                Debug.Log("Starting Combo: "+ combo.name);
                return combo;
            }
        }
        return null;
    }

    void EndCombo(Combo combo, bool complete = true)
    {
        indexHit = 0;
        Debug.Log("End Combo: " + (complete ? "Completed": "Failed"));
        StartCoroutine(EnableCombo(combo.recoverTime, false));
    }

    IEnumerator EnableCombo(float time, bool option)
    {
        //Debug.Log("Recover Time");
        canCombo = option;
        yield return new WaitForSeconds(time);
        canCombo = !option;
    }

    IEnumerator WaitHit(float time)
    {
        yield return new WaitForSeconds(time);
        ComboBuilder("Wait");
    }

    IEnumerator HitTimeOut(float chainTime, Combo combo)
    {
        yield return new WaitForSeconds(chainTime);
        EndCombo(combo, false);
        indexHit = 0;
        //Debug.Log("Time out");
    }

    void OnValidate(){
    }

    public void OrderList(){
        Combos.Sort(CompareCombo);
    }

    int CompareCombo(Combo c1, Combo c2){
        int index = 0;
        int result = CompareHit(c1.hits[index], c2.hits[index]);
        
        if(result == 0){
            int count1 = c1.hits.Count;
            int count2 = c2.hits.Count;

            while(result == 0){
                
                index++;
                if(index >= count1 && index < count2){
                    return -1;
                }
                else if(index < count1 && index >= count2){
                    return 1;
                }
                else if(index >= count1 && index >= count2 ){
                    return 0;
                }
                result = CompareHit(c1.hits[index], c2.hits[index]);
            }
        }
        return result;
    }

    int CompareHit(Hit h1, Hit h2){
        return h1.name.CompareTo(h2.name);
    }

    public void RemoveDuplicates(){
        Combos = new HashSet<Combo>(Combos).ToList();
    }
}
