using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class WalkingAlongSpline : MonoBehaviour
{
    public SplineContainer splineContainer; // Reference to the SplineController
    public float speed = 1.0f; // Speed of movement along the spline

    private float progress = 0.0f; // Current progress along the spline (0 to 1)s
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (splineContainer == null || splineContainer.Spline == null) return;
        
        progress += speed * Time.deltaTime; // Update progress based on speed and time
        if (progress > 1.0f) progress = 0.0f;
        
        
        Vector3 position = splineContainer.Spline.EvaluatePosition(progress); // Get position on the spline
        Vector3 tangent = splineContainer.Spline.EvaluateTangent(progress); // Update the GameObject's tangent for orientation
        
        transform.position = position; // Update the GameObject's position
        
        // Update rotation to align with the tangent
        if (tangent != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(tangent);
        }
    }
}
