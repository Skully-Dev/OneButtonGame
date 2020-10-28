using System.Collections;
using UnityEngine;
using TMPro;
// start time between 15 and 30 seconds
// 40% chance of element
    // fire = 2x speed
    // ice = 0.5x speed
// min 2 players


public class Timer : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Tooltip("Background fuse sound")]
    private AudioSource fuseSound;
    [SerializeField, Tooltip("Loud onUse fuse sound")]
    private AudioSource pressSound;
    [SerializeField, Tooltip("Explosion elimination sound")]
    private AudioSource boomSound;
    [SerializeField, Tooltip("Fuse particle system")]
    private ParticleSystem fuseParticle;
    [SerializeField, Tooltip("Explosion/EndRound particle system")]
    private ParticleSystem boomParticle;
    [SerializeField, Tooltip("Snowflake object, slow elemental image")]
    private GameObject slowIcon;
    [SerializeField, Tooltip("Fire object, fast elemental image")]
    private GameObject fastIcon;

    [SerializeField, Tooltip("Eliminated Text Title")]
    private GameObject roundOverTitle;
    [SerializeField, Tooltip("End of round menu buttons")]
    private GameObject roundOverMenu;

    [SerializeField, Tooltip("Debug text to display remaining time")]
    private TextMeshProUGUI timeText;
    
    [Header("Variables")]
    [SerializeField, Tooltip("The shortest possible time in seconds at start of round")]
    private float minTime;
    [SerializeField, Tooltip("The longest possible time in seconds at start of round")]
    private float maxTime;
    [Tooltip("The time left on the timer this round.")]
    private float timer;

    [Tooltip("random number, if min, slow effect, if max, fast effect.")]
    private int effect;

    [Tooltip("Lengh of time held this turn.")]
    private float timeHeld;

    [SerializeField, Tooltip("minimum time per turn held, to greif chickens")]
    private float minTurnTime;

    [Tooltip("Acts like a trigger for end of round code.")]
    private bool roundActive = true;

    [Tooltip("initialize code at begining of each turn state.")]
    private bool isNextTurn = true;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(minTime, maxTime); //generates a random round time based on variables
        timeText.text = timer.ToString(); //updates debug timer
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) //if timer not out
        {
            //burn
            if (Input.GetMouseButton(0)) //if left click held
            {
                if (isNextTurn) //if start of turn
                {
                    pressSound.volume = 1; //loud fuse noise
                    Time.timeScale = 3; //fast particle effects
                    isNextTurn = false; //no longer start of turn
                }

                if (effect == 5) //if highest random number
                {
                    timer -= Time.unscaledDeltaTime * 2f; //fast timer countdown
                    fastIcon.SetActive(true); //display elemental effect active
                }
                else if (effect == 1) //if lowest random number
                {
                    timer -= Time.unscaledDeltaTime * 0.5f; //slow timer countdown
                    slowIcon.SetActive(true); //display elemental effect
                }
                else //otherwise
                {
                    timer -= Time.unscaledDeltaTime; //standard timer countdown
                }
                
                timeHeld += Time.unscaledDeltaTime; //measures turn duration
            }
            else //idle, play not active, LMB not down.
            {
                if (!isNextTurn) //if start of end of turn
                {
                    if (timeHeld < minTurnTime) //if turn length too short
                    {
                        timer = timer - (minTurnTime - timeHeld); //reduce timer further
                    }
                    timeHeld = 0; //reset turn time
                    //hide elemental visuals
                    fastIcon.SetActive(false);
                    slowIcon.SetActive(false);
                    pressSound.volume = 0; //loud fuse off
                    Time.timeScale = 1; //slow down particles
                    effect = Random.Range(1, 6); //randomise next elemental result
                    isNextTurn = true; //wait for next turn
                }
            }
            timeText.text = timer.ToString(); //updates debug visual timer
        }
        else if (roundActive) //if begining of end of round
        {
            roundActive = false; //no longer begining of end of round
            //end round
            Time.timeScale = 1; //particle effects back to normal speed
            //hide elemental visuals
            fastIcon.SetActive(false);
            slowIcon.SetActive(false);
            //stop fuse sound effects
            fuseSound.Stop();
            pressSound.Stop();
            boomSound.Play(); //play explosion/elimination sound effect
            fuseParticle.Stop(); //stop fuse visuals
            boomParticle.Play(); //start explosion visuals
            StartCoroutine(RoundOver());
        }
    }

    /// <summary>
    /// Timed activation of End Of Round elements, this is followed by animations on playing.
    /// </summary>
    /// <returns></returns>
    IEnumerator RoundOver()
    {
        yield return new WaitForSeconds(3);
        roundOverTitle.SetActive(true); //eliminated
        yield return new WaitForSeconds(1);
        roundOverMenu.SetActive(true); //restart/mainMenu/Quit buttons
    }
}