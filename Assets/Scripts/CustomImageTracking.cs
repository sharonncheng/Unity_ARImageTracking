using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// import
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class CustomImageTracking : MonoBehaviour
{
    // visualizing the variables
    [SerializeField]
    // an array of augmented gameobjects
    private GameObject[] placeablePrefabs;
    // maps name of prefab to actual gameobject
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    // getting the ARTrackedImageManager
    private ARTrackedImageManager trackedImageManager;

    // only updates once at the beginning
    private void Awake() {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs){
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.SetActive(false);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }

    // called once when script is enabled
    private void OnEnable(){
        // Subscribe to the ARTrackedImageManager's trackedImagesChanged event to be notified whenever an image is added
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }
    private void OnDisable(){
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs){
        foreach (ARTrackedImage trackedImg in eventArgs.added){
            UpdateImage(trackedImg);
        }
        foreach (ARTrackedImage trackedImg in eventArgs.updated){
            UpdateImage(trackedImg);
        }
        foreach (ARTrackedImage trackedImg in eventArgs.removed){
            spawnedPrefabs[trackedImg.name].SetActive(false); // hide the prefab
        }
    }

    private void UpdateImage(ARTrackedImage trackedImg){
        string imgName = trackedImg.referenceImage.name;
        Vector3 position = trackedImg.transform.position;

        GameObject prefab = spawnedPrefabs[imgName];
        prefab.transform.position = position; // prefab is moving with image position
        prefab.SetActive(true); // prefab visible
    }
}
