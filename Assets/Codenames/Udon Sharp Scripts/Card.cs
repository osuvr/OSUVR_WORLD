
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

public class Card : UdonSharpBehaviour
{
    [Header("Synced Variables")]
    [UdonSynced(UdonSyncMode.None)] public int color = 0; //0 is neutral, -1 is blue, 1 is red, 69 is black
    [UdonSynced(UdonSyncMode.None)] public bool hidden = true; //0 is black, -1 is blue, 1 is red

    [Header("Components")]
    [SerializeField] Text textObject;
    [SerializeField] Image fillObject;
    [SerializeField] Codenames_GameController gameController;
    private bool oddFrame = false;
    private int localColor = 0;
    private bool localHidden = true;
    private bool teamLeaderCache = false;
    private Animator selfAnimator;
    private Image border;
    [UdonSynced(UdonSyncMode.None)] public int wordID = -1;

    void Start()
    {
        if (textObject == null)
        {
            textObject = GetComponentInChildren<Text>(true);
        }
        if (fillObject == null)
        {
            fillObject = GetComponentInChildren<Image>(true);
        }
        if (selfAnimator == null){
            selfAnimator = GetComponent<Animator>();
        }
        if(border == null){
            border = GetComponent<Image>();
        }

        textObject.text = gameController.getWord(wordID);
    }

    private void Update(){
        if(wordID < 0){
            textObject.text = "";
            return;
        }

        oddFrame = !oddFrame;

        //Update text on odd frames
        if(oddFrame && gameController.getWord(wordID) != textObject.text){
            if(textObject.text.Length == 0 || gameController.getWord(wordID).StartsWith(textObject.text.Split('|')[0])){
                textObject.text = textObject.text + gameController.getWord(wordID).Substring(textObject.text.Length, 1);
            } else {
                textObject.text = textObject.text.Substring(1);
            }
        }

        if(localHidden != hidden){
            localHidden = hidden;
            selfAnimator.SetBool("hidden", hidden);
        }

        if(localColor != color){
            localColor = color;
            selfAnimator.SetInteger("color", color);
        }

        if(gameController.teamLeader != teamLeaderCache){
            teamLeaderCache = gameController.teamLeader;
            selfAnimator.SetBool("teamLeader", gameController.teamLeader);
        }
    }

    override public void Interact(){
        selfAnimator.SetBool("hidden", false);
        switch(color){
            case 1:
                gameController.SendCustomNetworkEvent(NetworkEventTarget.Owner, "ClickRed");
                break;
            case -1:
                gameController.SendCustomNetworkEvent(NetworkEventTarget.Owner, "ClickBlue");
                break;
            case 69:
                gameController.SendCustomNetworkEvent(NetworkEventTarget.All, "ClickBlack");
                break;
            default:
                gameController.SendCustomNetworkEvent(NetworkEventTarget.Owner, "ClickNeutral");
                break;
        }
        SendCustomNetworkEvent(NetworkEventTarget.Owner, "Unhide");
    }

    public void Unhide()
    {
        hidden = false;
    }
    public void Hide()
    {
        hidden = false;
    }

    public void Reset(){
        hidden = true;
        color = 0;
        wordID = -1;
    }
}