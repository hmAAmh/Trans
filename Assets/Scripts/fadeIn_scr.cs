using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fadeIn_scr : MonoBehaviour
{
    float initRate;
    float initChange;
    public bool fadeIn;
    public bool active;
    Color tmp;
    sceneManager_scr managerScr;

    void Start()
    {
        managerScr = GameObject.Find("sceneManager").GetComponent<sceneManager_scr>();

        initRate = 0.0001f;
        initChange = initRate / 32;
        tmp = gameObject.GetComponent<SpriteRenderer>().color;

        if(fadeIn){
            tmp.a = 1f;
        }
        else{
            tmp.a = 0f;
            initRate *= -1f;
            initChange *= -1f;
        }
        gameObject.GetComponent<SpriteRenderer>().color = tmp;
    }

    void Update()
    {
        if(active){
            tmp = gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a -= initRate;
            gameObject.GetComponent<SpriteRenderer>().color = tmp;
            initRate += initChange;

            if(tmp.a < 0f || tmp.a > 3f){
                managerScr.fadeIn = false;
                if(!fadeIn){
                    SceneManager.LoadScene (sceneName:managerScr.nextScene);
                }
                else{
                    Destroy(gameObject);
                }
            }
        }
    }
}
