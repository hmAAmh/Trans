using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotPlacer : MonoBehaviour
{
    GameObject[] paintingArray;
    public GameObject jermobject;
    // Start is called before the first frame update
    void Start()
    {
        paintingArray = GameObject.FindGameObjectsWithTag("PaintingScreenshot");
        PlacePaintings();
    }

    void OnDestroy()
    {
        foreach (GameObject painting in paintingArray)
        { Destroy(painting); }
    }

    void PlacePaintings()
    {
        foreach (GameObject painting in paintingArray)
        {
            switch (painting.GetComponent<ScreenshotIndex>().paintingNumber)
            {
                case 1:
                    painting.transform.position = new Vector3(-5.92f,-0.65f,1f);
                    painting.transform.localScale = new Vector3(0.432f,0.432f,1f);
                    break;
                case 2:
                    painting.transform.position = new Vector3(-2.525f,-1.856f,1f);
                    painting.transform.localScale = new Vector3(0.432f,0.432f,1f);
                    break;
                case 3:
                    painting.transform.position = new Vector3(-5.91f,-3.03f,1f);
                    painting.transform.localScale = new Vector3(0.432f,0.432f,1f);
                    break;
            }
        }
    }

    void SpawnJerma(Vector3 position)
    {
        Instantiate(jermobject, position, Quaternion.identity);
    }
}
