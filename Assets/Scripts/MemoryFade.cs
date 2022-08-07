using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFade : MonoBehaviour
{
    SpriteRenderer memoryRenderer;
    SpriteRenderer borderRenderer;
    Color clear = new Color(1f, 1f, 1f, 0f);
    Color opaque = new Color(1f, 1f, 1f, 1f);
    // Start is called before the first frame update
    void Awake()
    {
        memoryRenderer = gameObject.GetComponent<SpriteRenderer>();

        foreach (SpriteRenderer spr in gameObject.GetComponentsInChildren<SpriteRenderer>())
        { if (spr != memoryRenderer){ borderRenderer = spr; } }

        //print(borderRenderer);

    }

    void Start()
    {
        memoryRenderer.color = clear;
        borderRenderer.color = clear;
    }

    public void StartFade(bool fadeIn, float maxTime){  StartCoroutine(_FadeRoutine(fadeIn, maxTime));  }

    public IEnumerator _FadeRoutine(bool fadeIn, float maxTime)
    {
        float elapsed = 0;
        Color startingColor, endingColor, currentColor;

        switch (fadeIn)
        {
            case true:  startingColor = clear;
                        endingColor = opaque;
                        break;
            case false: startingColor = opaque;
                        endingColor = clear;
                        break;
        }

        while (elapsed < maxTime){

                currentColor = Color.Lerp(startingColor, endingColor, (elapsed/maxTime));
                memoryRenderer.color = currentColor;
                borderRenderer.color = currentColor;
                elapsed += Time.deltaTime;
                yield return null;
            }
    }
}
