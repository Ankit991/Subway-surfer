using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] player;
  public  string NP;
    private void Awake()
    {
        NP = PlayerPrefs.GetString("PLayerss");

    }
    void Start()
    {

        if (NP == "P1")
        {
            player[0].SetActive(false);

            player[1].SetActive(true);

            player[2].SetActive(false);

        }
        if (NP == "P2")

        {
            player[1].SetActive(false);

            player[0].SetActive(false);
            player[2].SetActive(true);

        }

        if (NP == "P3")
        {
            player[0].SetActive(true);

            player[1].SetActive(false);
            player[2].SetActive(false);

        }
    }

   
    void Update()
    {
      
    }
}
