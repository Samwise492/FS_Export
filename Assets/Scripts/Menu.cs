using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    [SerializeField] private Button soundButton;
    int soundState;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Player_Name"))
            nameField.text = PlayerPrefs.GetString("Player_Name"); // get name which was written by the user
        if (PlayerPrefs.HasKey("Sound_State"))
        {
            soundState = PlayerPrefs.GetInt("Sound_State");
            soundButton.GetComponentInChildren<Text>().text = soundState == 1 ? "Sound (on)" : "Sound (off)";
            PlayerPrefs.SetInt("Sound_State", soundState);
        }   
    }

    public void OnEndEditName()
    {
        PlayerPrefs.SetString("Player_Name", nameField.text); // set name which was written by the user
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene(1); // load 1st level
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene(0); // load menu
    }

    public void OnClickResume()
    {
        SceneManager.UnloadSceneAsync(2);
        if (Time.timeScale > 0)
            Time.timeScale = 0; // speed of time flowing
        else Time.timeScale = 1;
    }

    public void OnClickSound()
    {
        soundState = soundState == 1 ? 0 : 1;
        Debug.Log(soundState);
        soundButton.GetComponentInChildren<Text>().text = soundState == 1 ? "Sound (on)" : "Sound (off)";
        PlayerPrefs.SetInt("Sound_State", soundState);
    }

}
