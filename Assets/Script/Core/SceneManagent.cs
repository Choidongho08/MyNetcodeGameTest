using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneEnum
{
    Bootstrap = 0,
    JoinScene,
    GameScene,
}

public class SceneManagent : MonoBehaviour
{
    public static SceneManagent Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        LoadScene(SceneEnum.JoinScene);
    }


    public void LoadScene(SceneEnum scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
