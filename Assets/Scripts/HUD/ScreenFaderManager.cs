using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(Image))]
public class ScreenFaderManager: MonoBehaviour {

    public static ScreenFaderManager instance;

    private Image screenFaderImage;
    private Animator anim;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    //-> Get components
    void Start () {
        anim = GetComponent<Animator>();
        screenFaderImage = GetComponent<Image>();
	}
	
    //-> Remove Screen fader
	public bool ScreenFadeIn()
    {
        anim.SetTrigger("FadeIn");
        Debug.Log("FadeIn");

        if (screenFaderImage.color.a == 0)
            return true;
        else
            return false;
    }

    //-> Show Screen Fader
    public void ScreenFadeOut()
    {
        anim.SetTrigger("FadeOut");
        Debug.Log("FadeOut");
    }

    //-> Deprecated

    //public void ScreenIn()
    //{
        //anim.SetTrigger("In");
        //Debug.Log("In");
    //}
    public void ScreenOut()
    {
        anim.SetTrigger("Out");
        Debug.Log("Out");
    }
}
