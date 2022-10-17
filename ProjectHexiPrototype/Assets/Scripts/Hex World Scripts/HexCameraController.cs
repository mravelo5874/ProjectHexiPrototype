using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCameraController : MonoBehaviour
{
    public Vector3 offset;
    public float smooth_time;

    public Transform target;
    public bool follow_target;

    
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (follow_target && target != null)
        {
            Vector3 target_pos = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, target_pos, ref velocity, smooth_time);
        }
    }
}
