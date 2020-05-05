using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LaserPointer_scene8 : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Behaviour_Pose controllerPose; // 2
    public SteamVR_Action_Boolean nextLineAction; // 3

    public LayerMask UIMask;

    private bool isHover = true;
    private bool isDown = false;

    // Laser Pointer Variables
    public GameObject laserePrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    public Color laserColorDefault;
    public Color laserColorHover;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn new laser and save a reference 'laser'
        laser = Instantiate(laserePrefab);
        laserTransform = laser.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100, UIMask))
        {
            //print("detected");
            hitPoint = hit.point;
            ShowLaser(hit);

            GameObject button = hit.collider.gameObject;
            if (button.name.Contains("Replay"))
            {
                laser.GetComponent<Renderer>().material.color = laserColorHover;

                if (nextLineAction.GetLastStateDown(handType))
                {
                    button.GetComponent<ButtonActionWrapper>().Replay();
                    print("Replay");
                }
            }
            else if (hit.collider.gameObject.name.Contains("Quit"))
            {
                laser.GetComponent<Renderer>().material.color = laserColorHover;
                
                if (nextLineAction.GetLastStateDown(handType))
                {
                    button.GetComponent<ButtonActionWrapper>().Quit();
                }
                
            }
            else
            {
                laser.GetComponent<Renderer>().material.color = laserColorDefault;
            }

            /*
            if (nextLineAction.GetLastStateDown(handType))
            {
                //Transform parentButton = hit.collider.gameObject.transform.parent;  
                GameObject button = hit.collider.gameObject;
                if (button.name.Contains("Replay"))
                {
                    button.GetComponent<ButtonActionWrapper>().Replay();
                } else if (hit.collider.gameObject.name.Contains("Quit")) {
                    button.GetComponent<ButtonActionWrapper>().Quit();
                }

            }
            */
        }
        else
        {
            if (laser.activeSelf)
            {
                laser.SetActive(false);
            } 
                
        }

    }


    private void ShowLaser(RaycastHit hit)
    {
        // Show the laser
        laser.SetActive(true);

        // Position the laser b/w controller and pt where raycast hits
        // Use 'lerp' cos you can give it two positions and the percent it should travel
        // If we pass in 0.5f, it returns the precise middle point
        laserTransform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint, .5f);

        // Point laser at position where raycast hit
        laserTransform.LookAt(hitPoint);

        // Scale laser so it fits perfectly b/w the two positions
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }
}
