using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PathAroundSphere : MonoBehaviour
{
    public TextAsset jsonFile; // Assign your JSON file in the inspector
    public GameObject sphere; // Assign the sphere GameObject
    public float radius = 5f; // Radius of the sphere
    public LineRenderer lineRenderer;

    void Start()
    {
        // lineRenderer = GetComponent<LineRenderer>();
        ReadCoordinatesAndCreatePath();
    }

    void ReadCoordinatesAndCreatePath()
    {
        CoordinateList coordinateList = JsonUtility.FromJson<CoordinateList>(jsonFile.text);

        List<Vector3> pathPoints = new List<Vector3>();
        foreach (Coordinates coordinates in coordinateList.locations)
        {
            Vector3 point = LatLongToSphere(radius, coordinates.latitude, coordinates.longitude);
            pathPoints.Add(point);
        }

        // Set the number of vertex counts first
        lineRenderer.positionCount = pathPoints.Count;

        // Now, set the positions
        lineRenderer.SetPositions(pathPoints.ToArray());

        // Optional: Customize your line renderer appearance
        lineRenderer.startWidth = 0.003f;
        lineRenderer.endWidth = 0.003f;
        // You can also set materials, colors, etc.
    }

    Vector3 LatLongToSphere(float radius, float latitude, float longitude)
    {
        // float latRad = latitude * Mathf.Deg2Rad;
        // float lonRad = longitude * Mathf.Deg2Rad;

        // float x = radius * Mathf.Cos(latRad) * Mathf.Cos(lonRad);
        // float y = radius * Mathf.Cos(latRad) * Mathf.Sin(lonRad);
        // float z = radius * Mathf.Sin(latRad);

        // return new Vector3(x, y, z);
        float latRad = (latitude + 90) * Mathf.Deg2Rad; // Adjust for texture origin
        float lonRad = (longitude + 180) * Mathf.Deg2Rad; // Adjust for texture origin

        float x = radius * Mathf.Cos(latRad) * Mathf.Cos(lonRad);
        float y = radius * Mathf.Sin(latRad);
        float z = radius * Mathf.Cos(latRad) * Mathf.Sin(lonRad);

        return new Vector3(x, y, z);
    }
}
