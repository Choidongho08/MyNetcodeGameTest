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
            joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode); // join�ڵ� �Է½�, �����ڵ带 ���� �� ã��
            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>(); // ������ �濡 ��
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
                allocation = await Relay.Instance.CreateAllocationAsync(4); // ����Ƽ�� 4���� �� �游��� ������ �޶�
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
                joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId); // �� �濡 �� �ڵ�
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
        RelayServerData relayData = new RelayServerData(allocation, "dtls"); // dtls�� udp���� �����ϰ� �޽����� �ְ������ �ִ� ��� ��������
        transport.SetRelayServerData(relayData);

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(SceneEnum.GameScene.ToString(),
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}