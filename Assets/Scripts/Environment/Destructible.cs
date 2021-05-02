using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion;

    void OnTriggerEnter(Collider collider)
    {
        GameObject player = GameObject.Find("Player");
        PlayerScript playerScript = player.GetComponent<PlayerScript>();

        if (collider.gameObject.CompareTag("Player"))
        {
            
            if (playerScript.heavyMass)
            {
                print("I come in pieces!");
                Instantiate(destroyedVersion, transform.position, transform.rotation);
                Destroy(gameObject);
            }

        }
    }
}
