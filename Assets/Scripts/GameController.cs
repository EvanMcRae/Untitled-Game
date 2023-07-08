using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool begun = false;
    public int stage = 0;
    [SerializeField] private Text stageLabel;
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject[] fighterOnePrefabs, fighterTwoPrefabs;
    private GameObject fighterOne, fighterTwo;

    public int fighterOneScore;
    public Text fighterOneText;
    public int fighterTwoScore;
    public Text fighterTwoText;
    public int scoreToWin;

    public int suspicion;
    public int susToLose;

    [SerializeField] private Image susMeter;

    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private SoundClip winSound, loseSound, matchStart, matchEnd;

    // Start is called before the first frame update
    void Start()
    {
        susMeter = GameObject.Find("Canvas/SuspicionBar/SuspicionBarFill").GetComponent<Image>();
        timer = GameObject.Find("Canvas/Timer").GetComponent<Timer>();
        stageLabel = GameObject.Find("Canvas/StageLabel").GetComponent<Text>();
        nextStage();
        //susMeter = GameObject.Find("Canvas/SuspicionBar/SuspicionBarFill").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            AwardScore(0);
        }
        else if(Input.GetButtonDown("Fire2")){
            AwardScore(1);
        }
        UpdateSusMeter();

        // TODO REMOVE - DEBUG!!
        if (Input.GetKeyDown(KeyCode.Y))
        {
            nextStage();
        }
    }

    //awards points to a figther based on the player's command
    void AwardScore(int fighter){
        //if f1 button
        if(fighter == 0){
            fighterOneScore += 1;
            fighterOneText.text = string.Format("{0:000}", fighterOneScore);
        }
        //if f2 button
        else if(fighter == 1){
            fighterTwoScore += 1;
            fighterTwoText.text = string.Format("{0:000}", fighterTwoScore);
        }
        //if suspicous
        //add suspicion
        CheckSusAction(fighter);
    }

    //check if the player's current action is suspicious, add apropriate suspicion if so
    void CheckSusAction(int fighter){
        //make if statements for fighters
        
        //if player 1 awared and player 1 hit, add 0%
        //if player 1 blocked, add 20%
        //if player 1 wiffed, add 40%
        //if player 1 didn't punch, add 80%

        //same for player 2

        //update sus meter
        UpdateSusMeter();

        CheckVictory();
    }

    //add suspicion if no ation taken when aproprate
    //activate when no score after good hit, somehow have way to check(perhaps put a timer to here when good hit takes place)
    void CheckSusInaction(){
        //if [winning?] fighter
        //suspicion += 10%
        //if [losing?] fighter
        //suspicion += 20%

        UpdateSusMeter();

        CheckVictory();
    }

    //updates the suspicion meter//TODO TEST TO SEE IF DONE CORRECTLY
    void UpdateSusMeter(){
        susMeter.fillAmount = suspicion / (float)susToLose;
    }

    //decide if the player won or lost
    void CheckVictory(){
        if(suspicion >= susToLose){
            LoseGame();
        }
        else if(fighterOneScore >= scoreToWin){
            if (stage == 3)
                WinGame();
            else
                WinStage();
        }
        else if(fighterTwoScore >= scoreToWin){
            LoseGame();
        }
    }

    void LoseGame()
    {
        begun = false;
        soundPlayer.PlaySound(loseSound);
        // show lose dialog with return to menu button
    }

    void WinStage()
    {
        begun = false;
        soundPlayer.PlaySound(matchEnd);
        // show win dialog with advance to next stage button
    }

    void WinGame()
    {
        begun = false;
        soundPlayer.PlaySound(winSound);
        // show win dialog with return to menu button
    }

    public void nextStage()
    {
        begun = false;
        if (stage < 3)
            StartCoroutine(PrepareStage());
    }

    IEnumerator PrepareStage()
    {
        // reset from previous stage, if applicable
        if (stage > 0)
        {
            Crossfade.FadeStart();
            yield return new WaitForSeconds(1.0f);
            
            GameObject.Destroy(fighterOne);
            GameObject.Destroy(fighterTwo);
            fighterOneText.text = string.Format("{0:000}", (fighterOneScore = 0));
            fighterTwoText.text = string.Format("{0:000}", (fighterTwoScore = 0));
            susMeter.fillAmount = (suspicion = 0);

            Crossfade.FadeEnd();
        }

        fighterOne = GameObject.Instantiate(fighterOnePrefabs[stage], new Vector3(-5.0f, -3.2f, 0f), Quaternion.identity);
        fighterTwo = GameObject.Instantiate(fighterTwoPrefabs[stage], new Vector3(5.0f, -3.2f, 0f), Quaternion.identity);

        // TODO have different times per stage?
        timer.timeLeft = 60f;
        timer.updateTimer(timer.timeLeft - 1);
        
        stageLabel.text = (stage+1) + "";
        soundPlayer.PlaySound(matchStart);

        // show animated 3-2-1 countdown text in the middle of the screen

        yield return new WaitForSeconds(3.0f);
        stage++;
        begun = true;
    }
}
