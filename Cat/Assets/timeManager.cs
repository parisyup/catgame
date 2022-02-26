using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject blackAndWhiteEffect;
    List<(GameObject currentObject, float magnitude, Vector3 direction)> stoppedObjects = new List<(GameObject currentObject, float magnitude, Vector3 direction)>();
    List<(GameObject currentObject, float magnitude, Vector3 direction)> newBulletsToBeStopped = new List<(GameObject currentObject, float magnitude, Vector3 direction)>();
    public void stopTime()
    {
        blackAndWhiteEffect.SetActive(true);
        var timeStoppedObjects = GameObject.FindGameObjectsWithTag("timeAffected");
        for (int i = 0; i < timeStoppedObjects.Length; i++)
        {

            Rigidbody currentRb = timeStoppedObjects[i].GetComponent<Rigidbody>();
            if (currentRb.velocity.magnitude > 0 && currentRb != null)
            {
                if (timeStoppedObjects[i].layer == 10)
                {
                    if (timeStoppedObjects[i].GetComponent<CustomBullet>().LifeTime > timeStoppedObjects[i].GetComponent<CustomBullet>().maxLifeTime - 0.4)
                    {
                        if (timeStoppedObjects[i].GetComponent<CustomBullet>().isPaused) return;
                        (GameObject currentObject, float magnitude, Vector3 direction) currentObject = (currentRb.gameObject, currentRb.velocity.magnitude, currentRb.velocity.normalized);
                        newBulletsToBeStopped.Add(currentObject);
                        Invoke("stopObjectWaitTime", ((float)(timeStoppedObjects[i].GetComponent<CustomBullet>().LifeTime - timeStoppedObjects[i].GetComponent<CustomBullet>().maxLifeTime + 0.1)));
                        isPaused = true;
                        timeStoppedObjects[i].GetComponent<CustomBullet>().isPaused = true;
                        return;
                    }

                }
                StopObject(currentRb);
            }
        }
        isPaused = true;
    }


    public void stopObjectWaitTime()
    {
        StopObject(newBulletsToBeStopped[0].currentObject.GetComponent<Rigidbody>());
        newBulletsToBeStopped.RemoveAt(0);
    }
    public void StopObject(Rigidbody currentRb)
    {     

            (GameObject currentObject, float magnitude, Vector3 direction) currentObject = (currentRb.gameObject, currentRb.velocity.magnitude, currentRb.velocity.normalized);

            stoppedObjects.Add(currentObject);

            currentRb.velocity = Vector3.zero;
            currentRb.isKinematic = true;



    }


    public void resumeTime()
    {
        blackAndWhiteEffect.SetActive(false);
        for (int i = 0; i < stoppedObjects.Count; i++)
        {
            if (stoppedObjects[i].currentObject != null) { 
            Rigidbody currentRb = stoppedObjects[i].currentObject.GetComponent<Rigidbody>();
            float recordedMagnitude = (float) stoppedObjects[i].magnitude;
            Vector3 recordedDirection = (Vector3) stoppedObjects[i].direction;

            currentRb.isKinematic = false;
            currentRb.velocity = recordedDirection * recordedMagnitude;

            if(stoppedObjects[i].currentObject.layer == 10) stoppedObjects[i].currentObject.GetComponent<CustomBullet>().isPaused = false;
        }
    }
        isPaused = false;
        stoppedObjects.Clear();
    }
}
