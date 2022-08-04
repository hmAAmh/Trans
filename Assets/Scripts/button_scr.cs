using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button_scr : MonoBehaviour
{
    sceneManager_scr managerScr;
    // Start is called before the first frame update
    void Start(){
        managerScr = GameObject.Find("sceneManager").GetComponent<sceneManager_scr>();
    }

    // Update is called once per frame
    void TaskOnClick(){
        if(managerScr.drawable){
            
        }
    }
}
