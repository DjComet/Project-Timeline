using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClampMovement : MonoBehaviour{
    //JA
    private GameObject player;
    private Inputs inputs;
    public float distanceFromCenterOfPlayer = 0.81f;

    public float sensitivityX = 15.0f;
    public float sensitivityY = 15.0f;

    public float minimumX = -360.0f;
    public float maximumX = 360.0f;

    public float minimumY = -60.0f;
    public float maximumY = 60.0f;

    private float xAngle = 0.0f;
    private float yAngle = 0.0f;

   
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
        originalRotation = transform.localRotation;
        playerOriginalRotation = player.transform.localRotation;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + player.transform.up * distanceFromCenterOfPlayer;
        //Gets rotational input from the mouse
        xAngle += Input.GetAxis("MouseX") * sensitivityX;
        yAngle += Input.GetAxis("MouseY") * sensitivityY;

        //Clamp rotation angles
        xAngle = ClampAngle(xAngle, minimumX, maximumX);
        yAngle = ClampAngle(yAngle, minimumY, maximumY);

        //Create rotations around axis
        Quaternion leftQuaternion = Quaternion.AngleAxis(yAngle, Vector3.left);
        Quaternion upQuaternion = Quaternion.AngleAxis(xAngle, Vector3.up);

        //Rotate
        transform.localRotation = originalRotation * upQuaternion * leftQuaternion;
        player.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
    }

    public static float ClampAngle(float angle, float min, float max)
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
    }
}
