using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

    private Image _image;
    public float _fadeSpeed = 0.05f;

    void Start()
    {
        _image = GetComponent<Image>();
        StartCoroutine(Fadein(_image, _fadeSpeed));
    }

    void Update()
    {
        // Update is empty for this purpose.
    }

    // You can use it for multiple images, just pass that image.
    IEnumerator Fadein(Image img, float speed)
    {
        // Will run only until image's alpha becomes completely 100, will stop after that.
        while (img.color.a < 100)
        {

            // You can replace WaitForEndOfFrame with WaitForSeconds.
            yield return new WaitForEndOfFrame();
            Color colorWithNewAlpha = img.color;
            colorWithNewAlpha.a += speed;
            img.color = colorWithNewAlpha;
        }
    }
}
