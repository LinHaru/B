using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position += player.transform.position - direction;
        direction = player.transform.position;
    }
}
