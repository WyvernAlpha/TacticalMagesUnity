///////////////////////////////////////////////////////////////////////////////////////
// Credit to: Emma Prats
// https://emmaprats.com/p/how-to-rotate-the-camera-around-an-object-in-unity3d/
///////////////////////////////////////////////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointRotator : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToTarget = 10;
    [SerializeField] [Range(0, 360)] private int maxRotationInOneSwipe = 180;

    private Vector3 previousPosition;

    private void Start()
    {
        cam.transform.position = target.position;
        cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * maxRotationInOneSwipe; // camera moves horizontally
            float rotationAroundXAxis = direction.y * maxRotationInOneSwipe; // camera moves vertically

            cam.transform.position = target.position;

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            previousPosition = newPosition;
        }
        
        //Zoom in on scroll up
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cam.fieldOfView--;
        }
        //Zoom out on scroll down
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cam.fieldOfView++;
        }
    }
}  

