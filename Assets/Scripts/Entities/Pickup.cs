using UnityEngine;

// abstract classes can generally be considered to be the base implementation

public abstract class Pickup : MonoBehaviour
{
    public virtual void OnPicked()
    {
        Destroy(gameObject); // object pooling in the future
    }
}
