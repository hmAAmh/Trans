using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    [Header("=== PAINTING INDEX ===")]
    [SerializeField] int levelIndex;
    [Header("REFERENCES")]
    
    public GameObject photoPrefab;
    Camera renderCam;
    GameObject memoryParent, background;
    [Header("DIMENSIONS")]
    public float minX_percent = 3.2f;
    public float minY_percent = 8.5f;
    public float xSize_percent = 52.2f;
    public float ySize_percent = 61.2f;
    int photoWidth = 1366;
    int photoHeight = 768;
    

    private void Start()
    {
        renderCam = gameObject.GetComponent<Camera>();
    }

    public IEnumerator TakeScreenshotCoroutine()
    {
        yield return new WaitForEndOfFrame();
        _TakeScreenshot();
    }
    void _TakeScreenshot()
    {
        // Initialize the textures for the screenshot. We'll use these to make a new sprite in a sec.
        RenderTexture rt = new RenderTexture(photoWidth, photoHeight, 24);
        renderCam.targetTexture = rt;
        RenderTexture.active = rt;
        renderCam.Render();
        Texture2D screenShot = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);

        // Read the pixels from the screen and apply them to our texture.
        screenShot.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
        renderCam.targetTexture = null;
        screenShot.Apply();

        // Make a new object that we'll apply the screenshot to as a sprite.
        GameObject photoObject = GameObject.Instantiate(photoPrefab, new Vector3(-20, 0, 0), Quaternion.identity);    
        // And apply the sprite!                                    // This is so that we only use the part of the screen
                                                                    // with the easel and canvas.
        Sprite photoSprite = Sprite.Create(screenShot, new Rect(   minX_percent/100*photoWidth,
                                                                    minY_percent/100*photoHeight,
                                                                    xSize_percent/100*photoWidth,   
                                                                    ySize_percent/100*photoHeight), 
                                                                    // And this is the pivot lol
                                                                    new Vector2(0, 0));
        photoObject.GetComponent<SpriteRenderer>().sprite = photoSprite;
        photoObject.GetComponent<ScreenshotIndex>().paintingNumber = levelIndex;
        DontDestroyOnLoad(photoObject);

        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
    }

}
