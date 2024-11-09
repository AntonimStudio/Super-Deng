using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Android.Types;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private int hp = 4;
    [SerializeField] private GameObject glowingPartTop;
    [SerializeField] private GameObject glowingPartMiddle;
    [SerializeField] private GameObject glowingPartLeft;
    [SerializeField] private GameObject glowingPartRight;

    private GameObject faceCurrent;
    [SerializeField] private Material materialTurnOn;
    [SerializeField] private Material materialTurnOff;
    public MeshRenderer rendPartTop;
    public MeshRenderer rendPartMiddle;
    public MeshRenderer rendPartLeft;
    public MeshRenderer rendPartRight;

    [SerializeField] private Image imageLose;
    [SerializeField] private TimerController TC;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RedFaceScript RFS;

    private void Awake()
    {
        rendPartTop = glowingPartTop.GetComponent<MeshRenderer>();
        rendPartMiddle = glowingPartMiddle.GetComponent<MeshRenderer>();
        rendPartLeft = glowingPartLeft.GetComponent<MeshRenderer>();
        rendPartRight = glowingPartRight.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeHP();
        }
    }

    public void SetCurrentFace(GameObject face)
    {
        faceCurrent = face;
    }

    public void TakeDamage()
    {
        if (hp > 1)
        {
            hp -= 1;
        }
        else
        {
            Lose();
        }
        
        if (hp <= 3)
        {
            rendPartTop.material = materialTurnOff;
        }
        if (hp <= 2)
        {
            rendPartRight.material = materialTurnOff;
        }
        if (hp <= 1)
        {
            rendPartLeft.material = materialTurnOff;
        }
    }

    public void TakeHP()
    {
        if (hp <= 4)
        {
            hp += 1;
        }
        if (hp == 2)
        {
            rendPartLeft.material = materialTurnOn;
        }
        if (hp == 3)
        {
            rendPartRight.material = materialTurnOn;
        }
        if (hp == 4)
        {
            rendPartTop.material = materialTurnOn;
        }
    }

    private void Lose()
    {
        faceCurrent.GetComponent<FaceScript>().havePlayer = false;
        rendPartMiddle.material = materialTurnOff;
        imageLose.gameObject.SetActive(true);
        if (TC != null)
        {
            TC.timerIsRunning = false;
        }
        RFS.isTurnOn = false;
        SCD.isOn = false;
    }
}
