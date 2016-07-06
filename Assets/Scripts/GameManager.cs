using UnityEngine;
using System;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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

public enum MASK {
    UseGravity = 0x000F,
    GravityDown = 0x00F0,
    GravityUp = 0x0F00
}

public class GameManager : MonoBehaviour {

    
    void OnEnable() {
        Assert.IsNotNull<GameObject>(m_oMainCharacter, "m_oMainCharacter NON SETTED!");

        m_oMainCharacter.GetComponent<Player>().OnPlayerConfigurationChanged += OnCharConfigChanged;
        m_oMainCharacter.GetComponent<Player>().OnLevelEndReached += OnLevelWin;
    }

    void Start () {

        m_iNextLevel = m_iCurrentLevel + 1;
        m_eCurrentState = State.Menu;
        Time.timeScale = 0.0f;

        if (OnGameStartMenu != null)
            OnGameStartMenu();
    }
    
    
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
            if (OnGamePlay != null)
                OnGamePlay();
        }
        else if (m_eCurrentState == State.Play && m_bPause) {
            m_eCurrentState = State.EnterPause;
            if (OnGamePause != null)
                OnGamePause();
        }
        else if (m_eCurrentState == State.Play && m_bCharDied) {
            m_eCurrentState = State.End;
            if (OnGameEnd != null)
                OnGameEnd();
        }
        else if (m_eCurrentState == State.Pause && m_bRestart) {
            m_eCurrentState = State.Start;
            if (OnGamePlay != null)
                OnGamePlay();
        }
        else if (m_eCurrentState == State.Pause && !m_bPause) {
            m_eCurrentState = State.ExitPause;
            if (OnGamePlay != null)
                OnGamePlay();
        }
        else if(m_eCurrentState == State.End && m_bStart && !m_bCharDied) {
            m_eCurrentState = State.Start;
            if (OnGamePlay != null)
                OnGamePlay();
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
        m_oGameUI.SetActive(true);
        m_oStartMenu.SetActive(true);
        
    }

    void OnStart() {
        Time.timeScale = 1.0f;

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
        Time.timeScale = 0.0f;
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
        Time.timeScale = 1.0f;
        Debug.Log("Gioco Continua");


        m_oPauseMenu.SetActive(false);
        m_oGameUI.SetActive(true);
        m_eCurrentState = State.Play;
    }

    void OnEnd() {
        Time.timeScale = 0.0f;
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

    //Re-send the event of the player to all who care about it. So for example UI doesn't need to know player, it register on GameManager
    void OnCharConfigChanged(float _fVolume, float _fMass, Shape _oShape, int _iGravity) {
        if(OnPlayerConfigurationChanged != null) {
            OnPlayerConfigurationChanged(_fVolume, _fMass, _oShape, _iGravity);
        }
    }


    void OnLevelWin() {
        m_oWinLevelUI.SetActive(true);
        m_oGameUI.SetActive(false);
    }

    public void NextLevel() {
        SceneManager.LoadScene(m_iNextLevel);
    }


    public GameObject m_oMainCharacter;
    public GameObject m_oGameUI;

    public GameObject m_oStartMenu;
    public GameObject m_oPauseMenu;
    public GameObject m_oEndMenu;
    public GameObject m_oWinLevelUI;
    public GameObject m_oLoseLevelUI;

    public bool m_bPause = false;
    public bool m_bCharDied = false;
    public bool m_bRestart = false;
    public bool m_bEnd = false;
    public bool m_bStart = false;
    public int m_iPoints = 0;
    public float m_fElapsedTime = 0;
    public State m_eCurrentState;


    private int m_iNextLevel;
    public int m_iCurrentLevel = -1;


    public event Action OnReInit;
    public event Action<float, float, Shape, int> OnPlayerConfigurationChanged;
    public event Action OnGameStartMenu;
    public event Action OnGamePlay;
    public event Action OnGamePause;
    public event Action OnGameEnd;
}
