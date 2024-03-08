using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PathOnPlane : MonoBehaviour
{
    public TextAsset jsonFile; // Assign your JSON file in the inspector
    public LineRenderer lineRenderer;
    public float scale = 1.0f; // Scale factor for adjusting the spacing on the flat surface
    public List<Vector3> pathPoints = new List<Vector3>(); // This will be populated from JSON
    public GameObject objectToAnimate;
    public float speed = 5.0f; // Units per second
    public int sampleRate = 1; // Use every 'sampleRate'th point from the pathPoints list
    public bool animate = true; 

    void Start()
    {
        ReadCoordinatesAndCreatePath();

        if (objectToAnimate != null)
        {
            StartCoroutine(MoveAlongPath());
        }
    }



    void ReadCoordinatesAndCreatePath()
    {
        CoordinateList coordinateList = JsonUtility.FromJson<CoordinateList>(jsonFile.text);

        for (int i = 0; i < coordinateList.locations.Length; i += sampleRate)
        {
            Coordinates coordinates = coordinateList.locations[i];
            Vector3 point = LatLongToFlatSurface(coordinates.latitude, coordinates.longitude, scale);
            pathPoints.Add(point);
        }

        Vector3 center = CalculateCenter(pathPoints);
        for (int i = 0; i < pathPoints.Count; i++)
        {
            pathPoints[i] -= center; // Centering the path around the GameObject's origin
        }

        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray());
    }

    Vector3 CalculateCenter(List<Vector3> pathPoints)
    {
        Vector3 center = Vector3.zero;
        foreach (Vector3 point in pathPoints)
        {
            center += point;
        }
        return center / pathPoints.Count;
    }

    Vector3 LatLongToFlatSurface(float latitude, float longitude, float scale)
    {
        float x = longitude * scale;
        float y = latitude * scale;
        return new Vector3(x, 0, y);
    }
    private IEnumerator MoveAlongPath()
    {
        while (animate) // Loop indefinitely
        {
            for (int i = 0; i < pathPoints.Count - 1; i++)
            {
                Vector3 startPoint = pathPoints[i];
                Vector3 endPoint = pathPoints[i + 1];

                // Rotate to face the next point
                Vector3 direction = (endPoint - startPoint).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    objectToAnimate.transform.rotation = lookRotation;
                }

                // Move to the next point
                while (Vector3.Distance(objectToAnimate.transform.position, endPoint) > 0.01f) // Use a threshold to avoid floating point precision issues
                {
                    objectToAnimate.transform.position = Vector3.MoveTowards(objectToAnimate.transform.position, endPoint, speed * Time.deltaTime);
                    yield return null; // Ensure this happens smoothly over time
                }
            }

            // Reset to start position before looping again, if needed
            // objectToAnimate.transform.position = pathPoints[0];
            // Alternatively, just let it loop without this line if not needed
        }
    }

}
