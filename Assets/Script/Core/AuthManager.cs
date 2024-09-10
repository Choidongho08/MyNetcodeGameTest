using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public async Task Authorization()
    {
        await UnityServices.InitializeAsync(); //유니티 서비스 초기화

        for (int i = 0; i < 5; i++)
        {
            if (await TryAuth()) break; 
            else Debug.Log("인증 실패");
        }
    }

    private async Task<bool> TryAuth() // 인증 시도
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); // 유니티서비스 익명 로그인
            if (AuthenticationService.Instance.IsAuthorized && AuthenticationService.Instance.IsSignedIn)
            {
                return true;
            }
        }
        catch (AuthenticationException e)
        {
            Debug.LogException(e);
            return false;
        }
        catch (RequestFailedException e)
        {
            Debug.LogException(e);
            return false;
        }
        return false;
    }
}