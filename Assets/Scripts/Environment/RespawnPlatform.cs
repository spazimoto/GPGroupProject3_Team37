using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    public GameObject platform;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    void Update()
    {
        StartCoroutine("Respawn");
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
        print("I've been destroyed!");
        Instantiate(platform, startPosition, startRotation);
        Destroy(gameObject);
    }
}
