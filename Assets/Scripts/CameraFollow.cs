using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The car to follow
    public float distance = 6f; // Distance behind the car
    public float height = 3f;   // Height above the car
    public float smoothSpeed = 5f; // How smoothly the camera follows

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position behind the car
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;
        // Smoothly move camera to desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        // Look at the car
        transform.LookAt(target);
    }
}
