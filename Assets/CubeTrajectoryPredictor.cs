using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class CubeTrajectoryPredictor : MonoBehaviour
{
    public GameObject markerPrefab;
    public UnityEngine.UI.Slider minMaxSlider; // Use UnityEngine.UI.Slider instead
    private GameObject markerInstance;
    private Rigidbody rb;
    private Vector3 previousPosition;
    private float updateTimer = 0f;
    private float updateInterval = 0.1f; // Interval in seconds

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (markerPrefab != null)
        {
            markerInstance = Instantiate(markerPrefab);
            markerInstance.SetActive(false);
        }
    }
    void Update()
    {
        updateTimer += Time.deltaTime; // Increment the timer

        // Check if the update interval has elapsed and the cube's position has changed
        if (updateTimer >= updateInterval && transform.position != previousPosition)
        {
            Vector3 predictedPosition = PredictLandingPosition();
            float noiseFactor = ReadNoiseFactorFromFile();

            if (noiseFactor > 0 && minMaxSlider != null)
            {
                float scaledNoiseFactor = noiseFactor * minMaxSlider.value;

                predictedPosition += new Vector3(
                    Random.Range(-scaledNoiseFactor, scaledNoiseFactor),
                    0, // No noise in y-axis
                    Random.Range(-scaledNoiseFactor, scaledNoiseFactor)
                );
            }
            if (markerInstance != null)
            {
                markerInstance.transform.position = predictedPosition;
                markerInstance.SetActive(true);
            }

            previousPosition = transform.position; // Update previous position
            updateTimer = 0f; // Reset the timer
        }
    }
Vector3 PredictLandingPosition()
{
    Vector3 currentPosition = transform.position;
    Vector3 currentVelocity = rb.velocity;
    Vector3 gravity = Physics.gravity;
    float timeStep = Time.fixedDeltaTime;
    int maxSteps = 9999; // Adjust as needed to ensure the prediction runs long enough
    float yOffset = -0.5f; // Adjust this value to determine how far below the object the prediction should be

    for (int i = 0; i < maxSteps; i++)
    {
        currentVelocity += gravity * timeStep;
        currentPosition += currentVelocity * timeStep;

        if (currentPosition.y <= transform.position.y + yOffset) // Stop the prediction below the object by applying yOffset
        {
            currentPosition.y = Mathf.Max(currentPosition.y, transform.position.y + yOffset); // Ensure predicted y is not above the object with the offset
            break;
        }
    }

    return currentPosition;
}


    float ReadNoiseFactorFromFile()
    {
        string filePath = Path.Combine(Application.dataPath, "neuralInterpretations.txt");
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("File not found: " + filePath);
            return 0f;
        }

        IEnumerable<string> lines = File.ReadLines(filePath).Reverse().Take(10);
        List<float> noiseFactors = new List<float>();

        foreach (string line in lines)
        {
            if (float.TryParse(line, out float noiseFactor))
            {
                noiseFactors.Add(noiseFactor);
            }
            else
            {
                Debug.LogWarning("Invalid number in file: " + line);
            }
        }

        if (noiseFactors.Count == 0)
        {
            return 0f;
        }

        float averageNoiseFactor = noiseFactors.Average();
        return averageNoiseFactor;
    }
}
