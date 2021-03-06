using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Playercontroller : MonoBehaviour
{
    Unityadsmanager unityadmanager;
    CharacterController controller;
    Power power;
    Animator Anim;
    Vector2 input;
    public int   rightmove,jumpforce;
    Vector3 velocity;
    bool Isground;
    public float Gravity = 9.81f;
    float  movespeed;
    public float jumpingnum;
    public float move,x;
    public int timedamp;
    public int ps=6;
    public bool collisionwithcontainer;
    public bool gameover;
    int gameovervalue,textvalue=4,vz=15;
    public GameObject cube;
    public   int vishal;
    //UI section 
    public GameObject Gameoverbutton;
    public GameObject Gameoverobj;
    public Text countdowntext;
    public GameObject GoText;
    public Text pointtext;
    public Text highscoretextvalue;
    public Text scorecurrentvalue;
    int pointvalue=1;
    public Text highscore;
    public GameObject highsc;
    



   public int pV=1;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    public  float dragDistance;  //minimum distance for a swipe to be registered
    private void Awake()
    {
        unityadmanager = GameObject.Find("Adsmanager").GetComponent<Unityadsmanager>();
    }
    void Start()
    {
        highscoretextvalue.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        highscore.text = PlayerPrefs.GetInt("Highscore",0).ToString();
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen

        InvokeRepeating("pointv", 4, 1);
        controller = GetComponent<CharacterController>();
        Anim = GetComponent<Animator>();
        gameovervalue = 1;
        Gameoverbutton.SetActive(false);
        GoText.SetActive(false);
        StartCoroutine(countdown());
        power = GetComponent<Power>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Highscore", 0) <= pointvalue)
        {
            PlayerPrefs.SetInt("Highscore", pointvalue);
            highscore.text = pointvalue.ToString();
        }
        if (controller.isGrounded)
        {
            Isground = true;
        }
        if (Isground && velocity.y < 0)
        {
            velocity.y = 0;
        }
        Anim.SetFloat("InputY", gameovervalue);
        Vector3 movement = transform.forward;
        controller.SimpleMove(movement * movespeed);
        if (!gameover)
        {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                                if (rightmove <= 2)
                                {
                                    rightmove += 3;
                                }
                               
                                move = rightmove;
                                collisionwithcontainer = false;
                            }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                                if (rightmove >= -2)
                                {
                                    rightmove -= 3;
                                }

                                move = rightmove;
                                collisionwithcontainer = false;
                            }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                                if (Isground)
                                {
                                    velocity.y = jumpforce;
                                    velocity.z = vz;
                                    jumpingnum = UnityEngine.Random.Range(0, 5);

                                    if (jumpingnum == 0)
                                    {
                                        Anim.SetTrigger("Normaljumptrigger");
                                    }
                                    if (jumpingnum == 1)
                                    {
                                        Anim.SetTrigger("Standingjump");
                                    }
                                    if (jumpingnum == 2)
                                    {
                                        Anim.SetTrigger("jumpdown");
                                    }
                                    if (jumpingnum == 3)
                                    {
                                        Anim.SetTrigger("runningforwardflip");
                                    }
                                    if (jumpingnum == 4)
                                    {
                                        Anim.SetTrigger("fronttwistflip");
                                    }
                                    if (jumpingnum == 5)
                                    {

                                        Anim.SetTrigger("backflip");
                                    }
                                    if (jumpingnum == 6)
                                    {

                                        Anim.SetTrigger("frontflip");
                                    }
                                }
                         

                            }
                            else
                        {   //Down swipe
                                if (Isground)
                                {
                                    Anim.SetTrigger("crouch");
                                    StartCoroutine(jumping());
                                }
                                Debug.Log("Down Swipe");
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
    
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (rightmove <= 2)
                {
                    rightmove += 3;
                }
              

                move = rightmove;
              
                collisionwithcontainer = false;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (rightmove >= -2)
                {
                    rightmove -= 3;
                }
             
                move = rightmove;
                collisionwithcontainer = false;
            }
        }
       
        x = Mathf.Lerp(x, move, timedamp * Time.deltaTime);
        if (!collisionwithcontainer)
        {
            controller.Move((x - transform.position.x) * Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Isground)
        {
           
            velocity.y = jumpforce;
            velocity.z = vz;
            jumpingnum = UnityEngine.Random.Range(0, 5);

            if (jumpingnum == 0)
            {
                Anim.SetTrigger("Normaljumptrigger");
            }
            if (jumpingnum == 1)
            {
                Anim.SetTrigger("Standingjump");
            }
            if (jumpingnum == 2)
            {
                Anim.SetTrigger("jumpdown");
            }
            if (jumpingnum == 3)
            {
                Anim.SetTrigger("runningforwardflip");
            }
            if (jumpingnum == 4)
            {
                Anim.SetTrigger("fronttwistflip");
            }
            if (jumpingnum == 5)
            {

                Anim.SetTrigger("backflip");
            }
            if (jumpingnum == 6)
            {
              
                Anim.SetTrigger("frontflip");
            }
           
        }
        if(  Input.GetKeyUp(KeyCode.Space))
        {
            velocity.z = ps;
        }
        if (Input.GetKeyDown(KeyCode.C) && Isground)
        {
            Anim.SetTrigger("crouch");
            StartCoroutine(jumping());
        }
      
        velocity.y -= Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
       
    }
   
    void pointv ()
    {  
            if (!gameover)
            {
                pointvalue += 1+pV;
                pointtext.text = pointvalue.ToString();
            scorecurrentvalue.text = pointvalue.ToString();
            }
    }
    IEnumerator jumping()
    {
      yield return   new WaitForSeconds(0.2f);
        controller.height = .7f;
        //controller.center = new Vector3(0, 1.3f, 0);
        yield return new WaitForSeconds(.85f);
        
       // controller.center = new Vector3(0, 0.65f, 0);
        controller.height = 1f;
        yield return new WaitForSeconds(.2f);
              controller.height = 1.2f;
        yield return new WaitForSeconds(.2f);
        controller.height = 1.4f;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Container"))
        {
            collisionwithcontainer = true;
        }
        if (other.gameObject.tag == "Containerfront")
        {
            collisionwithcontainer = true;
            Gameover();
        }
    }
    void Gameover()
    {
        
        if (power.invisible)
        {
            vishal += 1;
            vz = 0;
            velocity.z = 0f;
            if (vishal == 1)
            {
                Anim.SetBool("dying", true);
                StartCoroutine(vishalcount());
            }

            controller.height = 0.4f;
            controller.center = new Vector3(0f, 0.16f, 0);
            gameover = true;

            StartCoroutine(adswork());
            gameovervalue = 0;
            ps = 0;
            movespeed = 0;
            jumpforce = 0;

            Gameoverbutton.SetActive(true);
            Gameoverobj.SetActive(true);
            highscore.enabled = false;
            pointtext.enabled = false;
            highsc.SetActive(false);
        }
       
    }
    IEnumerator vishalcount()
    {
        yield return new WaitForSeconds(2f);
        Anim.SetBool("killed", true);

        Anim.SetBool("dying", false);
        vishal += 1;
    }
  public  void Restart()
    {
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    public void Home()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    IEnumerator countdown()
    {
     
        bool whilecon = true;
       
        while (whilecon)
        {
            textvalue -= 1;
            countdowntext.text = textvalue.ToString();
       
            
            if (textvalue == 0)
            {

                countdowntext.enabled = false;
                GoText.SetActive(true);
                new WaitForSeconds(1f);
                whilecon = false;
                cube.SetActive(false);
              
            }
            yield return new WaitForSeconds(1f);
            GoText.SetActive(false);
        }
    }
   
   IEnumerator adswork()
    {
        yield return new WaitForSeconds(5f);
            unityadmanager.checkgameoverforads = true;
        
    }
}
