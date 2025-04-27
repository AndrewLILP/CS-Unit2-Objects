using UnityEngine;

public class ArrayExample : MonoBehaviour
{
    public GameObject textPrefab;

    // Non-Initialized Array (default size)

    public GameObject[] testArray;

    public GameObject[] array = new GameObject[3];

    public int[] ints = new int[3];

    public float[] dropRates = new float[5];

    private void Start()
    {
        array[0] = Instantiate(textPrefab, transform);
        array[0].transform.position = new Vector2(0,0);

        array[1] = Instantiate(textPrefab, transform);
        array[1].transform.position = new Vector2(1, 0);

        array[2] = Instantiate(textPrefab, transform);
        array[2].transform.position = new Vector2(2, 0);



    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            float grabValue = dropRates[Random.Range(0, dropRates.Length)];
            Debug.Log(grabValue);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Destroy(array[0]);
            Debug.Log(array[0].name);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            array[0] = Instantiate(textPrefab, transform);
        }
    }
    // lists can have multiple types - arrays are 1 type of data and size can be set
}
