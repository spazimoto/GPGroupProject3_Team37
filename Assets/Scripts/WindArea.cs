using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public float baseStrength;
    private float modifiedStrength;

    [HideInInspector]public float strength; //DO NOT MODIFY

    public Vector3 direction;

    public GameObject player;
    public PlayerScript playerScript;

    void Start ()
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
    }

    void Update()
    {
        strength = baseStrength + modifiedStrength;

        if (playerScript.lightMass)
        {
            modifiedStrength = 5f; 
        }
        if (playerScript.normalMass)
        {
            modifiedStrength = 1f;
        }
        if (playerScript.heavyMass)
        {
            modifiedStrength = -2f;
        }
    }

}
