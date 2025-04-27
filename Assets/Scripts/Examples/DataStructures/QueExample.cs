using UnityEngine;
using System.Collections.Generic;

public class QueExample : MonoBehaviour // first in first out (FIFO) - process things in the order they come in
    // examples: task management, spawn waves etc *************************************************************

{
    public GameObject testPrefab;

    public Queue<GameObject> queue = new Queue<GameObject>();

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject tempObject = Instantiate(testPrefab, transform);
            
            queue.Enqueue(tempObject);
            Debug.Log("added" + tempObject);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            var removeObject = queue.Dequeue(); // not destroying 
            Debug.Log("removed" + removeObject);
        }
    }
}
