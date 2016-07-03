using UnityEngine;
using System.Collections;

public class RotateX : MonoBehaviour {

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update () {
        rb.AddTorque(Vector3.right * force);
    }

    private Rigidbody rb;
    public float force = 10;
}
