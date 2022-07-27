
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

public class WordBankToggle : UdonSharpBehaviour
{
    // public int id = 0;
    [SerializeField] public string bankName = "";
    [SerializeField] public string bank = "";
    [UdonSynced] public bool wordBankActive = false;

    public Text text;
    public Toggle toggle;
    void Start()
    {
        if (text != null)
        {
            text.text = bankName;
        }
    }

    override public void Interact(){
        SendCustomNetworkEvent(NetworkEventTarget.Owner, "ToggleActive");
    }

    public void ToggleActive(){
        wordBankActive = !wordBankActive;
    }

    private string[] split(string input)
    {
        string[] str_array = input.Split(',');
        for (int i = 0; i < str_array.Length; i++)
        {
            str_array[i] = str_array[i].Trim();
        }
        return str_array;
    }
    public string[] splitString()
    {
        return split(bank);
    }

    public void Update(){
        if(toggle != null && toggle.isOn != wordBankActive){
            toggle.isOn = wordBankActive;
        }
    }
}




