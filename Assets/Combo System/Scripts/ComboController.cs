using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class ComboController : MonoBehaviour
{

    public List<Combo> Combos = new List<Combo>();

    private List<Combo> filterCombos = new List<Combo>();
    private int indexHit = 0;

    private Coroutine timeOut, waitHit;

    private Combo combo;
    public List<string> InputKeys;

    public Text hitText, comboText, inputText, statusText, indexText;

    private Animator animator;
    //Bools
    private bool canCombo = true;
    private bool completedPreviousCombo = false;
    private Combo previousCombo;
    private float animationFade = 0.1f;
    void Start()
    {
        combo = Combos[0];
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!canCombo) return;
        foreach (var key in InputKeys)
        {
            if (Input.GetButtonDown(key))
            {
                inputText.text = "Key: " + key;
                ComboBuilder(key);
            }
            
        }

        if(Input.GetButtonDown("Submit")){
            PrintList(Combos);
        }
    }

    void ComboBuilder(string inputKey)
    {
        if(!canCombo) return;

        if(indexHit == 0 ) statusText.text = "Starting Combo";
        filterCombos = FilterCombos((indexHit == 0 ? Combos : filterCombos), inputKey, indexHit);
        
        if(filterCombos == null || !filterCombos.Any()){
            //EndCombo();
            return;
        }
        indexText.text = "Index: " + (indexHit + 1).ToString();
        combo = filterCombos.First();

        
        

        comboText.text = "Combo: " + combo.name;
        if(completedPreviousCombo){
            comboText.text = "Combo: " + previousCombo.name;
        }

        Hit actualHit = GetHit(combo, indexHit);
        var animationClip = GetAnimation(combo);
        if(inputKey != "Wait"){
            animator.CrossFade(animationClip.name, animationFade);
        }
        // if(actualHit.name == "Soco"){
        //     animator.SetBool("Punch", true);
        //     animator.SetInteger("ChainNumber", indexHit);
        // }
        // if(actualHit.name == "Chute"){
        //     animator.SetBool("Kick", true);
        //     animator.SetInteger("ChainNumber", indexHit);
        // }
        
        hitText.text = "Hit: "+ actualHit.name;

        if(actualHit.name != "Wait"){
            completedPreviousCombo = false;
        }
        StopComboCoroutines();
        
        indexHit++;

        
        completedPreviousCombo = CheckCompleteCombo(combo, indexHit);
        if(completedPreviousCombo){
            previousCombo = combo;
        }
        else{
            previousCombo = null;
        }

        StartCoroutine(EnableCombo(animationClip.length * 0.3f, false));
        waitHit = StartCoroutine(WaitHit(animationClip.length - animationFade));
        timeOut = StartCoroutine(HitTimeOut(animationClip.length + actualHit.chainTime - animationFade, combo));
       
    }

    AnimationClip GetAnimation(Combo combo){
        var animationClip = combo.animations[indexHit];
        if(animationClip == null){
            animationClip = new AnimationClip();
            //animationClip.length = 0;
        }
        return animationClip;
    }

    void StopComboCoroutines(){
        if(timeOut != null){
            StopCoroutine(timeOut);
        }
        if(waitHit != null){
            StopCoroutine(waitHit);
        }
    }

    List<Combo> FilterCombos(List<Combo> combos, string key, int index){
        List<Combo> filteredCombos;
        filteredCombos = combos.Where( combo => HasKeyInComboAtIndex(key, combo, index)).ToList();
        //PrintList(filteredCombos);
        return filteredCombos;
    }

    bool HasKeyInComboAtIndex(string key, Combo combo, int index){
        var hit = GetHit(combo, index);
        if(hit == null) return false;
        return CompareKey(hit, key);
    }

    void PrintList(List<Combo> combos){
        #if UNITY_EDITOR
        ClearConsole();
        #endif
        foreach (var combo in combos)
        {
            Debug.Log(combo.name);
            PrintList(combo.hits);
            Debug.Log(".");
        }
    }
    void PrintList(List<Hit> hits){
        foreach (var hit in hits)
        {
            Debug.Log(hit.name);

        }
    }

    Hit GetHit(Combo combo, int index){
        Hit hit;
        try
        {
            hit = combo.hits[indexHit];
        }
        catch(System.Exception e)  // CS0168
        {
            //statusText.text = "Procurando combos disponiveis";
            return null;
        }
        return hit;
    }


    void EndCombo(Combo combo, bool complete = true)
    {
        complete = CheckCompleteCombo(combo, indexHit);
        indexHit = 0;
            animator.SetBool("Punch", false);
            animator.SetBool("Kick", false);
            animator.SetInteger("ChainNumber", indexHit);
        statusText.text = "End Combo: " + (complete ? "Completed": "Failed");
        filterCombos.Clear();
        StopComboCoroutines();

        StartCoroutine(EnableCombo(combo.recoverTime, false));
    }

    bool CheckCompleteCombo(Combo combo, int hits){
        if(indexHit >= combo.Count() || completedPreviousCombo){
            completedPreviousCombo = false;
            return true;
        }
        return false;
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
        animator.SetBool("Punch", false);
        animator.SetBool("Kick", false);
        statusText.text = "Wait";
        ComboBuilder("Wait");
    }

    IEnumerator HitTimeOut(float chainTime, Combo combo)
    {
        yield return new WaitForSeconds(chainTime);
        statusText.text = "Time Out";
        
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

    bool CompareKey(Hit h1, string key){
        return h1.key.CompareTo(key) == 0 ? true : false;
    }

    public void RemoveDuplicates(){
        Combos = new HashSet<Combo>(Combos).ToList();
    }
    
    
    #if UNITY_EDITOR
    static void ClearConsole(){
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
    
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
    
        clearMethod.Invoke(null, null);
    }
    #endif
}
