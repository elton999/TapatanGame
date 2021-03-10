using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScream : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D FullScreenSprite;
    public Texture2D WindowedScreenSprite;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.fullScreen){
            GetComponent<RawImage>().texture = this.FullScreenSprite;
        } else {
            GetComponent<RawImage>().texture = this.WindowedScreenSprite;
        }
    }
}
