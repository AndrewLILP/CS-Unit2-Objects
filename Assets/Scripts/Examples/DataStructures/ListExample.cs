using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListExample : MonoBehaviour // list are flexible and normally not as performant as arrays due to flexibility differences
{
    public GameObject testPrefab;

    public List<GameObject> list; // only holds GameObjects

    public List<GameObject> list2 = new List<GameObject>();

    public List<object> list3Generic = new List<object>(); // can hold different varaible types
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        list2.Add(Instantiate(testPrefab, transform));
        list2.Add(Instantiate(testPrefab, transform));
        list2.Add(Instantiate(testPrefab, transform));
        list2.Add(Instantiate(testPrefab, transform));

        


        list2.Add(testPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
