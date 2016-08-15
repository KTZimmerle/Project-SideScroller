using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {

    public float rotSpeedX;
    public float rotSpeedY;
    public float rotSpeedZ;
    public bool randomize;

    void Start()
    {
        if (randomize)
        { 
            rotSpeedX *= Random.Range(0.1f, 1.5f);
            rotSpeedZ *= Random.Range(0.1f, 1.5f);
        }
    }

    void Update()
    {
        transform.Rotate(rotSpeedX + Time.deltaTime, rotSpeedY + Time.deltaTime, rotSpeedZ + Time.deltaTime);
    }
}
