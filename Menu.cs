using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screens")]
    public GameObject MainScreen;
    public GameObject LobbyScreen;
    [Header("Main Screen")]
    public Button CreateRoomButton;
    public Button JoinRoomButton;
    [Header("LobbyScreen")]
    public TextMeshProUGUI PlayerListText;
    public Button StartGameButton;

    private void Start()
    {
        CreateRoomButton.interactable = false;
        JoinRoomButton.interactable = false;

    }

    public override void OnConnectedToMaster()
    {
        CreateRoomButton.interactable = true;
        JoinRoomButton.interactable = true;

    }
    void SetScreen(GameObject screen)
    {
        MainScreen.SetActive(false);
        LobbyScreen.SetActive(false);
        screen.SetActive(true);
    }
    public void OnCreateRoomButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);


    }
    public void OnJoinRoomButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.JoinRoom(roomNameInput.text);

    }
    public void OnPlayernameupdate(TMP_InputField playerNameInput) 
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }
    public override void OnJoinedRoom()
    {
        SetScreen(LobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateLobbyUI() 
    {
        PlayerListText.text = "";
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            PlayerListText.text += player.NickName + "\n";
        }
        if (PhotonNetwork.IsMasterClient)
            StartGameButton.interactable = true;
        else
            StartGameButton.interactable = false;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }
    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(MainScreen);
    }
    [PunRPC]
    public void ChangeScene(string SampleScene)
    {
        PhotonNetwork.LoadLevel(SampleScene); // Sahneyi deðiþtirme iþlemi
    }


    public void OnStartGameButton()
    {
        photonView.RPC("ChangeScene", RpcTarget.All, "SampleScene");
        
    }

    
    

}
