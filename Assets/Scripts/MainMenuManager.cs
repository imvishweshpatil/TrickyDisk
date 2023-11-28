using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Image _soundImage;

    [SerializeField]
    private Sprite _activeSoundSprite, _inactiveSoundSprite;
    [SerializeField] private Button[] _menuButtons;

    private int _selectedButtonIndex = 0;

    private void Start()
    {
        bool sound = (PlayerPrefs.HasKey(Constants.DATA.SETTINGS_SOUND) ? PlayerPrefs.GetInt(Constants.DATA.SETTINGS_SOUND)
            : 1) == 1;
        _soundImage.sprite = sound ? _activeSoundSprite : _inactiveSoundSprite;

        AudioManager.Instance.AddButtonSound();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            AudioManager.Instance.PlayButtonSound();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateButton(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateButton(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteSelectedButton();
        }
    }

    private void NavigateButton(int direction)
    {
        _selectedButtonIndex = Mathf.Clamp(_selectedButtonIndex + direction, 0, _menuButtons.Length - 1);

    }

    private void ExecuteSelectedButton()
    {
        if (_selectedButtonIndex >= 0 && _selectedButtonIndex < _menuButtons.Length)
        {

            AudioManager.Instance.PlayButtonSound();
            
            _menuButtons[_selectedButtonIndex].onClick.Invoke();
        }
    }

    public void ClickedPlay()
    {

        SceneManager.LoadScene(Constants.DATA.GAMEPLAY_SCENE);
    }

    public void ClickedQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ToggleSound()
    {
        bool sound = (PlayerPrefs.HasKey(Constants.DATA.SETTINGS_SOUND) ? PlayerPrefs.GetInt(Constants.DATA.SETTINGS_SOUND)
           : 1) == 1;
        sound = !sound;
        _soundImage.sprite = sound ? _activeSoundSprite : _inactiveSoundSprite;
        PlayerPrefs.SetInt(Constants.DATA.SETTINGS_SOUND, sound ? 1 : 0);
        AudioManager.Instance.ToggleSound();
    }
}