
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class StartGame : UdonSharpBehaviour
{
    void Start()
    {
        
    }

    [SerializeField] Codenames_GameController gameController;

    override public void Interact()
    {
        SendCustomNetworkEvent(NetworkEventTarget.Owner, "StartNewGame");
    }

    public void StartNewGame(){
        gameController.NewGame();
    }
}