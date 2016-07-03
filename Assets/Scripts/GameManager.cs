using UnityEngine;
using System;


public enum State {
    Menu,
    Start,
    Play,
    EnterPause,
    Pause,
    ExitPause,
    End,
    Restart
};


public class GameManager : MonoBehaviour {

    // Use this for initialization
    void Start () {
        if (m_oMainCharacter != null) {
            //m_oMainCharacter.GetComponent<BirdFly>().OnCharCollision += CharacterCollision;
            //m_oMainCharacter.GetComponent<BirdFly>().OnCharOutOfBound += CharacterOutOfBorder;
        }
        
    }
    
    // Update is called once per frame
    void Update () {
        if(m_eCurrentState == State.Menu) {
            OnMenu();
        }
        else if (m_eCurrentState == State.Start)
            OnStart();
        else if (m_eCurrentState == State.Play)
            OnPlay();
        else if (m_eCurrentState == State.Pause)
            OnPause();
        else if (m_eCurrentState == State.EnterPause)
            OnEnterPause();
        else if (m_eCurrentState == State.ExitPause)
            OnExitPause();
        else if (m_eCurrentState == State.End)
            OnEnd();


        if (m_eCurrentState == State.Menu && m_bStart) {
            m_eCurrentState = State.Start;
        } else if (m_eCurrentState == State.Start) {
            m_eCurrentState = State.Play;
        }
        else if (m_eCurrentState == State.Play && m_bPause) {
            m_eCurrentState = State.EnterPause;
        }
        else if (m_eCurrentState == State.Play && m_bCharDied) {
            m_eCurrentState = State.End;
        }
        else if (m_eCurrentState == State.Pause && m_bRestart) {
            m_eCurrentState = State.Start;
        }
        else if (m_eCurrentState == State.Pause && !m_bPause) {
            m_eCurrentState = State.ExitPause;
        }
        else if(m_eCurrentState == State.End && m_bStart && !m_bCharDied) {
            m_eCurrentState = State.Start;
        }

    }

    void CharacterCollision() {
        Debug.Log("Personaggio morto!");
        m_bCharDied = true;
    }

    void CharacterOutOfBorder() {
        if(m_eCurrentState == State.End) {
            if (OnReInit != null) {
                OnReInit();
            }
            m_bCharDied = false;

            m_oMainCharacter.GetComponent<Rigidbody>().velocity = Vector3.zero;
            m_oMainCharacter.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            m_oMainCharacter.GetComponent<Rigidbody>().useGravity = false;
        }
    }


    void OnMenu() {
        m_oStartMenu.SetActive(true);
    }

    void OnStart() {
        Debug.Log("Start");
        m_bPause = false;
        m_bCharDied = false;
        m_bStart = false;
        m_bEnd = false;

        

        m_oEndMenu.SetActive(false);
        m_oStartMenu.SetActive(false);
        m_oGameUI.SetActive(true);
    }

    void OnPlay() {
        m_fElapsedTime += Time.deltaTime;
        m_iPoints = (int)m_fElapsedTime;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            m_bPause = true;
        }
    }


    void OnEnterPause() {

        Debug.Log("Gioco Entra in Pausa");


        m_oStartMenu.SetActive(false);
        m_oPauseMenu.SetActive(true);
        m_oEndMenu.SetActive(false);
        m_oGameUI.SetActive(false);

        m_eCurrentState = State.Pause;
    }


    void OnPause() {

    }

    void OnExitPause() {

        Debug.Log("Gioco Continua");


        m_oPauseMenu.SetActive(false);
        m_oGameUI.SetActive(true);
        m_eCurrentState = State.Play;
    }

    void OnEnd() {

        m_oPauseMenu.SetActive(false);
        m_oGameUI.SetActive(false);
        m_oEndMenu.SetActive(true);
  
    }


    public void startGame() {
        m_bStart = true;
    }

    public void endGame() {
        m_bEnd = true;
    }

    public void continueGame() {
        m_bPause = false;
    }

    public GameObject m_oMainCharacter;
    public GameObject m_oGameUI;

    public GameObject m_oStartMenu;
    public GameObject m_oPauseMenu;
    public GameObject m_oEndMenu;

    public bool m_bPause = false;
    public bool m_bCharDied = false;
    public bool m_bRestart = false;
    public bool m_bEnd = false;
    public bool m_bStart = false;
    public int m_iPoints = 0;
    public float m_fElapsedTime = 0;
    public State m_eCurrentState;

    public event Action OnReInit;
}
