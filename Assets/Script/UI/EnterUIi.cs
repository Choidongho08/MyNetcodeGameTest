using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.UI;

public class EnterUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    private static string joinCode = string.Empty;
    private Allocation allocation;
    private JoinAllocation joinAllocation;

    private void Awake()
    {
        hostButton.onClick.AddListener(HandleHostStartEvent);
        clientButton.onClick.AddListener(HandleClientStartEvent);
        NetworkManager.Singleton.ConnectionApprovalCallback += Approval;
    }

    private void Approval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;
        
    }

    private async void HandleClientStartEvent()
    {
        joinCode = ipInput.text;
        try
        {
            joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode); // join코드 입력시, 조인코드를 통해 방 찾기
            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>(); // 있으면 방에 들어감
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            transport.SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private async void HandleHostStartEvent()
    {
        while (true)
        {
            try
            {
                allocation = await Relay.Instance.CreateAllocationAsync(4); // 유니티에 4명이 들어갈 방만들고 인증을 달라
                break;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                continue;
            }
        }

        while (true)
        {
            try
            {
                joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId); // 그 방에 들어갈 코드
                Debug.Log(joinCode);
                break;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                continue;
            }
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayData = new RelayServerData(allocation, "dtls"); // dtls란 udp에서 안전하게 메시지를 주고받을수 있는 통신 프로토콜
        transport.SetRelayServerData(relayData);

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(SceneEnum.GameScene.ToString(),
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}