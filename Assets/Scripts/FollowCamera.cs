using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class FollowCamera : MonoBehaviour {

    void OnEnable() {
        Assert.IsNotNull<GameManager>(m_oGameManager, "m_oGameManager NON SETTED!");

        m_oGameManager.OnGameStartMenu += OnStartMenu;
        m_oGameManager.OnGamePlay += OnGamePlay;
        m_oGameManager.OnGamePause += OnGamePause;
    }

    // Use this for initialization
    void Start() {
        m_oStartPosition = gameObject.transform;

        
    }

    // Update is called once per frame
    void Update() {
        if(m_bEnabled && m_bPlay)
            follow();

        if (!m_bPlay) {
            gameObject.transform.position = m_oStartPosition.position;
        }
    }


    void follow() {

        Vector3 _aNewPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);


        if (!m_bFreezeX) {
            _aNewPosition.x = m_oFollowed.transform.position.x;
        }

        if (!m_bFreezeY) {
            _aNewPosition.y = m_oFollowed.transform.position.y;
        }

        if (!m_bFreezeZ) {
            _aNewPosition.z = m_oFollowed.transform.position.z - 40;
        }

        gameObject.transform.position = _aNewPosition;

    }


    void OnStartMenu() {
        m_bPlay = false;
        m_bEnabled = true;
    }


    void OnGamePlay() {
        m_bPlay = true;       
    }

    void OnGamePause() {
        m_bPlay = false;
    }


    #region VARS
    //VARS
    [SerializeField]
    private GameObject m_oFollowed;

    [SerializeField]
    private bool m_bFreezeX = false;

    [SerializeField]
    private bool m_bFreezeY = true;

    [SerializeField]
    private bool m_bFreezeZ = true;

    [SerializeField]
    private bool m_bEnabled = true;

    private bool m_bPlay = false;

    private Transform m_oStartPosition;

    public GameManager m_oGameManager;

    #endregion
}
