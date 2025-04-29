using UnityEngine;

public class LerpExample : MonoBehaviour
{
    public Transform unit1, unit2, unit3;
    public Transform point1, point2 , point3;
    public float timeScale = 1.0f;
    public float length = 1.0f;
    public float min = 0.0f;
    public float max = 2.0f;

    public float timer;

    public bool isPingPong = true;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (isPingPong)
        {
            //timer = Mathf.PingPong(Time.time * timeScale, length);
            float pingPong = Mathf.PingPong(Time.time, length);
            timeScale = Mathf.Lerp(min, max, pingPong);
            timer = Mathf.PingPong(Time.time + timeScale, length);
        }

        MoveUnit1();
        MoveUnit2(); 
        MoveUnit3();
    }

    void MoveUnit1()
    {
        Vector2 pos = Vector2.Lerp((Vector2)point1.position, (Vector2)point2.position, timer);
        unit1.position = new Vector3(pos.x, pos.y, unit1.position.z);
    }

    void MoveUnit2()
    {
        Vector2 pos = Vector2.Lerp((Vector2)point2.position, (Vector2)point3.position, timer);
        unit2.position = new Vector3(pos.x, pos.y, unit2.position.z);
    }
    void MoveUnit3()
    {
        Vector2 pos = Vector2.Lerp((Vector2)unit1.position, (Vector2)unit2.position, timer);
        unit3.position = new Vector3(pos.x, pos.y, unit3.position.z);

    }

    
}
