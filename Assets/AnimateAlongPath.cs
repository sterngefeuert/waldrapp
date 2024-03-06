using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateAlongPath : MonoBehaviour
{
    public GameObject objectToAnimate; // Assign this in the Inspector
    public List<Vector3> pathPoints; // Assume this is already populated
    public float speed = 5.0f; // Units per second

    private void Start()
    {

        
        if (objectToAnimate != null)
        {
            StartCoroutine(MoveAlongPath());
        }
    }

    private IEnumerator MoveAlongPath()
    {
        foreach (Vector3 point in pathPoints)
        {
            // Keep moving until the object reaches the current point
            while (objectToAnimate.transform.position != point)
            {
                objectToAnimate.transform.position = Vector3.MoveTowards(objectToAnimate.transform.position, point, speed * Time.deltaTime);
                yield return null; // Wait until next frame
            }
        }

        // Optionally loop or do something else once the path is complete
    }
}
