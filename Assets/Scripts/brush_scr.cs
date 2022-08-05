using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brush_scr : MonoBehaviour
{
    GameObject cursorHead;
    GameObject draw;

    void Start(){
        cursorHead = GameObject.Find("cursorHead");
        draw = GameObject.Find("Draw");
    }


    void Update(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        cursorHead.transform.GetComponent<SpriteRenderer>().color = draw.transform.GetComponent<Draw_script>().currentColor;
    }
}
