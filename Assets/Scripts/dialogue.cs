using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

[System.Serializable]
public class dialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] sentences;
    //public string sentence;
    Text textComp;
    int sentenceLength, sentenceIndex, wordsDisplayed, frameCounter, frameTextRate;
    private Vector3 scaleChangePos, scaleChangeNeg, scaleDefault, scaleBlowup, scaleMin;
    float scaleAmount, scaleLeadIn, scaleLeadInInit, scaleLeadInAmount;
    sceneManager_scr managerScr;
    public GameObject blackScreen;

    public bool Activated;
    bool final, tweenedBlowup, tweenedStart, tweenedEnd, tweenActivate;

    void Start(){
        textComp = gameObject.GetComponent<Text>();
        managerScr = GameObject.Find("sceneManager").GetComponent<sceneManager_scr>();
        
        transform.parent.localScale = new Vector3(0f, 0f, -5f);
        sentenceIndex = 0;
        wordsDisplayed = 0;
        frameCounter = 0;
        frameTextRate = 24;
        final = false;

        tweenedBlowup = true;
        tweenedStart = false;
        tweenedEnd = true;
        tweenActivate = true;

        sentenceLength = sentences.Length;
        textComp.text = sentences[sentenceIndex].Substring(0, wordsDisplayed);

        scaleAmount = 0.00005f / 2f;
        scaleLeadInInit = 0.00002f;
        scaleLeadIn = scaleLeadInInit;
        scaleLeadInAmount = scaleLeadIn / 40;
        scaleDefault = new Vector3(0.01f, 0.01f, -5f);
        scaleMin = new Vector3(0.01f / 100, 0.01f / 100, -5f);
        scaleBlowup = new Vector3(0.011f, 0.011f, -5f);
        
        scaleChangeNeg = new Vector3(-scaleAmount / 4, -scaleAmount / 4, 0f);
    }

    void Update(){
        if(Activated && !(managerScr.fadeIn)){
            frameCounter++;

            tweening();

            if(tweenedStart && tweenedEnd){
                if(frameCounter % frameTextRate == 0){
                    wordsDisplayed = Mathf.Min(wordsDisplayed + 1, sentences[sentenceIndex].Length);
                    textComp.text = sentences[sentenceIndex].Substring(0, wordsDisplayed);

                    if(wordsDisplayed < sentences[sentenceIndex].Length && frameCounter % (frameTextRate * 3) == 0){
                        RuntimeManager.CreateInstance("event:/charAppear").start();
                    }
                }
                click();
            }

        }
    }

    public void setActive(){
        float pos = 3.76001f;
        Activated = true;
        transform.parent.position = new Vector3(pos, transform.parent.position.y, transform.parent.position.z);
        final = true;
        managerScr.drawable = false;
        RuntimeManager.CreateInstance("event:/paperFlip").start();
    }

    void click(){
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                if(textComp.text != sentences[sentenceIndex]){
                    textComp.text = sentences[sentenceIndex];
                    wordsDisplayed = sentences[sentenceIndex].Length;
                }
                else{
                    sentenceIndex++;
                    wordsDisplayed = 0;
                    if(sentenceIndex >= sentenceLength){
                        tweenedBlowup = true;
                        tweenedEnd = false;
                        scaleLeadIn = scaleLeadInInit;
                        RuntimeManager.CreateInstance("event:/textBoxAppear").start();
                    }
                    else{       
                        textComp.text = sentences[sentenceIndex].Substring(0, wordsDisplayed);
                        RuntimeManager.CreateInstance("event:/paperFlip").start();
                    }  
                }
            }
    }

    void tweening(){
        if(!tweenedStart){
            if(tweenedBlowup){
                if(tweenActivate){
                    RuntimeManager.CreateInstance("event:/textBoxAppear").start();
                }
                tweenActivate = false;
                if(transform.parent.localScale.x < scaleBlowup.x){
                    scaleChangePos = new Vector3(scaleLeadIn, scaleLeadIn, 0f);
                    transform.parent.localScale += scaleChangePos;
                    scaleLeadIn += scaleLeadInAmount;
                }
                else{
                    tweenedBlowup = false;
                }
            }
            else{
                if(transform.parent.localScale.x > scaleDefault.x){
                    transform.parent.localScale += scaleChangeNeg;
                }
                else{
                    tweenedStart = true;
                    transform.parent.localScale = scaleDefault;
                }
            }
        }
        if(!tweenedEnd){
            if(tweenedBlowup){
                if(transform.parent.localScale.x < scaleBlowup.x){
                    transform.parent.localScale -= scaleChangeNeg;
                }
                else{
                    tweenedBlowup = false;
                }
            }
            else{
                if(transform.parent.localScale.x > scaleMin.x){
                    scaleChangePos = new Vector3(scaleLeadIn, scaleLeadIn, 0f);
                    transform.parent.localScale -= scaleChangePos;
                    scaleLeadIn += scaleLeadInAmount;
                }
                else{
                    
                    Destroy(transform.parent.gameObject);
                    Destroy(gameObject);
                    if(final){
                        
                        RuntimeManager.CreateInstance("event:/sceneEnd").start();
                        GameObject fadeOut = Instantiate(blackScreen);
                        fadeOut.GetComponent<fadeIn_scr>().fadeIn = false;
                        fadeOut.GetComponent<fadeIn_scr>().active = true;
                        fadeOut.transform.position = new Vector3(0f, 0f, 0f);
                    }
                    else{
                        managerScr.drawable = true;
                    }
                }
            }
        }
    }
}
