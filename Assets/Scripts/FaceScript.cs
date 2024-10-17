using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

// Обязательное уведомление: "Правые", "Левые" и "Верхние" указаны для треугольника с основанием, направленным ВНИЗ!!!
// Обратите внимание, что "Правая" сторона раньше носила название "BlueSide", "Левая" - "OrangeSide", а "Верхняя" - "GreenSide"
// Помимо прочих наименований, "Правая" сторона может записываться как "Side1", "Левая" - "Side2", а "Верхняя" - "Side3"
public class FaceScript : MonoBehaviour 
{

    public GameObject player;

    [Space]
    [Header("Sides of the Face")]
    [FormerlySerializedAs("sideBlue")]
    [SerializeField] private GameObject siderRight; // BlueSide == Side1
    [FormerlySerializedAs("sideOrange")]
    [SerializeField] private GameObject sideLeft; // OrangeSide == Side2
    [FormerlySerializedAs("sideGreen")]
    [SerializeField] private GameObject sideTop; // GreenSide == Side3
    public Dictionary<string, GameObject> sides;

    [Space]
    [Header("Materials")]
    [FormerlySerializedAs("materialWhite")]
    [SerializeField] private Material materialBasicFace;
    [FormerlySerializedAs("materialLightBlue")]
    [SerializeField] private Material materialPlayerFace;
    [FormerlySerializedAs("materialBlue")]
    [SerializeField] private Material materialRightFace;
    [FormerlySerializedAs("materialOrange")]
    [SerializeField] private Material materialLeftFace;
    [FormerlySerializedAs("materialGreen")]
    [SerializeField] private Material materialTopSide;
    public Dictionary<string, int> materials;

    [Space]
    [Header("Glowing&Rendering")]
    public MeshRenderer rend;
    public GameObject glowingPart;

    [Header("Key Bindings")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyTop = KeyCode.W;
    public KeyCode keyRight = KeyCode.D;


    [Space]
    [Header("Scene ScriptManagers")]
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private BeatController BC;
    [SerializeField] private SoundScript SS;
    [SerializeField] private TutorialController TC;

    [Space]
    [Header("Questions")]
    public bool havePlayer = false;
    [SerializeField] private bool isAnExtremeSide = false;
    [SerializeField] private bool isTutorial = false;
    private bool transferInProgress = false;
    [HideInInspector] public bool isKilling = false;

    private void Awake() 
    {
        rend = glowingPart.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        materials = new Dictionary<string, int>();
        sides = new Dictionary<string, GameObject>();

        if (havePlayer)
        {
            materials.Add("LeftSide", 1);
            materials.Add("RightSide", 2);
            materials.Add("TopSide", 3);

            sides.Add("LeftSide", sideLeft);
            sides.Add("RightSide", siderRight);
            sides.Add("TopSide", sideTop);

            if (TC != null && !TC._tutorialSettings[TC._index].isMoving)
            {
                player.SetActive(false);
                havePlayer = false;
            }
            else
            {
                gameObject.GetComponent<FaceScript>().rend.material = materialPlayerFace;
                siderRight.GetComponent<FaceScript>().rend.material = materialRightFace;
                sideLeft.GetComponent<FaceScript>().rend.material = materialLeftFace;
                sideTop.GetComponent<FaceScript>().rend.material = materialTopSide;
            }
        }
    }

    private void Update()
    {
        if (havePlayer && !transferInProgress && (SCD == null || SCD.isOn) && BC.canPress)
        {
            if (Input.GetKeyDown(keyLeft) || Input.GetKeyDown(keyTop) || Input.GetKeyDown(keyRight))
            {
                string direction = "";
                if (Input.GetKeyDown(keyLeft) && !Input.GetKey(keyTop) && !Input.GetKey(keyRight)) direction = "Left";
                if (Input.GetKeyDown(keyTop) && !Input.GetKey(keyLeft) && !Input.GetKey(keyRight)) direction = "Top";
                if (Input.GetKeyDown(keyRight) && !Input.GetKey(keyTop) && !Input.GetKey(keyLeft)) direction = "Right";

                if (!string.IsNullOrEmpty(direction))
                {
                    StartTransfer(GetGameObject($"{direction}Side"), GetInt($"{direction}Side"), direction);
                    BC.isAlreadyPressed = true;
                    BC.isAlreadyPressedIsAlreadyPressed = false;
                    SS.TurnOnSound();
                }
            }
        }
    }

    public void TurnOnInTutorial()
    {
        player.SetActive(true);
        havePlayer = true;
        gameObject.GetComponent<FaceScript>().rend.material = materialPlayerFace;
        siderRight.GetComponent<FaceScript>().rend.material = materialRightFace;
        sideLeft.GetComponent<FaceScript>().rend.material = materialLeftFace;
        sideTop.GetComponent<FaceScript>().rend.material = materialTopSide;
    }

    private void StartTransfer(GameObject targetSide, int sideNumber, string color)
    {
        transferInProgress = true;
        siderRight.GetComponent<FaceScript>().rend.material = materialBasicFace;
        sideLeft.GetComponent<FaceScript>().rend.material = materialBasicFace;
        sideTop.GetComponent<FaceScript>().rend.material = materialBasicFace;
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

    public void ReceivePlayer(GameObject newPlayer, int sideNumber, string color) //GameObject newPlayer, int sideNumber, string color)
    {

        rend.material = materialPlayerFace;

        materials.Remove("RightSide");
        materials.Remove("LeftSide");
        materials.Remove("TopSide");

        sides.Remove("RightSide");
        sides.Remove("LeftSide");
        sides.Remove("TopSide");

        if ((sideNumber == 1 && color == "Left") || (sideNumber == 2 && color == "Top") || (sideNumber == 3 && color == "Right"))
        {
            materials.Add("LeftSide", 2);
            materials.Add("RightSide", 3);
            materials.Add("TopSide", 1);

            sides.Add("LeftSide", siderRight);
            sides.Add("RightSide", sideTop);
            sides.Add("TopSide", sideLeft);

            siderRight.GetComponent<FaceScript>().rend.material = materialLeftFace;
            sideLeft.GetComponent<FaceScript>().rend.material = materialTopSide;
            sideTop.GetComponent<FaceScript>().rend.material = materialRightFace;
        }
        else if ((sideNumber == 1 && color == "Right") || (sideNumber == 2 && color == "Left") || (sideNumber == 3 && color == "Top"))
        {
            materials.Add("LeftSide", 1);
            materials.Add("RightSide", 2);
            materials.Add("TopSide", 3);

            sides.Add("LeftSide", sideLeft);
            sides.Add("RightSide", siderRight);
            sides.Add("TopSide", sideTop);

            siderRight.GetComponent<FaceScript>().rend.material = materialRightFace;
            sideLeft.GetComponent<FaceScript>().rend.material = materialLeftFace;
            sideTop.GetComponent<FaceScript>().rend.material = materialTopSide;
        }
        else if ((sideNumber == 1 && color == "Top") || (sideNumber == 2 && color == "Right") || (sideNumber == 3 && color == "Left"))
        {
            materials.Add("LeftSide", 3);
            materials.Add("RightSide", 1);
            materials.Add("TopSide", 2);

            sides.Add("LeftSide", sideTop);
            sides.Add("RightSide", sideLeft);
            sides.Add("TopSide", siderRight);

            sideLeft.GetComponent<FaceScript>().rend.material = materialRightFace;
            siderRight.GetComponent<FaceScript>().rend.material = materialTopSide;
            sideTop.GetComponent<FaceScript>().rend.material = materialLeftFace;
        }
        havePlayer = true;

        newPlayer.transform.SetParent(gameObject.transform);
        newPlayer.transform.localPosition = new Vector3(0, 0, 0);
        if (isTutorial)
        {
            newPlayer.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            newPlayer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
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
            return -1;
        }
    }
}