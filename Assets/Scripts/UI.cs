using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class UI : MonoBehaviour {

    void OnEnable() {
        Assert.IsNotNull<Text>(m_oTxtValueVolume, "m_oTxtValueVolume NON SETTED!");
        Assert.IsNotNull<Text>(m_oTxtValueMass, "m_oTxtValueMass NON SETTED!");
        Assert.IsNotNull<Text>(m_oTxtValueShape, "m_oTxtValueShape NON SETTED!");
        Assert.IsNotNull<Text>(m_oTxtValueGravity, "m_oTxtValueGravity NON SETTED!");

        Assert.IsNotNull<GameManager>(m_oGameManager, "m_oGameManager NON SETTED!");
        m_oGameManager.OnPlayerConfigurationChanged += OnConfigurationChanged;
    }


    void Start () {
        
    }
    


    void OnConfigurationChanged(float _fVolume, float _fMass, Shape _oShape, int _iGravity) {
        showVolume(_fVolume);
        showMass(_fMass);
        showShape(_oShape);
        bool _bUseGravity = (_iGravity & (int)MASK.UseGravity) != 0x0;
        bool _bGravityDown = (_iGravity & (int)MASK.GravityDown) != 0x0;
        showGravity(_bUseGravity, _bGravityDown);

    }


    void showVolume(float _fVolume) {
        m_oTxtValueVolume.text = "" + _fVolume.ToString("#.00");
    }

    void showMass(float _fMass) {
        m_oTxtValueMass.text = "" + _fMass.ToString("#.00");
    }

    void showShape(Shape _oShape) {
        if(_oShape == Shape.SPHERE) {
            m_oTxtValueShape.text = "Sphere";
        }
        else if(_oShape == Shape.CUBE) {
            m_oTxtValueShape.text = "Cube";
        }
    }


    void showGravity(bool _bUseGravity, bool _bGravityDown) {
        m_oTxtValueGravity.text = (_bUseGravity ? "Enabled" : "Disabled") + " - " + (_bGravityDown ? "Down" : "Up");
    }


    public Text m_oTxtValueVolume = null;
    public Text m_oTxtValueMass = null;
    public Text m_oTxtValueShape = null;
    public Text m_oTxtValueGravity = null;
    public GameManager m_oGameManager = null;

}
