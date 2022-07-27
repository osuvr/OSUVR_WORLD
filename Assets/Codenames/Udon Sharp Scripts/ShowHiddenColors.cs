
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ShowHiddenColors : UdonSharpBehaviour
{
    [SerializeField] Codenames_GameController gameController;
    void Start()
    {
        
    }

    override public void Interact(){
        gameController.teamLeader = !gameController.teamLeader;
    }
}
