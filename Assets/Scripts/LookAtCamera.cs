using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType; // This should be outside the method, at the class level

    public enum BillboardType { LookAtCamera };

    // LateUpdate is called after all Update methods are called
    void LateUpdate()
    {
        if (billboardType == BillboardType.LookAtCamera)  // This checks if the billboard type is set to LookAtCamera
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);  // Make the character face the camera
        }
    }
}
