using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{

    private Dictionary<string, GameObject> uiPanels = new Dictionary<string, GameObject>();

    public Button start = default;
    public Button exit = default;

    public string sceneName = "ControllableScene";
    public GameObject mainMenuPanel = default;
    public GameObject gamePanel = default;
    public GameObject finishPanel = default;

    override public void Awake()
    {
        uiPanels.Clear();

        uiPanels.Add("MainMenu", mainMenuPanel);
        uiPanels.Add("Game", gamePanel);
        uiPanels.Add("Finish", finishPanel);
    }

    void Start()
    {
        HideAllPanels();

        // 显示主菜单
        ShowPanel("MainMenu");

        
        start.onClick.AddListener(LoadScene);
        exit.onClick.AddListener(ExitGame);
    }

    private void LoadScene()
    {
        Debug.Log("LoadScene!!");
        SceneManager.LoadScene("ControllableScene");
    }
    private void ExitGame()
    {
        Debug.Log("Exit Game!!");
#if UNITY_EDITOR
        // 如果在编辑器中运行，则停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 如果在 standalone 或其他平台上运行，则退出应用程序
        Application.Quit();
#endif

    }
    public void ShowPanel(string panelName)
    {
        if (uiPanels.TryGetValue(panelName, out GameObject panel))
        {
            panel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Panel not found: " + panelName);
        }
    }

    public void HidePanel(string panelName)
    {
        if (uiPanels.TryGetValue(panelName, out GameObject panel))
        {
            panel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Panel not found: " + panelName);
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in uiPanels.Values)
        {
            panel.SetActive(false);
        }
    }

}
