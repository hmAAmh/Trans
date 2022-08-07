using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

[System.Serializable]
public class dialogue_ARCHIVE : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] sentences;
    //public string sentence;
    Text textComp;
    int sentenceLength, sentenceIndex, lettersDisplayed, frameCounter, frameTextRate;
    private Vector3 scaleDefault, scaleBlowup, scaleMin;
    sceneManager_scr managerScr;
    public GameObject blackScreen;
    Screenshot screenshot;
    MemoryFade memoryFade;

    public bool Activated;
    bool final, tweenedStart, tweenedEnd;
    bool currentlyTweening = false;

    void Start(){
        textComp = gameObject.GetComponent<Text>();
        managerScr = GameObject.Find("sceneManager").GetComponent<sceneManager_scr>();
        screenshot = GameObject.FindWithTag("MainCamera").GetComponent<Screenshot>();
        memoryFade = GameObject.FindWithTag("Memory").GetComponent<MemoryFade>();
        
        transform.parent.localScale = new Vector3(0f, 0f, -5f);
        sentenceIndex = 0;
        lettersDisplayed = 0;
        frameCounter = 0;
        frameTextRate = 24;
        final = false;

        tweenedStart = false;
        tweenedEnd = true;

        sentenceLength = sentences.Length;
        textComp.text = sentences[sentenceIndex].Substring(0, lettersDisplayed);

        scaleDefault = new Vector3(0.01f, 0.01f, -5f);
        scaleMin = new Vector3(0.01f / 100, 0.01f / 100, -5f);
        scaleBlowup = new Vector3(0.011f, 0.011f, -5f);

    }

    void Update(){
        if(Activated && !(managerScr.fadeIn)){
            frameCounter++;

            if(tweenedStart && tweenedEnd){
                if(frameCounter % frameTextRate == 0){
                    lettersDisplayed = Mathf.Min(lettersDisplayed + 1, sentences[sentenceIndex].Length);
                    textComp.text = sentences[sentenceIndex].Substring(0, lettersDisplayed);

                    if(lettersDisplayed < sentences[sentenceIndex].Length && frameCounter % (frameTextRate * 3) == 0){
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
        TweenAppear(true);
        transform.parent.position = new Vector3(pos, transform.parent.position.y, transform.parent.position.z);
        final = true;
        managerScr.drawable = false;
        RuntimeManager.CreateInstance("event:/paperFlip").start();

        // Take a screenshot of the finished painting, and store it as a sprite.
        StartCoroutine(screenshot.TakeScreenshotCoroutine());
        memoryFade.StartFade(false, 1f);


    }

    void click(){
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                if(textComp.text != sentences[sentenceIndex]){
                    textComp.text = sentences[sentenceIndex];
                    lettersDisplayed = sentences[sentenceIndex].Length;
                }
                else{
                    sentenceIndex++;
                    lettersDisplayed = 0;
                    if(sentenceIndex >= sentenceLength){
                        tweenedEnd = false;
                        TweenAppear(false);
                        RuntimeManager.CreateInstance("event:/textBoxAppear").start();
                    }
                    else{       
                        textComp.text = sentences[sentenceIndex].Substring(0, lettersDisplayed);
                        RuntimeManager.CreateInstance("event:/paperFlip").start();
                    }  
                }
            }
    }

    public void TweenAppear(bool appear)
    {
        if (appear)
        {
            StartCoroutine(_TweenLocalScale(scaleMin, scaleBlowup, 0.25f, true));
            StartCoroutine(_TweenLocalScale(scaleBlowup, scaleDefault, 0.4f));
        }
        else
        {
            StartCoroutine(_TweenLocalScale(scaleDefault, scaleBlowup, 0.25f));
            StartCoroutine(_TweenLocalScale(scaleBlowup, scaleMin, 0.4f));    
        }
    }
    IEnumerator _TweenLocalScale(Vector3 startingScale, Vector3 endingScale, float maxTime,
                                bool startTweenedStart=false ) // Whether or not we mark tweenedStart as true after lerping.
    {

        while (currentlyTweening == true){ yield return null; }

        currentlyTweening = true;
        float elapsed = 0;
        Vector3 currentFactor = Vector3.one;
        while (elapsed < maxTime){

                currentFactor = Vector3.Lerp(startingScale, endingScale, (elapsed/maxTime));
                transform.parent.localScale = currentFactor;
                elapsed += Time.deltaTime;
                yield return null;
            }
        currentlyTweening = false;
        if(startTweenedStart){ tweenedStart = true; }

        if (endingScale == scaleMin) // IE: If we have shrunk down to nothing.
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
            if(final){
                
                RuntimeManager.CreateInstance("event:/sceneEnd").start();
                GameObject fadeOut = Instantiate(blackScreen);
                //print("activated from dialogue");
                fadeOut.GetComponent<fadeIn_scr>().fadeIn = false;
                fadeOut.GetComponent<fadeIn_scr>().BeginFade();
                fadeOut.transform.position = new Vector3(0f, 0f, 0f);
            }
            else{
                managerScr.drawable = true;
                memoryFade.StartFade(true, 2f);
            }
        }
    }
}
