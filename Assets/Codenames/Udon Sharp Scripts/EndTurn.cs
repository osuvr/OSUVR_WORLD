
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class EndTurn : UdonSharpBehaviour
{
    [SerializeField] Codenames_GameController gameController;
    void Start()
    {
        
    }

    override public void Interact(){
        gameController.SendCustomNetworkEvent(NetworkEventTarget.Owner, "ClickNeutral");
    }
}
