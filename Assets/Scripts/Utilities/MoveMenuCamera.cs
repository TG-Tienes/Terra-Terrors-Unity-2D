using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMenuCamera : MonoBehaviour
{
    float start = 0;
    public float camSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        start += Time.deltaTime * camSpeed;
        transform.position = new Vector3(start, transform.position.y, transform.position.z);
    }
}
