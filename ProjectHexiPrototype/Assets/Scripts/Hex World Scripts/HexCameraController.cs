using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCameraController : MonoBehaviour
{
    public static HexCameraController instance;
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        SetDefaultFocus();
    }

    public MyObject my_object;

    public Vector3 default_offset;
    public Vector3 inside_hex_cell_offset;
    public float default_rotation;
    public float inside_hex_cell_rotation;
    public float smooth_time;

    public Transform target;
    public bool follow_target;

    // private vars
    private Vector3 current_offset;
    private Vector3 velocity = Vector3.zero;

    

    void FixedUpdate()
    {
        if (follow_target && target != null)
        {
            Vector3 target_pos = target.position + current_offset;
            transform.position = Vector3.SmoothDamp(transform.position, target_pos, ref velocity, smooth_time);
        }
    }

    public void SetFollowTarget(Transform new_target)
    {
        target = new_target;
    }

    public void SetDefaultFocus()
    {
        // set default offset + rotation
        current_offset = default_offset;
        my_object.ChangeRotation(new Vector3(default_rotation, 0f, 0f), smooth_time, true);
    }

    public void SetInsideHexCellFocus()
    {
        // set inside hex cell offset + rotation
        current_offset = inside_hex_cell_offset;
        my_object.ChangeRotation(new Vector3(inside_hex_cell_rotation, 0f, 0f), smooth_time, true);
    }
}
