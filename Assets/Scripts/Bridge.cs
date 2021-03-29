using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public GameObject destroyedVersion;

    public GameObject player;

    void OnTriggerEnter(Collider collider)
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();

        if (collider.gameObject.CompareTag("Player"))
        {
            if(!playerScript.lightMass)
            {
                print("I come in pieces!");
                Instantiate(destroyedVersion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            
        }
    }
}
