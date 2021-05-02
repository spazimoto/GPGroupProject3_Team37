using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject lightParticles;
    public GameObject heavyParticles;
    void Update()
    {
        if(Input.GetKey(KeyCode.Q) || Input.GetButton("Light"))
        {
            lightParticles.gameObject.SetActive(true);
            heavyParticles.gameObject.SetActive(false);
        }

        else if(Input.GetKey(KeyCode.E) || Input.GetButton("Heavy"))
        {
            lightParticles.gameObject.SetActive(false);
            heavyParticles.gameObject.SetActive(true);
        }

        else
        {

            lightParticles.gameObject.SetActive(false);
            heavyParticles.gameObject.SetActive(false);
        }
    }
}
