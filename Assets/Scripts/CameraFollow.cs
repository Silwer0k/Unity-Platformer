using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector2 cameraPos2D;
    private Vector2 targetPos2D;
    private Vector3 zeroVector = Vector3.zero;

    public Transform target;
    [Range(0f, 200f)] public float speed = 40f;

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraPos2D = transform.position;
        targetPos2D = target.position;
        if(cameraPos2D != targetPos2D)
        {
            transform.position = Vector3.SmoothDamp(transform.position, 
                new Vector3(target.position.x, target.position.y, transform.position.z), ref zeroVector, Mathf.Pow(speed* Time.fixedDeltaTime, -1f));
        }
    }
}
