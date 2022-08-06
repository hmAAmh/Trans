using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class sceneManager_scr : MonoBehaviour
{
    public bool drawable = false;
    public bool fadeIn = true;
    public string nextScene;
    public string musc;
    private FMOD.Studio.EventInstance instance;
    public bool muscFadeOut = false;

    void Start(){
        instance = FMODUnity.RuntimeManager.CreateInstance(musc);
        instance.start();
    }

    void Update(){
        if(muscFadeOut){
            muscFadeOut = false;
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
