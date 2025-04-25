using UnityEngine;
// only contains static methods
// this is a static class so we can call it from anywhere
// cannot be instantiated, inherited or destroyed
// typically used for utility functions, helper methods, or constants
// definition of static: https://docs.microsoft.com/en-us/dotnet/csharp/programming/language-reference/keywords/static
public static class Utilities //: NON-MonoBehaviour
{
    public static string DEVICE_ID = "andfg"; // this is the device ID for the player

    public static float startingValue = 0.5f; // this is the starting value for the player

    public static float[] floats = new float[10]; // this is an array of floats that can be used anywhere in the game
    public static float MutliplySomeValues(float input1, float input2)
    {
        // this is a static method that multiplies two values
        // this is a utility function that can be used anywhere in the game
        return input1 * input2;
    }
}
