using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Combat : MonoBehaviour
{
    protected Transform player;
    protected PlayableCharacter pc;
    protected Animator ac;
    protected PlayerMovement pm;
    protected Rigidbody rb;

    public float holdCd;

    protected float lastPressedTime;
    protected int comboIndex;
    public float comboTime = .25f;
    protected bool firstAtk;
    protected bool secondAtk;

    protected bool readyToAtk = true;

    public delegate void TestDelegate();
    public TestDelegate abilityMethod;

    public bool abilityReady;

    public GameObject abilityText,/* abilityIcon,*/ abilityProgress, abilityPS, shakeManager;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = this.transform.root.GetChild(0);

        ac = this.GetComponentInChildren<Animator>();
        pm = player.GetComponent<PlayerMovement>();
        rb = player.GetComponent<Rigidbody>();
        pc = player.GetComponent<PlayableCharacter>();

        abilityReady = true;
    }

    // Update is called once per frame
    public virtual void Update()
    {


        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    Debug.Log("HOLDING ATTACK");
        //}
        //if (Input.GetKey(KeyCode.F) && readyToHold) // if holding down F key
        //{
        //    //StartCoroutine(SpinAttackMov());
        //    pm.MoveInterrupt(false); // pause movement
        //    holding = true;
        //    readyToAtk = false; 
        //    hit.enabled = true;
        //    hit = colliders[0]; // set hitbox to be sphere collider (hard coded)
        //    ac.SetBool("holdAtk", true); // begin spinning attack animation
        //}
        //else if(holding == true) // if holding is true but key not pressed
        //{
        //    this.transform.rotation = pm.orientation.transform.rotation; // set rotation of player to be orientation rotation
        //    pm.MoveInterrupt(true); // allow movement
        //    ac.SetBool("holdAtk", false); // disable animation
        //    holding = false;
        //    readyToHold = true;
        //    hit = colliders[0];
        //    hit.enabled = false;
        //    readyToAtk = true;
        //    ac.SetTrigger("holdEnd"); // begin ending animation
        //}

        if (Input.GetKeyDown(KeyCode.X) && abilityReady) // universal
        {
            StartCoroutine(Ability(abilityMethod));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToAtk && pm.grounded) // if mouse clicked
        {
            Debug.Log("Combo Attack : " + (comboIndex + 1) + " / 3");
            lastPressedTime = Time.time; // set recent click time 
            if (firstAtk) // if one attack already done
            {
                secondAtk = Time.time - lastPressedTime <= comboTime; // check time between now and last pressed, true if less than combo timer
                if (secondAtk)
                {
                    bool thirdAtk = Time.time - lastPressedTime <= comboTime; // same as before but for third attack
                    if(comboIndex == 2 && thirdAtk) // if combo ready for last hit 
                    {

                        ac.SetTrigger("atkEnd");
                        firstAtk = false; // reset bools
                        secondAtk = false;
                        
                    }
                }

            }
            else
            {
                firstAtk = true; // first attack
            }
            pm.MoveInterrupt(false); // halts movement for attack
            Attack(); // call attack
            readyToAtk = false; // disable interrupting attacks
        }
        if((firstAtk || secondAtk) && Time.time - lastPressedTime > comboTime) // regardless of mouse click, if first or second click achieved + time has expired, fail combo, reset combo progress
        {
            Debug.Log("Combo Failed");
            ac.SetTrigger("atkEnd");
            secondAtk = false;
            firstAtk = false;
            comboIndex = 0;
        }
    }
    void Attack()
    {

        /*ac.Play("Attack", -1, 0f);*/ // if attack called, begin animations, set state of combo blend tree
        ac.SetTrigger("atk");
        ac.SetFloat("atkIndex", comboIndex);
        pm.transform.rotation = pm.orientation.rotation; // set rotation of player to orientation rotation
        //StartCoroutine(EnableHit()); // run coroutine for enabling hitbox

    }


    IEnumerator Ability(TestDelegate method) // universal
    {
        ac.Play("Ability", -1, 0f);
        method();

        Instantiate(abilityPS, transform.position, Quaternion.identity, transform);

        //Image image = abilityIcon.GetComponent<Image>();
        //image.color = new Color(image.color.r, image.color.g, image.color.b, .5f);
        abilityReady = false;
        yield return StartCoroutine(Timer(pc.abilityCd));
        abilityReady = true;
    }



    IEnumerator Timer(float time) // universal
    {
        float currentTime = 0;
        abilityText.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(time - currentTime) + "s";
        while (currentTime < time)
        {
            yield return new WaitForSeconds(0.1f);
            abilityProgress.GetComponent<Image>().fillAmount = 1 - (currentTime / time);
            abilityText.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(time - currentTime) + "s";
            currentTime += .1f;
        }
        //Image image = abilityIcon.GetComponent<Image>();
        //image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        abilityProgress.GetComponent<Image>().fillAmount = 0;
        abilityText.GetComponent<TextMeshProUGUI>().text = "";

    }

    //IEnumerator EnableHit()
    //{
    //    yield return new WaitForSeconds(.6f); // wait enough time for attack to start
    //    hit.enabled = true;

    //    yield return new WaitForSeconds(.3f); // turn hitbox on for near end of attack duration
    //    hit.enabled = false;

    //    //Collider[] colliders = Physics.OverlapSphere(this.transform.position, 5.0f); // finds all colliders within a radius
    //    //foreach (Collider hit in colliders)
    //    //{
    //    //    Rigidbody rb = hit.GetComponent<Rigidbody>();

    //    //    if (rb != null)
    //    //        rb.AddExplosionForce(pc.knockBackModifier, this.transform.position, 5.0f, 3.0f); // adds force to object rigidbody
    //    //}


    //    yield return new WaitForSeconds(.3f); // end of attack

        

    //}
}
