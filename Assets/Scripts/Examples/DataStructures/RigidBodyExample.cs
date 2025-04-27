using UnityEngine;

public class RigidBodyExample : MonoBehaviour
{
    public Rigidbody prefab;


    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody[] rigidbodies = SpawnRigidbodies(6, 2);
    }

    public Rigidbody[] SpawnRigidbodies(int indexLength, float xSpacing)
    {
        Rigidbody[] rbs = new Rigidbody[indexLength];

        for (int i = 0; i < indexLength; i++)
        {
            Vector3 position = new Vector3(i + xSpacing, 0f, 0f);
            Rigidbody rb = Instantiate(prefab, position, Quaternion.identity);
            rbs[i] = rb;
        }

        return rbs;
    }
}
