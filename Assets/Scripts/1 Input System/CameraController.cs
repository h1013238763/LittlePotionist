using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : BaseControllerMono<CameraController>
{
    // Components
    public GameObject main_camera;

    // Varibles
    public int camera_mode;         // the mode of camera
    public UnityAction camera_func; // what camera need to do for each frame
    public GameObject target;       // the target object for follow mode
    
    void Awake()
    {
        Initial();
    }

    void Start()
    {
        SwitchCameraMode(0);
    }

    void Update()
    {
        camera_func.Invoke();
    }

    private void Initial()
    {
        main_camera = GameObject.Find("Main Camera");
        if(main_camera == null)
            main_camera = new GameObject("Main Camera");
    }

    public void SwitchCameraMode(int id)
    {
        switch (id)
        {
            case 0: // follow the player
                target = PlayerController.GetController().player;
                camera_func = Follow;
                break;
            default:
                break;
        }
    }

    private void Follow()
    {
        float x, y;
        x = target.transform.position.x;
        y = target.transform.position.y;
        main_camera.transform.position = new Vector3(x, y, -1);
    }
}
