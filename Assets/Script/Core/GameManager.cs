using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await AuthManager.Instance.Authorization(); // 유니티 서비스 초기화
        SceneManagent.Instance.LoadScene(SceneEnum.JoinScene);
    }
}
