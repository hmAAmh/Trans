using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class dialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] sentences;
    //public string sentence;
    Text textComp;
    int sentenceLength, sentenceIndex;

    void Start(){
        textComp = gameObject.GetComponent<Text>();
        
        sentenceLength = sentences.Length;
        sentenceIndex = 0;
        textComp.text = sentences[sentenceIndex];
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            sentenceIndex++;
            if(sentenceIndex >= sentenceLength){
                textComp.text = " ";
            }
            else{
                textComp.text = sentences[sentenceIndex];
            }
        }
    }
}
