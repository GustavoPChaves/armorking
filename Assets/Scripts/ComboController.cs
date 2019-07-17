using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{

    public List<Combo> Combos = new List<Combo>();

    public int indexHit = 0;
    private Coroutine timeOut, waitHit;
    private Combo combo;
    public List<string> InputKeys;

    //Bools
    private bool canCombo = true;
    // Start is called before the first frame update
    void Start()
    {
        combo = Combos[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(!canCombo) return;

        foreach (var key in InputKeys)
        {
            if (Input.GetButtonDown(key)){
                
                ComboBuilder(key);
                
            }
        }
        
    }

    void ComboBuilder(string inputKey){

        
        
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

        if(waitHit != null){
            StopCoroutine(waitHit);
        }
        if(timeOut != null){
            StopCoroutine(timeOut);
        }

        else if(actualHit.key != inputKey){
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

    Combo StartCombo(string inputKey){

        
        foreach (var combo in Combos)
        {
            if(combo.hits[0].key == inputKey){
                Debug.Log("Starting Combo: "+ combo.name);
                return combo;
            }
        }
        return null;
    }

    void EndCombo(Combo combo, bool complete = true){
        indexHit = 0;
        Debug.Log("End Combo: " + (complete ? "Completed": "Failed"));
        StartCoroutine(EnableCombo(combo.recoverTime, false));
    }

    IEnumerator EnableCombo(float time, bool option){
        //Debug.Log("Recover Time");
        canCombo = option;
        yield return new WaitForSeconds(time);
        canCombo = !option;
    }

    IEnumerator WaitHit(float time){
        yield return new WaitForSeconds(time);
        ComboBuilder("Wait");
    }

    IEnumerator HitTimeOut(float chainTime, Combo combo){
        yield return new WaitForSeconds(chainTime);
        EndCombo(combo, false);
        indexHit = 0;
        //Debug.Log("Time out");

    }
}
