using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableExample", menuName = "Scriptable Objects/ScriptableExample")]
public class ScriptableExample : ScriptableObject
{
    public string exampleName;
    public string exampleDescription;
    public string exampleVersion;
    public string exampleVersionDescription;
    public float minSpeed;
    public float maxSpeed;
    public Rigidbody body;
}
