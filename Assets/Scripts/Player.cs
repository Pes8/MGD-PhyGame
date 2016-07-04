using UnityEngine;
using System;
using UnityEngine.Assertions;

public enum Shape {
    SPHERE = 0,
    CUBE = 1,
    SIZE = 2 // not really a shape.
};


public class Player : MonoBehaviour {


    void Start () {
        m_oMyRigidBody = gameObject.GetComponent<Rigidbody>();
        m_oMyTransform = gameObject.GetComponent<Transform>();
        m_oMyMeshFilter = gameObject.GetComponent<MeshFilter>();

        Assert.IsNotNull<Rigidbody>(m_oMyRigidBody, "RigidBody NON SETTED!");
        Assert.IsNotNull<Transform>(m_oMyTransform, "Transform NON SETTED!");
        Assert.IsNotNull<MeshFilter>(m_oMyMeshFilter, "Mesh Filter NON SETTED!");

        m_bUseGravity = m_oMyRigidBody;

        m_oObjectsCollider[0] = new SphereCollider();
        m_oObjectsCollider[1] = new BoxCollider();

#if UNITY_EDITOR
        foreach (Mesh _m in m_oObjectsMesh) {
            Assert.IsNotNull<Mesh>(_m, "Mesh NON SETTED!");
        }

        foreach (Collider _m in m_oObjectsCollider) {
            Assert.IsNotNull<Collider>(_m, "Collider NON SETTED!");
        }
#endif
        sendChangeConfigEvent();
    }
    

    void Update () {
        if (Input.GetKey(KeyCode.V)) {
            if(Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.UpArrow)) {
                Debug.Log("VOLUME SU!");
                changeVolume(1);
            }
            else if(Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.DownArrow)) {
                Debug.Log("VOLUME GIU!");
                changeVolume(-1);
            }
        }

        else if (Input.GetKey(KeyCode.M)) {
            if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.UpArrow)) {
                Debug.Log("MASS SU!");
                changeMass(1);
            } else if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.DownArrow)) {
                Debug.Log("MASS GIU!");
                changeMass(-1);
            }
        }

        else if (Input.GetKeyDown(KeyCode.G)) { 
            toggleGravity();
        }
        
        else if (Input.GetKeyDown(KeyCode.I)) {
            invertGravity();
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            changeShape();
        }
    }


    void changeVolume(int _iSign) {
        Vector3 _oValue = (m_oMultiplier.x * _iSign) * Vector3.one;

        //Checking only one axys because the scale is homogeneus
        if ((m_oMyTransform.localScale + _oValue).x > Vector3.zero.x) {
            m_oMyTransform.localScale += _oValue;
        }

        sendChangeConfigEvent();
    }


    void changeMass(int _iSign) {
        float _fValue = m_oMultiplier.y * _iSign;

        //Checking only one axys because the scale is homogeneus
        if (m_oMyRigidBody.mass + _fValue > 0.001f) {
            m_oMyRigidBody.mass += _fValue;
        }

        sendChangeConfigEvent();
    }


    void toggleGravity() {
        m_bUseGravity = !m_bUseGravity;
        m_oMyRigidBody.useGravity = m_bUseGravity;
        sendChangeConfigEvent();
    }


    void invertGravity() {
        m_bGravityDown = !m_bGravityDown;
        Physics.gravity *= -1;
        sendChangeConfigEvent();
    }

    void changeShape() {

        m_oShape = (Shape)(((int)(m_oShape+1)) % ((int)Shape.SIZE));
        Destroy(gameObject.GetComponent<Collider>());

        if (m_oShape == Shape.CUBE) {
            m_oMyMeshFilter.mesh = m_oObjectsMesh[(int)m_oShape];
            gameObject.AddComponent<BoxCollider>();
        }

        if (m_oShape == Shape.SPHERE) {
            m_oMyMeshFilter.mesh = m_oObjectsMesh[(int)m_oShape];
            gameObject.AddComponent<SphereCollider>();
        }

        sendChangeConfigEvent();
    }


    void sendChangeConfigEvent() {
        if (OnPlayerConfigurationChanged != null) {
            int _iGravity = (m_bUseGravity ? (int)MASK.UseGravity : 0);
            _iGravity |= (m_bGravityDown ? (int)MASK.GravityDown : (int)MASK.GravityUp);
            OnPlayerConfigurationChanged(m_oMyTransform.localScale.x, m_oMyRigidBody.mass, m_oShape, _iGravity);
        }
    }


    void OnTriggerEnter(Collider other) {
        if(other.tag == TAG_TRIGGER_END) {
            //Reached end of level, WIN!
            if (OnLevelEndReached != null)
                OnLevelEndReached();
        }
    }


    //VARS
    #region VARS

    public static string TAG_TRIGGER_END = "EndLevel";

    [SerializeField]
    private Shape m_oShape = Shape.SPHERE;

    public Mesh[] m_oObjectsMesh = new Mesh[(int)Shape.SIZE];
    public Collider[] m_oObjectsCollider = new Collider[(int)Shape.SIZE];


    public Vector3 m_oMultiplier = new Vector3(1.0f, 1.0f, 1.0f); // x -> scale, y -> mass, z -> ???

    private Rigidbody m_oMyRigidBody = null;
    private Transform m_oMyTransform = null;
    private MeshFilter m_oMyMeshFilter = null;

    private bool m_bUseGravity = true;
    private bool m_bGravityDown = true;

    public event Action<float, float, Shape, int> OnPlayerConfigurationChanged;
    public event Action OnLevelEndReached;
    #endregion

}
