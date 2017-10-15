using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Credit to Endang Ruhiyat for the amazing cabin chair models.
 * https://grabcad.com/library/aircraft-seat-bussines
 */
public class SceneInit : MonoBehaviour {

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    // The greater the value of LowPassKernelWidthInSeconds, the slower the
    // filtered value will converge towards current input sample (and vice versa).
    float lowPassKernelWidthInSeconds = 1.0f;
    // This next parameter is initialized to 2.0 per Apple's recommendation,
    // or at least according to Brady! ;)
    public float shakeDetectionThreshold = 1f;
    float lastShakeTime;

    float lowPassFilterFactor;
    Vector3 lowPassValue;
    
    GameObject cabinCamera;
    GameObject groundCamera;

    void Start()
    {
        lastShakeTime = Time.time;
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
        cabinCamera = GameObject.Find("CabinCamera");
        groundCamera = GameObject.Find("GroundCamera");
        cabinCamera.SetActive(true);
        groundCamera.SetActive(false);
    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold && Time.time - lastShakeTime > .5)
        {
            lastShakeTime = Time.time;

            // Perform your "shaking actions" here. If necessary, add suitable
            // guards in the if check above to avoid redundant handling during
            // the same shake (e.g. a minimum refractory period).
            Debug.Log("Shake event detected at time " + Time.time);
            if (cabinCamera.activeSelf)
            {
                cabinCamera.SetActive(false);
                groundCamera.SetActive(true);
            }
            else
            {
                cabinCamera.SetActive(true);
                groundCamera.SetActive(false);
            }
        }
    }
}
