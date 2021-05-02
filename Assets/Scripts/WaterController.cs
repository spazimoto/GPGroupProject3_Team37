using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    AudioSource audio;
    private Vector3 scaleChange;
    Vector3 initScale;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        audio.Pause();
    }

    void Update()
    {
        PlayerScript playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        if(playerScript.waterActive)
        {
            initScale = transform.localScale;
            scaleChange = Vector3.up;

            initScale.y += 10f * Time.deltaTime;

            transform.localScale = initScale;

            audio.UnPause();
        }
    }

}
