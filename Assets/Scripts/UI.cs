using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    // Use this for initialization
    void Start () {
 
    }
    
    // Update is called once per frame
    void Update () {

        txt.text = "Score: " + m_oGameManager.m_iPoints;
    }


    public GameManager m_oGameManager;
    public Text txt;
}
