using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMPannel : MonoBehaviour
{
    // Start is called before the first frame update
    public Button retryButt;
    public Button exitButt;
    void Start()
    {
        retryButt.onClick.AddListener(GameRetry);
        exitButt.onClick.AddListener(GameExit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GameRetry()
    {
        OnPlay.s = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        DataBase.Destroy();Debug.Log("exit") ; 
        Application.Quit();
#endif
    }
}
