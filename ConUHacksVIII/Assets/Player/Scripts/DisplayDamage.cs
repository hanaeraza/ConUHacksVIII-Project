using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDamage : MonoBehaviour
{
    [SerializeField] Canvas impactCanvas;
    [SerializeField] float impactTime = 0.3f;
    [SerializeField] float fadeTime = 0.1f;
    [SerializeField] float impactTimer = 0f;
    [SerializeField] float fadeTimer = 0f;
    Image image;
    Color baseColor;
    // Start is called before the first frame update
    void Start()
    {
        impactCanvas.enabled = false;
        image = impactCanvas.GetComponentInChildren<Image>();
        baseColor = new Color(image.color.r, image.color.g, image.color.b);

        impactTimer = 0;
        fadeTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (impactTimer > 0) {
            impactTimer -= Time.deltaTime;
        }
        else if (fadeTimer > 0) {
            fadeTimer -= Time.deltaTime;
            if (fadeTimer > 0) {
                baseColor.a = (fadeTimer / fadeTime);
                image.color = baseColor;
            }
        }
        else {
            impactCanvas.enabled = false;
        }
    }

    public void ShowDamageImpact() {
        impactTimer = impactTime;
        fadeTimer = fadeTime;
        impactCanvas.enabled = true;
        baseColor.a = 1f;
        image.color = baseColor;
    }
}
