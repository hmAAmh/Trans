using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public Camera renderCam;
    public GameObject photoToHud;
    [Header("DIMENSIONS")]
    int photoWidth;
    int photoHeight;
    public float minX_percent;
    public float minY_percent;
    public float xSize_percent;
    public float ySize_percent;

    private void Start()
    {
        photoWidth = 1366;
        photoHeight = 768;

        print(photoWidth);
        print(photoHeight);
    }

    public void LateUpdate()
    {
        if (Input.GetKeyDown("space"))
        {
            RenderTexture rt = new RenderTexture(photoWidth, photoHeight, 24);
            renderCam.targetTexture = rt;
            RenderTexture.active = rt;
            renderCam.Render();
            Texture2D screenShot = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
            renderCam.targetTexture = null;
            screenShot.Apply();
            GameObject flyToHud = GameObject.Instantiate(photoToHud, new Vector3(-20, 0, 0), Quaternion.identity);    
            flyToHud.GetComponent<SpriteRenderer>().sprite = Sprite.Create(screenShot, new Rect(minX_percent/100*photoWidth,
                                                                                                minY_percent/100*photoHeight,
                                                                                                xSize_percent/100*photoWidth,
                                                                                                ySize_percent/100*photoHeight), new Vector2(0, 0));

            Camera.main.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);

            //StartCoroutine(DestroyAfterDelay(flyToHud));
        }
    }

    IEnumerator DestroyAfterDelay(GameObject Us)
    {
        yield return new WaitForSeconds(2);
        Destroy(Us);

    }

}
