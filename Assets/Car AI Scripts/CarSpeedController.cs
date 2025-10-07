using UnityEngine;

public class CarSpeedController : MonoBehaviour
{
    public float maxTorque = 50f;
    public float topSpeed = 150f;
    public float decelerationSpeed = 10f;

    private Rigidbody rb;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ControlSpeed()
    {
        if (rb == null || wheelRL == null || wheelRR == null) return;
        float currentSpeed = rb.linearVelocity.magnitude * 3.6f;
        if (currentSpeed < topSpeed)
        {
            wheelRL.motorTorque = maxTorque;
            wheelRR.motorTorque = maxTorque;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
        else
        {
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
            wheelRL.brakeTorque = decelerationSpeed;
            wheelRR.brakeTorque = decelerationSpeed;
        }
    }
}
