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
        Vector3 predictedPosition = PredictLandingPosition();
        float noiseFactor = ReadNoiseFactorFromFile();
        if (noiseFactor > 0 && minMaxSlider != null)
        {
            float scaledNoiseFactor = noiseFactor * minMaxSlider.value; // Use minMaxSlider.value instead of GetValue()

            predictedPosition += new Vector3(
                Random.Range(-scaledNoiseFactor, scaledNoiseFactor),
                Random.Range(-scaledNoiseFactor, scaledNoiseFactor),
                Random.Range(-scaledNoiseFactor, scaledNoiseFactor)
            );
        }

        if (markerInstance != null)
        {
            markerInstance.transform.position = predictedPosition;
            markerInstance.SetActive(true);
        }
    }

    Vector3 PredictLandingPosition()
    {
        Vector3 currentPosition = transform.position;
        Vector3 currentVelocity = rb.velocity;
        Vector3 gravity = Physics.gravity;
        float timeStep = Time.fixedDeltaTime;
        int maxSteps = 9999; // Adjust as needed to ensure the prediction runs long enough

        for (int i = 0; i < maxSteps; i++)
        {
            currentVelocity += gravity * timeStep;
            currentPosition += currentVelocity * timeStep;

            if (currentPosition.y <= 0)
            {
                currentPosition.y = 0;
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
