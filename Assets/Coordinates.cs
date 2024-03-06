[System.Serializable]
public class Coordinates
{
    public float latitude;
    public float longitude;
    public string timestamp;
}

[System.Serializable]
public class CoordinateList
{
    public Coordinates[] locations;
}
