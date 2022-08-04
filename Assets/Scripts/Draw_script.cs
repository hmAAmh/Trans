using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_script : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;
    public Material Material1;
    public Material Material2;
    public Material materialUsed;

    int curLayer;

    LineRenderer currentLineRenderer;

    Vector2 lastPos;

    GameObject manager;
    sceneManager_scr managerScr;
    
    void Start(){
        manager = GameObject.Find("sceneManager");
        managerScr = manager.GetComponent<sceneManager_scr>();
        materialUsed = Material1;
        curLayer = -9998;
    }

    private void Update(){
        if(managerScr.drawable){
            Draw();
        }
    }
    
    void Draw(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            CreateBrush();
        }
        if(Input.GetKey(KeyCode.Mouse0)){
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            if(mousePos != lastPos){
                AddAPoint(mousePos);
                lastPos = mousePos;
            }
        }
        else{
            currentLineRenderer = null;
        }
    }

    void CreateBrush(){
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit)
            {
                if (hit.transform.tag == "paintBlob"){
                    materialUsed = hit.transform.GetComponent<SpriteRenderer>().material;
                }
            }
        }

        currentLineRenderer.GetComponent<LineRenderer>().material = materialUsed;
        currentLineRenderer.sortingOrder  = curLayer;
        curLayer++;

        
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void AddAPoint(Vector2 pointPos){
        currentLineRenderer.positionCount++;

        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }
}
