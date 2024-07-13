using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceScript : MonoBehaviour
{
    [SerializeField] private GameObject sideBlue; // BlueSide == Side1
    [SerializeField] private GameObject sideOrange; // OrangeSide == Side2
    [SerializeField] private GameObject sideGreen; // GreenSide == Side3
    public GameObject player;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialLightBlue;
    [SerializeField] private Material materialBlue;
    [SerializeField] private Material materialOrange;
    [SerializeField] private Material materialGreen;
    public bool havePlayer = false;
    private bool transferInProgress = false;
    [Space]
    public MeshRenderer rend;
    public GameObject glowingPart;
    public Dictionary<string, int> materials;
    public Dictionary<string, GameObject> sides;
    [Space]
    private float inputWindow = 0.25f; // ������������ ���� ����� � ��������
    [SerializeField] private RhythmManager RM;
    [SerializeField] private StartCountDown SCD;

    private void Start()
    {
        RM = FindObjectOfType<RhythmManager>();

        materials = new Dictionary<string, int>();
        sides = new Dictionary<string, GameObject>();
        if (havePlayer)
        {
            materials.Add("OrangeSide", 1);
            materials.Add("BlueSide", 2);
            materials.Add("GreenSide", 3);

            sides.Add("OrangeSide", sideOrange);
            sides.Add("BlueSide", sideBlue);
            sides.Add("GreenSide", sideGreen);

            gameObject.GetComponent<FaceScript>().rend.material = materialLightBlue;
            sideBlue.GetComponent<FaceScript>().rend.material = materialBlue;
            sideOrange.GetComponent<FaceScript>().rend.material = materialOrange;
            sideGreen.GetComponent<FaceScript>().rend.material = materialGreen;
        }

    }

    private void Update()
    {
        //Debug.Log(RM.timer.ToString() + " " + inputWindow.ToString() + " " + RM.beatInterval.ToString());
        if (havePlayer && !transferInProgress && SCD.isOn)// && RM.timer + inputWindow >= RM.beatInterval
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartTransfer(GetGameObject("OrangeSide"), GetInt("OrangeSide"), "Orange");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartTransfer(GetGameObject("GreenSide"), GetInt("GreenSide"), "Green");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartTransfer(GetGameObject("BlueSide"), GetInt("BlueSide"), "Blue");
            }
        }
    }

    private void StartTransfer(GameObject targetSide, int sideNumber, string color)
    {
        transferInProgress = true;
        sideBlue.GetComponent<FaceScript>().rend.material = materialWhite;
        sideOrange.GetComponent<FaceScript>().rend.material = materialWhite;
        sideGreen.GetComponent<FaceScript>().rend.material = materialWhite;
        StartCoroutine(TransferPlayer(targetSide, sideNumber, color));
    }

    private IEnumerator TransferPlayer(GameObject targetSide, int sideNumber, string color)
    {
        yield return new WaitForSeconds(0.01f);
        FaceScript targetFace = targetSide.GetComponent<FaceScript>();
        if (!targetFace.havePlayer)
        {
            targetFace.ReceivePlayer(player, sideNumber, color);
            havePlayer = false;
        }
        transferInProgress = false;
    }

    public void ReceivePlayer(GameObject newPlayer, int sideNumber, string color)
    {
        rend.material = materialLightBlue;

        materials.Remove("BlueSide");
        materials.Remove("OrangeSide");
        materials.Remove("GreenSide");

        sides.Remove("BlueSide");
        sides.Remove("OrangeSide");
        sides.Remove("GreenSide");

        if ((sideNumber == 1 && color == "Orange") || (sideNumber == 2 && color == "Green") || (sideNumber == 3 && color == "Blue"))
        {
            materials.Add("OrangeSide", 2);
            materials.Add("BlueSide", 3);
            materials.Add("GreenSide", 1);

            sides.Add("OrangeSide", sideBlue);
            sides.Add("BlueSide", sideGreen);
            sides.Add("GreenSide", sideOrange);

            sideBlue.GetComponent<FaceScript>().rend.material = materialOrange;
            sideOrange.GetComponent<FaceScript>().rend.material = materialGreen;
            sideGreen.GetComponent<FaceScript>().rend.material = materialBlue;
        }
        else if ((sideNumber == 1 && color == "Blue") || (sideNumber == 2 && color == "Orange") || (sideNumber == 3 && color == "Green"))
        {
            materials.Add("OrangeSide", 1);
            materials.Add("BlueSide", 2);
            materials.Add("GreenSide", 3);

            sides.Add("OrangeSide", sideOrange);
            sides.Add("BlueSide", sideBlue);
            sides.Add("GreenSide", sideGreen);

            sideBlue.GetComponent<FaceScript>().rend.material = materialBlue;
            sideOrange.GetComponent<FaceScript>().rend.material = materialOrange;
            sideGreen.GetComponent<FaceScript>().rend.material = materialGreen;
        }
        else if ((sideNumber == 1 && color == "Green") || (sideNumber == 2 && color == "Blue") || (sideNumber == 3 && color == "Orange"))
        {
            materials.Add("OrangeSide", 3);
            materials.Add("BlueSide", 1);
            materials.Add("GreenSide", 2);

            sides.Add("OrangeSide", sideGreen);
            sides.Add("BlueSide", sideOrange);
            sides.Add("GreenSide", sideBlue);

            sideOrange.GetComponent<FaceScript>().rend.material = materialBlue;
            sideBlue.GetComponent<FaceScript>().rend.material = materialGreen;
            sideGreen.GetComponent<FaceScript>().rend.material = materialOrange;
        }

        player = newPlayer;
        havePlayer = true;
        /*
        player.transform.SetParent(gameObject.transform);
        player.transform.localPosition = new Vector3(0, -10, 0);
        player.transform.localRotation = new Quaternion(0, -90, 0, 0);*/
    }

    public GameObject GetGameObject(string key)
    {
        GameObject gameObject;
        if (sides.TryGetValue(key, out gameObject))
        {
            return gameObject;
        }
        else
        {
            Debug.LogWarning($"�������� � ������ '{key}' �� ������");
            return null;
        }
    }
    public int GetInt(string key)
    {
        int num;
        if (materials.TryGetValue(key, out num))
        {
            return num;
        }
        else
        {
            Debug.LogWarning($"�������� � ������ '{key}' �� ������");
            return -1;
        }
    }
}