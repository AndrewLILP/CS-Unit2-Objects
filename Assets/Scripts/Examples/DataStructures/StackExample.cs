using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// LAst in First out - ******** (LIFO)
/// used when you want to backtrack, work in reverse order, navigating history, AI pathfind in reverse
/// push and pop
/// </summary>

public class StackExample : MonoBehaviour
{
    public GameObject testPrefab;
    public Stack<GameObject> testStack = new Stack<GameObject>();

    GameObject tempObject;


    // Update is called once per frame
    void Update()
    {
        // push - add to stack
        if (Input.GetKeyUp(KeyCode.Z))
        {
            tempObject = Instantiate(testPrefab, transform);
            testStack.Push(tempObject);
            Debug.Log("Pushed" + tempObject.name);
        }

        // pop - remove from top of stack
        if (Input.GetKeyUp(KeyCode.X))
        {
            var removeObject = testStack.Pop();
            Debug.Log("Popped from stack" + removeObject);
            Destroy(removeObject);
        }
    }
}
