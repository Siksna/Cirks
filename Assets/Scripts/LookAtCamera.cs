using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;

    public enum BillboardType { LookAwayFromCamera };

    void LateUpdate()
    {
        if (billboardType == BillboardType.LookAwayFromCamera)
        {
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);
        }
    }
}
