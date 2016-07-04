using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    // Use this for initialization
    void Start() {
        if(m_bEnabled)
            follow();
    }

    // Update is called once per frame
    void Update() {
        if(m_bEnabled)
            follow();
    }


    void follow() {

        Vector3 _aNewPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);


        if (!m_bFreezeX) {
            _aNewPosition.x = m_oFollowed.transform.position.x;
        }

        if (!m_bFreezeY) {
            _aNewPosition.y = m_oFollowed.transform.position.y;
        }

        if (!m_bFreezeY) {
            _aNewPosition.y = m_oFollowed.transform.position.y;
        }

        gameObject.transform.position = _aNewPosition;

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
    #endregion
}
