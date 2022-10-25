using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    [SerializeField] private Button soundButton;
    [SerializeField] private Image creditsImage;
    int soundState = 1;

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
        SceneManager.LoadSceneAsync(6); // load level selection
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickMenu()
    {
        SceneManager.LoadSceneAsync(0); // load menu
        if (Time.timeScale != 0)
            Time.timeScale = 0; // speed of time flowing
        else Time.timeScale = 1;
    }

    public void OnClickResume()
    {
        SceneManager.UnloadSceneAsync(1);
        if (Time.timeScale != 0)
            Time.timeScale = 0; // speed of time flowing
        else Time.timeScale = 1;
    }

    public void OnClickSound()
    {
        soundState = soundState == 1 ? 0 : 1;
        soundButton.GetComponentInChildren<Text>().text = soundState == 1 ? "Sound (on)" : "Sound (off)";
        PlayerPrefs.SetInt("Sound_State", soundState);
        if (PlayerPrefs.GetInt("Sound_State") == 1) // turn on
            MusicController.Instance.background.volume = 0.6f;
        if (PlayerPrefs.GetInt("Sound_State") == 0) // turn off
            MusicController.Instance.background.volume = 0f;
    }

    public void OnClickCredits()
    {
        creditsImage.gameObject.SetActive(true);
    }

    public void OnClickCreditsExit()
    {
        creditsImage.gameObject.SetActive(false);
    }

}
