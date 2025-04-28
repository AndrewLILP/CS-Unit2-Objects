using UnityEngine;
using TMPro;


public class PlayerPrefsExample : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private float mouseSen;
    [SerializeField] private TMP_Text txtDisplay;


    public void SaveData()
    {
        PlayerPrefs.SetString("SAVE_DATA", inputField.text);
        PlayerPrefs.SetFloat("MOUSE_SEN", mouseSen);


    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("SAVE_DATA"))
        {
            txtDisplay.SetText(PlayerPrefs.GetString("SAVE_DATA"));
        }
        else
        {
            Debug.Log("No data to load");
        }

        if (PlayerPrefs.HasKey("MOUSE_SEN"))
        {
            txtDisplay.SetText(PlayerPrefs.GetString("MOUSE_SEN"));
        }
        else
        {
            Debug.Log("No data to load");
        }
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey("SAVE_DATA");
        //
        // PlayerPrefs.DeleteAll
    }

    
}
