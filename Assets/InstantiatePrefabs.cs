using UnityEngine;

public class InstantiatePrefabs : MonoBehaviour
{
    public GameObject prefab; // Assign your prefab in the Inspector

    public int maxFilesToLoad = 5; // Set this to the maximum number of files you want to load

    void Start()
    {
        // Assuming your JSON files are located in "Assets/Resources/JsonFiles"
        object[] jsonFiles = Resources.LoadAll("Data/exports/", typeof(TextAsset));

        Debug.Log(jsonFiles.Length); // This should print the number of JSON files in the folder (e.g., 3 if you have 3 JSON files
        // Limit the number of files processed to either the length of jsonFiles or maxFilesToLoad, whichever is smaller
        int filesToProcess = Mathf.Min(jsonFiles.Length, maxFilesToLoad);


        for (int i = 0; i < filesToProcess; i++)
        {

            TextAsset jsonFile = jsonFiles[i] as TextAsset;


            // Instantiate the prefab
            GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            instance.name = jsonFile.name;

            Transform lineTransform = instance.transform.Find("Line");
            GameObject lineGameObject = lineTransform.gameObject;




            // Assuming your prefab has a component named "YourComponent" that has a public TextAsset variable named "data"
            PathOnPlane component = lineGameObject.GetComponent<PathOnPlane>();
            
            if (component != null)
            {
                // Assign the loaded TextAsset (JSON data) to the prefab's component
                component.jsonFile = jsonFile;
            }
        }
    }
}
