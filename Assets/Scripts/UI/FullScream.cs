using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScream : MonoBehaviour
{   
    public Texture2D FullScreenSprite;
    public Texture2D WindowedScreenSprite;

    void Update()
    {
        if(Screen.fullScreen){
            GetComponent<RawImage>().texture = this.FullScreenSprite;
        } else {
            GetComponent<RawImage>().texture = this.WindowedScreenSprite;
        }
    }
}
