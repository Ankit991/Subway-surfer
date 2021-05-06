using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class menumanager : MonoBehaviour
{
    public Text highscorepoint,cointext;
    public Text coinvalue;
    public AudioSource audiosrc;
    public Button[] Taptochangebutton;
   public string Name,Playername;
    // Start is called before the first frame update
    void Start()
    {
        highscorepoint.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        cointext.text = PlayerPrefs.GetInt("Totalcoins", 0).ToString();
        Name = PlayerPrefs.GetString("PLayerss");
    }

    // Update is called once per frame
    void Update()
    {
        Taptochangebutton[0].onClick.AddListener(Player1);
        Taptochangebutton[1].onClick.AddListener(Player2); 
        Taptochangebutton[2].onClick.AddListener(Player3);
        if (Input.GetKey(KeyCode.D))
        {
            PlayerPrefs.DeleteKey("PLayerss");
        }
    }
    public void Player1()
    {
        Playername = "P1";
        PlayerPrefs.SetString("PLayerss", Playername);
    }
    public void Player2()
    {
        Playername = "P2";
        PlayerPrefs.SetString("PLayerss",Playername);
    }
    public void Player3()
    {
        Playername = "P3";
        PlayerPrefs.SetString("PLayerss",Playername);
    }
    public void Playsound()
    {
        audiosrc.enabled = true;
    }
    public void Stopsound()
    {
        
        audiosrc.enabled = false;
    }
   
}
