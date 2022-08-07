using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fadeIn_scr : MonoBehaviour
{
    float targetAlpha;
    public bool fadeIn, active;
    private bool muscFadeOut = false;
    [SerializeField] bool original = false; // Mark this as true in the inspector to signal that this object is the
                                            // fade present at the start of every scene
    [SerializeField] dialogue dialogue;
    
    Color tmp;
    //sceneManager_scr managerScr;

    void Start(){
        //managerScr = GameObject.FindWithTag("sceneManager").GetComponent<sceneManager_scr>();
        //if (managerScr == null){print("could not find managerScr");}

        if(active){ BeginFade(); }
    }

    void InitializeColor()
    {
        tmp = gameObject.GetComponent<SpriteRenderer>().color;

        if(fadeIn){
            tmp.a = 1f;
            targetAlpha = 0f;
            // If we're fading in from black, we start at full alpha, and a target alpha of 0f.
        }
        else{
            //print("fading out to black");
            tmp.a = 0f;
            targetAlpha = 1f;
            // If we're fading out to black, we start at 0 alpha, and a target alpha of 1f.
        }
        gameObject.GetComponent<SpriteRenderer>().color = tmp;
    }

    public void BeginFade()
    {
        InitializeColor();

        if(!fadeIn && !muscFadeOut){
                muscFadeOut = true;
                GameObject.FindWithTag("sceneManager").GetComponent<sceneManager_scr>().muscFadeOut = true;
            }

        StartCoroutine(_FadeRoutine());
    }

    IEnumerator _FadeRoutine()
    {
        
        float elapsed = 0;
        Color starting = gameObject.GetComponent<SpriteRenderer>().color;
        Color inprogress = starting;

        while (elapsed < 3f){

                inprogress.a = Mathf.Lerp(starting.a, targetAlpha, (elapsed/3f));
                //print(inprogress.a);
                gameObject.GetComponent<SpriteRenderer>().color = inprogress;
                elapsed += Time.deltaTime;
                yield return null;
            }

        GameObject.FindWithTag("sceneManager").GetComponent<sceneManager_scr>().fadeIn = false;
            if(!fadeIn){
                SceneManager.LoadScene (sceneName:
                                        GameObject.FindWithTag("sceneManager").
                                        GetComponent<sceneManager_scr>().nextScene);
                }
            else{
                if (original){ dialogue.TweenAppear(true); }
                Destroy(gameObject);
                }
    }

    /*void Update(){
        if(active){
            if(!fadeIn && !muscFadeOut){
                muscFadeOut = true;
                managerScr.muscFadeOut = true;
            }

            tmp = gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a -= initRate;
            gameObject.GetComponent<SpriteRenderer>().color = tmp;
            initRate += initChange;

            if(tmp.a < 0f || tmp.a > 3f){
                
            }
        }
    }*/
}
