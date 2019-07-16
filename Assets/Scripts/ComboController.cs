using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{

    public List<Combo> Combos = new List<Combo>();

    public int indexHit = 0;
    private Coroutine timeOut;
    private Combo combo;


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

        if (Input.GetButtonDown("Fire1")){
            if(timeOut != null){
                StopCoroutine(timeOut);
            }
            if(indexHit == 0){
                Debug.Log("Starting Combo: "+ combo.name);
            }

            Hit actualHit = combo.hits[indexHit];
            Debug.Log(actualHit.name);
            
            indexHit++;
            
            
            if(indexHit >= combo.Count()){
                indexHit = 0;
                Debug.Log("End Combo");
                StartCoroutine(EnableCombo(combo.recoverTime, false));
            }
            else{
                StartCoroutine(EnableCombo(actualHit.recoverTime, false));
                timeOut = StartCoroutine(HitTimeOut(actualHit.chainTime));
            }
        }

    }

    IEnumerator EnableCombo(float time, bool option){
        Debug.Log("Recover Time");
        canCombo = option;
        yield return new WaitForSeconds(time);
        canCombo = !option;
    }

    IEnumerator HitTimeOut(float chainTime){
        yield return new WaitForSeconds(chainTime);
        indexHit = 0;
        Debug.Log("Time out");

    }
}
