using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour
{
    private ARRaycastManager rayManager;
    private GameObject visual;
    bool isCreated = false;

    //private GameObject variableForPrefab;
    //public GameObject myPrefab;

    private GameObject foodPrefab;
    

    void Start ()
    {

        foodPrefab = (GameObject)Resources.Load("prefabs/NoodleMeshCleaned", typeof(GameObject));

        //foodPrefab = Instantiate(Resources.Load("prefabs/cube", typeof(GameObject))) as GameObject;

        // get the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;

        // hide the placement indicator visual
        visual.SetActive(false);
    }

    
    void Update ()
    {
        // shoot a raycast from the center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // if we hit an AR plane surface, update the position and rotation
        if(hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            // foodPrefab.transform.position = hits[0].pose.position;
            // foodPrefab.transform.rotation = hits[0].pose.rotation;

            // enable the visual if it's disabled
             if(isCreated == false)
                    {
                        // foodPrefab.transform.position = hitPose.position;
                        // foodPrefab.transform.rotation = hitPose.rotation;
                        // isCreated = true;
                        visual.SetActive(true);
                    }
            // if(!visual.activeInHierarchy)
            //     visual.SetActive(true);

              if (Input.touchCount > 0)
                {

                    var hitPose = hits[0].pose;

                    if(isCreated == false)
                    {
                        Instantiate(foodPrefab, hitPose.position, hitPose.rotation);

                        // foodPrefab.transform.position = hitPose.position;
                        // foodPrefab.transform.rotation = hitPose.rotation;
                        isCreated = true;
                    }
                    else
                    {
                        //foodPrefab.SetActive(true);
                        visual.SetActive(false);


                    }
                    //  if (foodPrefab == null)
                    //     {
                    //         Instantiate(foodPrefab, hitPose.position, hitPose.rotation);
                    //     }
                    //     else
                    //     {
                    //         visual.SetActive(false);

                    //     }

                    //Touch touch = Input.GetTouch(0);
                    //touchPosition = Input.getTouch(0).position

                }
        }
    }
}