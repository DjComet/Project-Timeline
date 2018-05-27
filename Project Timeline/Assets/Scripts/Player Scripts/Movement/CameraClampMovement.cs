using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClampMovement : MonoBehaviour{
    //JA
    [ExecuteInEditMode]
    private GameObject player;
    private Inputs inputs;
    public float distanceFromCenterOfPlayer = 0.81f;

    public bool invertYaw = false;
    public bool invertPitch = false;
    int invYaw = 1;
    int invPitch = 1;

    [Range(0,15)]public float sensitivityYaw = 15.0f;
    [Range(0, 15)] public float sensitivityPitch = 15.0f;

    public float minimumX = -360.0f;
    public float maximumX = 360.0f;

    public float minimumY = -80.0f;
    public float maximumY = 80.0f;

    public float yaw = 0.0f;
    public float pitch = 0.0f;

    
    Quaternion originalRotation;
    Quaternion playerOriginalRotation;


    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (rb)
        {
            rb.freezeRotation = true;
        }
        originalRotation = transform.rotation;
        playerOriginalRotation = player.transform.rotation;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        invPitch = (invertPitch ? 1 : -1);
        invYaw = (invertYaw ? -1 : 1);
    }

    void LateUpdate()
    { 
        if(!PauseMenu.GameIsPaused)
        {
            yaw = transform.localEulerAngles.y;
            //pitch = transform.eulerAngles.x; //Pasan cosas to raras con esto. Al llegar a x=0, x retorna a 80 automaticamente.


            transform.position = player.transform.position + player.transform.up * distanceFromCenterOfPlayer;
            //Gets rotational input from the mouse
            yaw += Input.GetAxis("MouseX") * sensitivityYaw * invYaw;
            pitch += Input.GetAxis("MouseY") * sensitivityPitch * invPitch;

            //Clamp rotation angles
            pitch = Mathf.Clamp(pitch, minimumY, maximumY);
            //yaw = ClampAngle(yaw, minimumX, maximumX);
            //pitch = ClampAngle(pitch, minimumY, maximumY);

            //Create rotations around axis
            //Quaternion leftQuaternion = Quaternion.AngleAxis(pitch, Vector3.left);
            //Quaternion upQuaternion = Quaternion.AngleAxis(yaw, Vector3.up);

            //Rotate
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
            player.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        }

    }

    /*public static float ClampAngle(float angle, float min, float max)//Works for quaternions
    {
        angle = angle % 360;
        if ((angle >= -360.0f) && (angle <= 360.0f))
        {
            if (angle < -360.0f)
            {
                angle += 360.0f;
            }
            if (angle > 360.0f)
            {
                angle -= 360.0f;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }*/
}
