
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;
using Unity.Collections;

public class Codenames_GameController : UdonSharpBehaviour
{
    [Header("Cards")]
    [SerializeField] Card card1;
    [SerializeField] Card card2;
    [SerializeField] Card card3;
    [SerializeField] Card card4;
    [SerializeField] Card card5;
    [SerializeField] Card card6;
    [SerializeField] Card card7;
    [SerializeField] Card card8;
    [SerializeField] Card card9;
    [SerializeField] Card card10;
    [SerializeField] Card card11;
    [SerializeField] Card card12;
    [SerializeField] Card card13;
    [SerializeField] Card card14;
    [SerializeField] Card card15;
    [SerializeField] Card card16;
    [SerializeField] Card card17;
    [SerializeField] Card card18;
    [SerializeField] Card card19;
    [SerializeField] Card card20;
    [SerializeField] Card card21;
    [SerializeField] Card card22;
    [SerializeField] Card card23;
    [SerializeField] Card card24;
    [SerializeField] Card card25;

    [Header("Scoreboard")]
    // [SerializeField] Text blueScoreText;
    // [SerializeField] Text redScoreText;
    // [SerializeField] Text blueCountText;
    // [SerializeField] Text redCountText;
    [SerializeField] Text turnIndicator;
    [SerializeField] Text status;
    // [UdonSynced(UdonSyncMode.None)] int blueScore = 0;
    // [UdonSynced(UdonSyncMode.None)] int redScore = 0;
    // [UdonSynced(UdonSyncMode.None)] int blueCount = 0;
    // [UdonSynced(UdonSyncMode.None)] int redCount = 0;
    [UdonSynced(UdonSyncMode.None)] bool redTurn = false;

    [Header("Custom Word Banks - Separate Words with Commas")]
    [SerializeField] WordBankToggle defaultWordBankToggle;
    [SerializeField] WordBankToggle customWordBankToggle1;
    [SerializeField] WordBankToggle customWordBankToggle2;
    [SerializeField] WordBankToggle customWordBankToggle3;
    [SerializeField] WordBankToggle customWordBankToggle4;
    [SerializeField] WordBankToggle customWordBankToggle5;

    private string[] wordIDs = new string[1]; // all 6 possible word banks
    private int[] selectedWordIDs = new int[25];
    private string[] wordBankNames = new string[6];
    private string[] wordBank0;
    private string[] wordBank1;
    private string[] wordBank2;
    private string[] wordBank3;
    private string[] wordBank4;
    private string[] wordBank5;

    private int[] wordBankStartIndexes = new int[6];

    public bool teamLeader = false;
    public bool redTurnCache = false;

    private Animator turnAnimator;

    void Start()
    {
        turnAnimator = turnIndicator.GetComponentInParent<Animator>();

        if(defaultWordBankToggle.bank == null || defaultWordBankToggle.bank == ""){
            defaultWordBankToggle.bank = "africa, agent, air, alien, amazon, angel, antarctica, apple, arm, back, band, bank, bark, beach, belt, berlin, berry, board, bond, boom, bow, box, bug, canada, capital, cell, center, china, chocolate, circle, club, compound, copper, crash, cricket, cross, death, dice, dinosaur, doctor, dog, dress, dwarf, eagle, egypt, engine, england, europe, eye, fair, fall, fan, field, file, film, fish, flute, fly, forest, fork, france, gas, ghost, giant, glass, glove, gold, grass, greece, green, ham, head, himalaya, hole, hood, hook, human, horseshoe, hospital, hotel, ice, ice cream, india, iron, ivory, jam, jet, jupiter, kangaroo, ketchup, kid, king, kiwi, knife, knight, lab, lap, laser, lawyer, lead, lemon, limousine, leadlock, log, mammoth, maple, march, mass, mercury, millionaire, model, mole, moscow, mouth, mug, needle, net, new york, night, note, novel, nurse, nut, oil, olive, olympus, opera, orange, paper, park, part, paste, phoenix, piano, telescope, teacher, switch, swing, sub, stick, staff, stadium, sprint, spike, snowman, slip, shot, shadow, server, ruler, row, rose, root, rome, rock, robot, robin, revolution, rat, racket, queen, press, port, pilot, time, tooth, tower, truck, triangle, trip, turkey, undertaker, unicorn, vacuum, van, wake, wall, war, washer, washington, water, wave, well, whale, whip, worm, yard";
        }
        if(defaultWordBankToggle.name == null || defaultWordBankToggle.name == ""){
            defaultWordBankToggle.name = "Classic Codenames";
        }

        defaultWordBankToggle.wordBankActive = true;

        wordBank0 = defaultWordBankToggle.splitString();
        wordBank1 = customWordBankToggle1.splitString();
        wordBank2 = customWordBankToggle2.splitString();
        wordBank3 = customWordBankToggle3.splitString();
        wordBank4 = customWordBankToggle4.splitString();
        wordBank5 = customWordBankToggle5.splitString();
        wordBankNames[0] = defaultWordBankToggle.name;
        wordBankNames[1] = customWordBankToggle1.name;
        wordBankNames[2] = customWordBankToggle2.name;
        wordBankNames[3] = customWordBankToggle3.name;
        wordBankNames[4] = customWordBankToggle4.name;
        wordBankNames[5] = customWordBankToggle5.name;

        wordBankStartIndexes[0] = 0;
        wordBankStartIndexes[1] = wordBank0.Length;
        wordBankStartIndexes[2] = wordBankStartIndexes[1] + wordBank1.Length;
        wordBankStartIndexes[3] = wordBankStartIndexes[2] + wordBank2.Length;
        wordBankStartIndexes[4] = wordBankStartIndexes[3] + wordBank3.Length;
        wordBankStartIndexes[5] = wordBankStartIndexes[4] + wordBank4.Length;

        wordIDs = new string[wordBankStartIndexes[5] + wordBank5.Length];

        wordBank0.CopyTo(wordIDs, wordBankStartIndexes[0]);
        wordBank1.CopyTo(wordIDs, wordBankStartIndexes[1]);
        wordBank2.CopyTo(wordIDs, wordBankStartIndexes[2]);
        wordBank3.CopyTo(wordIDs, wordBankStartIndexes[3]);
        wordBank4.CopyTo(wordIDs, wordBankStartIndexes[4]);
        wordBank5.CopyTo(wordIDs, wordBankStartIndexes[5]);

            // active0 = true;
            // active1 = false;
            // active2 = false;
            // active3 = false;
            // active4 = false;
            // active5 = false;

        status.text = "CODENAMES Debug Panel";
        turnIndicator.text = "Click New Game to Begin";
    }

    public void Reset(){
        // blueScore = 0;
        // redScore = 0;
        // blueCount = 0;
        // redCount = 0;
        redTurn = false;
        teamLeader = false;
    }

    public void SelectWords(){
        int total_word_bank_size = 0;
        for (int i = 0; i <= 5; i++)
        {
            if (GetActiveWordBank(i))
            {
                total_word_bank_size += wordBank(i).Length;
            }
        }

        int[] combinedWordBank = new int[total_word_bank_size];

        int currentIndex = 0;
        for (int i = 0; i <= 5; i++)
        {
            if (GetActiveWordBank(i))
            {
                // wordBank(i).CopyTo(combinedWordBank, currentIndex);
                for (int j = 0; j < wordBank(i).Length; j++){
                    combinedWordBank[currentIndex + j] = wordBankStartIndexes[i] + j;
                }
                currentIndex += wordBank(i).Length;
            }
        }

        int[] selectedIndices = new int[25];
        for (int i = 0; i < 25; i++){
            int startIndex = Random.Range(0, total_word_bank_size);
            int randomIndex = startIndex;
            bool already_selected = true;
            while(already_selected){
                already_selected = false;
                for (int j = 0; j < i; j++){
                    if(randomIndex + 1 == startIndex){//shouldn't be possible
                        return;
                    }
                    if(selectedIndices[j] == randomIndex){
                        already_selected = true;
                        randomIndex = (randomIndex + 1) % total_word_bank_size;
                        break;
                    }
                }
            }
            selectedWordIDs[i] = combinedWordBank[randomIndex];
            selectedIndices[i] = randomIndex;
        }
    }

    public void ResetCards(){
        card1.Reset();
        card2.Reset();
        card3.Reset();
        card4.Reset();
        card5.Reset();
        card6.Reset();
        card7.Reset();
        card8.Reset();
        card9.Reset();
        card10.Reset();
        card11.Reset();
        card12.Reset();
        card13.Reset();
        card14.Reset();
        card15.Reset();
        card16.Reset();
        card17.Reset();
        card18.Reset();
        card19.Reset();
        card20.Reset();
        card21.Reset();
        card22.Reset();
        card23.Reset();
        card24.Reset();
        card25.Reset();
    }

    public void AssignWordsToCards()
    {
        if(selectedWordIDs.Length == 25){
            card1.wordID = selectedWordIDs[0];
            card2.wordID = selectedWordIDs[1];
            card3.wordID = selectedWordIDs[2];
            card4.wordID = selectedWordIDs[3];
            card5.wordID = selectedWordIDs[4];
            card6.wordID = selectedWordIDs[5];
            card7.wordID = selectedWordIDs[6];
            card8.wordID = selectedWordIDs[7];
            card9.wordID = selectedWordIDs[8];
            card10.wordID = selectedWordIDs[9];
            card11.wordID = selectedWordIDs[10];
            card12.wordID = selectedWordIDs[11];
            card13.wordID = selectedWordIDs[12];
            card14.wordID = selectedWordIDs[13];
            card15.wordID = selectedWordIDs[14];
            card16.wordID = selectedWordIDs[15];
            card17.wordID = selectedWordIDs[16];
            card18.wordID = selectedWordIDs[17];
            card19.wordID = selectedWordIDs[18];
            card20.wordID = selectedWordIDs[19];
            card21.wordID = selectedWordIDs[20];
            card22.wordID = selectedWordIDs[21];
            card23.wordID = selectedWordIDs[22];
            card24.wordID = selectedWordIDs[23];
            card25.wordID = selectedWordIDs[24];
        }
    }

    public void SetColors(){
        redTurn = Random.Range(0, 2) == 0;

        int[] card_colors = new int[25];
        int red_count = 9;
        int blue_count = 8;
        if(!redTurn){
            blue_count = 9;
            red_count = 8;
        }
        for (int i = 0; i < red_count; i++)
        {
            int randomIndex = Random.Range(0, 25);
            while (card_colors[randomIndex] != 0)
            {
                randomIndex = (randomIndex + 1) % 25;
            }
            card_colors[randomIndex] = 1;
        }
        for (int i = 0; i < blue_count; i++)
        {
            int randomIndex = Random.Range(0, 25);
            while (card_colors[randomIndex] != 0)
            {
                randomIndex = (randomIndex + 1) % 25;
            }
            card_colors[randomIndex] = -1;
        }

        int blackIndex = Random.Range(0, 25);
        while (card_colors[blackIndex] != 0)
        {
            blackIndex = (blackIndex + 1) % 25;
        }
        card_colors[blackIndex] = 69;

        for (int i = 0; i < card_colors.Length; i++){
            switch(i){
                case 0:
                    card1.color = card_colors[i];
                    break;
                case 1:
                    card2.color = card_colors[i];
                    break;
                case 2:
                    card3.color = card_colors[i];
                    break;
                case 3:
                    card4.color = card_colors[i];
                    break;
                case 4:
                    card5.color = card_colors[i];
                    break;
                case 5:
                    card6.color = card_colors[i];
                    break;
                case 6:
                    card7.color = card_colors[i];
                    break;
                case 7:
                    card8.color = card_colors[i];
                    break;
                case 8:
                    card9.color = card_colors[i];
                    break;
                case 9:
                    card10.color = card_colors[i];
                    break;
                case 10:
                    card11.color = card_colors[i];
                    break;
                case 11:
                    card12.color = card_colors[i];
                    break;
                case 12:
                    card13.color = card_colors[i];
                    break;
                case 13:
                    card14.color = card_colors[i];
                    break;
                case 14:
                    card15.color = card_colors[i];
                    break;
                case 15:
                    card16.color = card_colors[i];
                    break;
                case 16:
                    card17.color = card_colors[i];
                    break;
                case 17:
                    card18.color = card_colors[i];
                    break;
                case 18:
                    card19.color = card_colors[i];
                    break;
                case 19:
                    card20.color = card_colors[i];
                    break;
                case 20:
                    card21.color = card_colors[i];
                    break;
                case 21:
                    card22.color = card_colors[i];
                    break;
                case 22:
                    card23.color = card_colors[i];
                    break;
                case 23:
                    card24.color = card_colors[i];
                    break;
                case 24:
                    card25.color = card_colors[i];
                    break;
            }
        }
    }

    public void NewGame(){
        int total_word_bank_size = 0;
        for (int i = 0; i <= 5; i++){
            if(GetActiveWordBank(i)){
                total_word_bank_size += wordBank(i).Length;
            }
        }
        if(total_word_bank_size < 25){
            status.text = "Not Enough Words in Selected Word Banks to Start :(";
            return;
        }

        status.text = "Resetting Cards";
        ResetCards();
        status.text = "Selecting Words";
        SelectWords();
        status.text = "Assigning Words To Cards";
        AssignWordsToCards();
        status.text = "Setting Colors";
        SetColors();
        redTurnCache = !redTurn;
        status.text = "Done";
    }

    public void ResetTeamLeaders(){
        teamLeader = false;
    }

    public string[] wordBank(int index){
        switch(index){
            case 0:
                return wordBank0;
            case 1:
                return wordBank1;
            case 2:
                return wordBank2;
            case 3:
                return wordBank3;
            case 4:
                return wordBank4;
            case 5:
                return wordBank5;
        }
        return wordBank0;
    }

    public void SetActiveWordBank(int index, bool active)
    {
        switch (index)
        {
            case 0:
                defaultWordBankToggle.wordBankActive = active;
                break;
            case 1:
                customWordBankToggle1.wordBankActive = active;
                break;
            case 2:
                customWordBankToggle2.wordBankActive = active;
                break;
            case 3:
                customWordBankToggle3.wordBankActive = active;
                break;
            case 4:
                customWordBankToggle4.wordBankActive = active;
                break;
            case 5:
                customWordBankToggle5.wordBankActive = active;
                break;
        }

        int total_word_bank_size = 0;
        int selected_word_bank_count = 0;
        string selectedWordBanks = "";
        for (int i = 0; i <= 5; i++)
        {
            if (GetActiveWordBank(i))
            {
                selected_word_bank_count++;
                total_word_bank_size += wordBank(i).Length;
                if (selectedWordBanks == "")
                {
                    selectedWordBanks = wordBankNames[i];
                }
                else
                {
                    selectedWordBanks = selectedWordBanks + " + " + wordBankNames[i];
                }
            }
        }
        if (selected_word_bank_count > 0)
        {
            status.text = total_word_bank_size + " Words | " + selectedWordBanks;
        }
        else
        {
            status.text = "Please Select Some Word Banks";
        }
    }

    public bool GetActiveWordBank(int index)
    {
        switch (index)
        {
            case 0:
                return defaultWordBankToggle.wordBankActive ;
            case 1:
                return customWordBankToggle1.wordBankActive ;
            case 2:
                return customWordBankToggle2.wordBankActive ;
            case 3:
                return customWordBankToggle3.wordBankActive ;
            case 4:
                return customWordBankToggle4.wordBankActive ;
            case 5:
                return customWordBankToggle5.wordBankActive ;
        }
        return false;
    }

    private void Update(){
        if(redTurn != redTurnCache){
            redTurnCache = redTurn;
            if(redTurn){
                turnIndicator.text = "Red Team's Turn";
                Animator turnAnimator = turnIndicator.GetComponentInParent<Animator>();
                if (turnAnimator)
                {
                    turnAnimator.SetInteger("color", 1);
                    turnAnimator.SetBool("hidden", false);
                }
            } else {
                turnIndicator.text = "Blue Team's Turn";
                Animator turnAnimator = turnIndicator.GetComponentInParent<Animator>();
                if(turnAnimator){
                    turnAnimator.SetInteger("color", -1);
                    turnAnimator.SetBool("hidden", false);
                }
            }
        }
    }

    public void ClickRed()
    {
        if(!redTurn){
            redTurn = !redTurn;
        }
    }
    public void ClickBlue()
    {
        if(redTurn){
            redTurn = !redTurn;
        }
    }
    public void ClickBlack()
    {
        turnIndicator.text = "GAME OVER";
        Animator turnAnimator = turnIndicator.GetComponentInParent<Animator>();
        if (turnAnimator)
        {
            turnAnimator.SetInteger("color", 69);
            turnAnimator.SetBool("hidden", false);
        }
    }
    public void ClickNeutral()
    {
        redTurn = !redTurn;
    }


    public string getWord(int wordID){
        if(wordID >= 0 && wordID < wordIDs.Length){
            return wordIDs[wordID];
        } else {
            return "";
        }
    }
}
