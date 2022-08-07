using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class button_scr : MonoBehaviour
{
    bool clicked = false;
    sceneManager_scr managerScr;
    public GameObject blackScreen;
    // Start is called before the first frame update
    void Start(){
        managerScr = GameObject.Find("sceneManager").GetComponent<sceneManager_scr>();
    }

    // Update is called once per frame
    public void TaskOnClick(){
        if(!clicked){
            Debug.Log("test!");
            clicked = true;
            RuntimeManager.CreateInstance("event:/sceneEnd").start();
            GameObject fadeOut = Instantiate(blackScreen);
            print("activated from button");
            fadeOut.GetComponent<fadeIn_scr>().fadeIn = false;
            fadeOut.GetComponent<fadeIn_scr>().BeginFade();
            fadeOut.transform.position = new Vector3(0f, 0f, 0f);
        }
    }
}
