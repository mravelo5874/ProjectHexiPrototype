using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static float CAMERA_FOCUS_SPEED = 0.2f;
    public static CameraController instance;
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        // reset camera
        ResetCamera();
    }

    // class used to store camera pos/rot/fov for focus entities
    [System.Serializable]
    public class CameraFocusModule
    {
        public Transform transform;
        public float fov;
    }

    public MyObject camera_object;

    [Header("Camera Focus Modules")]
    public CameraFocusModule default_module;
    public CameraFocusModule player_module;
    public CameraFocusModule enemy1_module;
    public CameraFocusModule enemy2_module;
    public CameraFocusModule enemy3_module;
    private CameraFocusModule GetModule(FocusEntity entity)
    {
        switch (entity)
        {
            default: 
            case FocusEntity.Default: return default_module;
            case FocusEntity.Player: return player_module;
            case FocusEntity.Enemy1: return enemy1_module;
            case FocusEntity.Enemy2: return enemy2_module;
            case FocusEntity.Enemy3: return enemy3_module;
        }
    }

    private FocusEntity current_focus_entity = FocusEntity.Default;

    public enum FocusEntity
    {
        Default,
        Player,
        Enemy1,
        Enemy2,
        Enemy3,
    }
    public void FocusCamera(FocusEntity focus_entity)
    {
        // return if camera already focused on entity
        if (current_focus_entity == focus_entity)
        {
            return;
        }
        current_focus_entity = focus_entity;
        // get correct module and focus camera
        CameraFocusModule module = GetModule(focus_entity);
        camera_object.MoveToTransform(module.transform, CAMERA_FOCUS_SPEED, true);
        camera_object.ChangeRotation(module.transform.rotation.eulerAngles, CAMERA_FOCUS_SPEED, true);
        camera_object.ChangeCameraFOV(module.fov, CAMERA_FOCUS_SPEED, true);
    }

    // default camera focus module
    public void ResetCamera(float delay = 0f)
    {
        // return if camera already focused on entity
        if (current_focus_entity == FocusEntity.Default)
        {
            return;
        }
        current_focus_entity = FocusEntity.Default;
        StartCoroutine(ResetCameraRoutine(delay));
    }
    private IEnumerator ResetCameraRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        // reset camera focus
        CameraFocusModule module = default_module;
        camera_object.MoveToTransform(module.transform, CAMERA_FOCUS_SPEED, true);
        camera_object.ChangeRotation(module.transform.rotation.eulerAngles, CAMERA_FOCUS_SPEED, true);
        camera_object.ChangeCameraFOV(module.fov, CAMERA_FOCUS_SPEED, true);
    }
}
