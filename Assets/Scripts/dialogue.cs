using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;

[System.Serializable]
public class dialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] sentences;
    //public string sentence;
    Text textComp;
    int sentenceLength, sentenceIndex, wordsDisplayed, frameCounter, frameTextRate;
    GameObject manager;
    sceneManager_scr managerScr;

    public bool Activated;
    bool final;

    void Start(){
        textComp = gameObject.GetComponent<Text>();
        manager = GameObject.Find("sceneManager");
        managerScr = manager.GetComponent<sceneManager_scr>();
        
        sentenceIndex = 0;
        wordsDisplayed = 0;
        frameCounter = 0;
        frameTextRate = 8;
        final = false;

        sentenceLength = sentences.Length;
        textComp.text = sentences[sentenceIndex].Substring(0, wordsDisplayed);

        
    }

    void Update(){
        if(Activated){
            frameCounter++;

            if(frameCounter % frameTextRate == 0){
                wordsDisplayed = Mathf.Min(wordsDisplayed + 1, sentences[sentenceIndex].Length);
                textComp.text = sentences[sentenceIndex].Substring(0, wordsDisplayed);
            }
            
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                if(textComp.text != sentences[sentenceIndex]){
                    textComp.text = sentences[sentenceIndex];
                    wordsDisplayed = sentences[sentenceIndex].Length;
                }
                else{
                    sentenceIndex++;
                    wordsDisplayed = 0;
                    if(sentenceIndex >= sentenceLength){
                        managerScr.drawable = true;
                        Destroy(transform.parent.gameObject);
                        Destroy(gameObject);
                        if(final){
                            SceneManager.LoadScene (sceneName:"JermaDumpy");
                        }
                    }
                    else{  
                        
                        textComp.text = sentences[sentenceIndex].Substring(0, wordsDisplayed);
                    }  
                }
            }
        }
    }

    public void setActive(){
        Activated = true;
        transform.parent.position = new Vector3(500f, transform.parent.position.y, transform.parent.position.z);
        final = true;
    }
}
