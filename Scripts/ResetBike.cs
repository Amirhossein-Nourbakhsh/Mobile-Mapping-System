using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBike : MonoBehaviour
{

    public Transform bodyT;
    public float _originalRotX;
    public float _originalRotZ;

    // Start is called before the first frame update
    void Start()
    {
        _originalRotX = bodyT.rotation.x;
        _originalRotZ = bodyT.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetCar()
    {
        // Move the rigidbody up by 3 metres
        bodyT.Translate(0, 3, 0);
        // Reset the rotation to what it was when the car was initialized
        bodyT.rotation = (Quaternion.Euler(new Vector3(_originalRotX, bodyT.rotation.y, _originalRotZ)));
    }
}
