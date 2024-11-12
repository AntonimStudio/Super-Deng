using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.XR;

// Обязательное уведомление: "Правые", "Левые" и "Верхние" указаны для треугольника с основанием, направленным ВНИЗ!!!
// Обратите внимание, что "Правая" сторона раньше носила название "BlueSide", "Левая" - "OrangeSide", а "Верхняя" - "GreenSide"
// Помимо прочих наименований, "Правая" сторона может записываться как "Side2", "Левая" - "Side1", а "Верхняя" - "Side3"
public class FaceScript : MonoBehaviour 
{
    /*                /\  
                     /  \
                    /  3 \
                   /______\
                  / \    / \
                 / 1 \  / 2 \
                /_____\/_____\
    */
    public GameObject player;
    [SerializeField] private PlayerScript PS;

    [Space]
    [Header("Sides of the Face")]
    [FormerlySerializedAs("sideBlue")]
    [SerializeField] private GameObject side2; // BlueSide == Side2
    [FormerlySerializedAs("sideOrange")]
    [SerializeField] private GameObject side1; // OrangeSide == Side1
    [FormerlySerializedAs("sideGreen")]
    [SerializeField] private GameObject side3; // GreenSide == Side3
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
    [SerializeField] private Material materialTopFace;
    public Dictionary<string, int> materials;

    [Space]
    [Header("Glowing&Rendering")]
    public MeshRenderer rend;
    public GameObject glowingPart;

    [Header("Key Bindings")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyTop = KeyCode.W;
    public KeyCode keyRight = KeyCode.D;
    [SerializeField] private TextMeshProUGUI textTop;
    [SerializeField] private TextMeshProUGUI textRight;
    [SerializeField] private TextMeshProUGUI textLeft;

    [Space]
    [Header("Scene ScriptManagers")]
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private BeatController BC;
    [SerializeField] private SoundScript SS;
    [SerializeField] private TutorialController TC;
    [SerializeField] private BonusSpawnerScript BSS;

    [Space]
    [Header("Questions")]
    public bool havePlayer = false;
    [SerializeField] private bool isAnExtremeSide = false;
    [SerializeField] private bool isTutorial = false;
    private bool transferInProgress = false;
    private bool isPreviousAnExtremeSide1 = false;
    [HideInInspector] public bool isKilling = false;
    [HideInInspector] public bool isBlinking = false;
    [HideInInspector] public bool isBlocked = false;
    [HideInInspector] public bool isBonus = false;

    private void Awake() 
    {
        rend = glowingPart.GetComponent<MeshRenderer>();
        keyRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
        keyLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
        keyTop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));

        textRight.text = keyRight.ToString();
        textLeft.text = keyLeft.ToString();
        textTop.text = keyTop.ToString();
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

            sides.Add("LeftSide", side1);
            sides.Add("RightSide", side2);
            sides.Add("TopSide", side3);
            /*        /\  
                     /  \
                    /  3 \
                   /______\
                  / \    / \
                 / 1 \  / 2 \
                /_____\/_____\
            */
            if (TC != null && !TC._tutorialSettings[TC._index].isMoving)
            {
                player.SetActive(false);
                havePlayer = false;
            }
            else
            {
                gameObject.GetComponent<FaceScript>().rend.material = materialPlayerFace;
                side2.GetComponent<FaceScript>().rend.material = materialRightFace;
                side1.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side3.GetComponent<FaceScript>().rend.material = materialTopFace;
            }
        }
        PS.SetCurrentFace(gameObject);
    }

    private void Update()
    {
        if (havePlayer && !transferInProgress && (SCD == null || SCD.isOn) && BC.canPress)
        {
            if (Input.GetKeyDown(keyLeft) || Input.GetKeyDown(keyTop) || Input.GetKeyDown(keyRight))
            {
                string direction = "";
                if (Input.GetKeyDown(keyLeft) && !GetGameObject("LeftSide").GetComponent<FaceScript>().isBlocked
                        && !Input.GetKey(keyTop) && !Input.GetKey(keyRight)) direction = "Left";
                else if (GetGameObject("LeftSide").GetComponent<FaceScript>().isBlocked)
                {
                    SS.TurnOnSoundBlock();
                }

                if (Input.GetKeyDown(keyTop) && !GetGameObject("TopSide").GetComponent<FaceScript>().isBlocked
                    && !Input.GetKey(keyLeft) && !Input.GetKey(keyRight)) direction = "Top";
                else if (GetGameObject("TopSide").GetComponent<FaceScript>().isBlocked)
                {
                    SS.TurnOnSoundBlock();
                }

                if (Input.GetKeyDown(keyRight) && !GetGameObject("RightSide").GetComponent<FaceScript>().isBlocked
                    && !Input.GetKey(keyTop) && !Input.GetKey(keyLeft)) direction = "Right";
                else if (GetGameObject("RightSide").GetComponent<FaceScript>().isBlocked)
                {
                    SS.TurnOnSoundBlock();
                }

                if (!string.IsNullOrEmpty(direction))
                {
                    StartTransfer(GetGameObject($"{direction}Side"), GetInt($"{direction}Side"), direction);
                    BC.isAlreadyPressed = true;
                    BC.isAlreadyPressedIsAlreadyPressed = false;
                    SS.TurnOnSoundStep();
                }
            }
        }

        if (havePlayer && isBonus && BSS!= null)
        {
            Debug.Log("111");
            HealthBonus bonusHealth = GetComponentInChildren<HealthBonus>(true);
            if (bonusHealth != null)
            {
                Debug.Log("!!");
                PS.TakeHP();
                isBonus = false;
                bonusHealth.DestroyMe();
            }
            ComboBonus bonusCombo = GetComponentInChildren<ComboBonus>(true);
            if (bonusCombo != null)
            {
                Debug.Log("COMBOTIME");
                isBonus = false;
                bonusCombo.DestroyMe();
            }
        }
    }

    public void TurnOnInTutorial()
    {
        player.SetActive(true);
        havePlayer = true;
        gameObject.GetComponent<FaceScript>().rend.material = materialPlayerFace;
        side2.GetComponent<FaceScript>().rend.material = materialRightFace;
        side1.GetComponent<FaceScript>().rend.material = materialLeftFace;
        side3.GetComponent<FaceScript>().rend.material = materialTopFace;
    }

    private void StartTransfer(GameObject targetSide, int sideNumber, string color)
    {
        transferInProgress = true;
        side2.GetComponent<FaceScript>().rend.material = materialBasicFace;
        side1.GetComponent<FaceScript>().rend.material = materialBasicFace;
        side3.GetComponent<FaceScript>().rend.material = materialBasicFace;
        StartCoroutine(TransferPlayer(targetSide, sideNumber, color));
    }

    private IEnumerator TransferPlayer(GameObject targetSide, int sideNumber, string color)
    {
        yield return new WaitForSeconds(0.01f);
        FaceScript targetFace = targetSide.GetComponent<FaceScript>();
        if (!targetFace.havePlayer)
        {
            targetFace.ReceivePlayer(player, sideNumber, color, isAnExtremeSide, isPreviousAnExtremeSide1);
            havePlayer = false;
        }
        transferInProgress = false;
    }

    public void ReceivePlayer(GameObject newPlayer, int sideNumber, string color, bool isPreviousAnExtremeSide, bool isPreviousPreviousAnExtremeSide) //GameObject newPlayer, int sideNumber, string color)
    {
        rend.material = materialPlayerFace;

        materials.Remove("RightSide");
        materials.Remove("LeftSide");
        materials.Remove("TopSide");

        sides.Remove("RightSide");
        sides.Remove("LeftSide");
        sides.Remove("TopSide");
        /*            /\  
                     /  \
                    /  3 \
                   /______\
                  / \    / \
                 / 1 \  / 2 \
                /_____\/_____\
        */
        
        if (!isPreviousPreviousAnExtremeSide && isPreviousAnExtremeSide && isAnExtremeSide)
        {
            if (sideNumber == 1 && color == "Left")
            {
                materials.Add("LeftSide", 2);
                materials.Add("RightSide", 1);
                materials.Add("TopSide", 3);

                sides.Add("LeftSide", side2);
                sides.Add("RightSide", side1);
                sides.Add("TopSide", side3);

                side2.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side1.GetComponent<FaceScript>().rend.material = materialRightFace;
                side3.GetComponent<FaceScript>().rend.material = materialTopFace;
            }
            else if (sideNumber == 2 && color == "Top")
            {
                materials.Add("LeftSide", 3);
                materials.Add("RightSide", 2);
                materials.Add("TopSide", 1);

                sides.Add("LeftSide", side3);
                sides.Add("RightSide", side2);
                sides.Add("TopSide", side1);

                side2.GetComponent<FaceScript>().rend.material = materialRightFace;
                side1.GetComponent<FaceScript>().rend.material = materialTopFace;
                side3.GetComponent<FaceScript>().rend.material = materialLeftFace;
            }
            else if (sideNumber == 3 && color == "Right")
            {
                materials.Add("LeftSide", 1);
                materials.Add("RightSide", 3);
                materials.Add("TopSide", 2);

                sides.Add("LeftSide", side1);
                sides.Add("RightSide", side3);
                sides.Add("TopSide", side2);

                side2.GetComponent<FaceScript>().rend.material = materialTopFace;
                side1.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side3.GetComponent<FaceScript>().rend.material = materialRightFace;
            }
            else if (sideNumber == 1 && color == "Right")
            {
                materials.Add("LeftSide", 3);
                materials.Add("RightSide", 2);
                materials.Add("TopSide", 1);

                sides.Add("LeftSide", side3);
                sides.Add("RightSide", side2);
                sides.Add("TopSide", side1);

                side2.GetComponent<FaceScript>().rend.material = materialRightFace;
                side1.GetComponent<FaceScript>().rend.material = materialTopFace;
                side3.GetComponent<FaceScript>().rend.material = materialLeftFace;
            }
            else if (sideNumber == 2 && color == "Left")
            {
                materials.Add("LeftSide", 1);
                materials.Add("RightSide", 3);
                materials.Add("TopSide", 2);

                sides.Add("LeftSide", side1);
                sides.Add("RightSide", side3);
                sides.Add("TopSide", side2);

                side2.GetComponent<FaceScript>().rend.material = materialTopFace;
                side1.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side3.GetComponent<FaceScript>().rend.material = materialRightFace;
            }
            else if (sideNumber == 3 && color == "Top")
            {
                materials.Add("LeftSide", 2);
                materials.Add("RightSide", 1);
                materials.Add("TopSide", 3);

                sides.Add("LeftSide", side2);
                sides.Add("RightSide", side1);
                sides.Add("TopSide", side3);

                side2.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side1.GetComponent<FaceScript>().rend.material = materialRightFace;
                side3.GetComponent<FaceScript>().rend.material = materialTopFace;
            }
            else if (sideNumber == 1 && color == "Top")
            {
                materials.Add("LeftSide", 1);
                materials.Add("RightSide", 3);
                materials.Add("TopSide", 2);

                sides.Add("LeftSide", side1);
                sides.Add("RightSide", side3);
                sides.Add("TopSide", side2);

                side1.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side2.GetComponent<FaceScript>().rend.material = materialTopFace;
                side3.GetComponent<FaceScript>().rend.material = materialRightFace;
            }
            else if (sideNumber == 2 && color == "Right")
            {
                materials.Add("LeftSide", 2);
                materials.Add("RightSide", 1);
                materials.Add("TopSide", 3);

                sides.Add("LeftSide", side2);
                sides.Add("RightSide", side1);
                sides.Add("TopSide", side3);

                side1.GetComponent<FaceScript>().rend.material = materialRightFace;
                side2.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side3.GetComponent<FaceScript>().rend.material = materialTopFace;
            }
            else if (sideNumber == 3 && color == "Left")
            {
                materials.Add("LeftSide", 3);
                materials.Add("RightSide", 2);
                materials.Add("TopSide", 1);

                sides.Add("LeftSide", side3);
                sides.Add("RightSide", side2);
                sides.Add("TopSide", side1);

                side1.GetComponent<FaceScript>().rend.material = materialTopFace;
                side2.GetComponent<FaceScript>().rend.material = materialRightFace;
                side3.GetComponent<FaceScript>().rend.material = materialLeftFace;
            }
            isPreviousAnExtremeSide1 = true;
        }
        else
        {
            /*        /\  
                     /  \
                    /  3 \
                   /______\
                  / \    / \
                 / 1 \  / 2 \
                /_____\/_____\
            */
            if ((sideNumber == 1 && color == "Left") || (sideNumber == 2 && color == "Top") || (sideNumber == 3 && color == "Right"))
            {
                materials.Add("LeftSide", 2);
                materials.Add("RightSide", 3);
                materials.Add("TopSide", 1);

                sides.Add("LeftSide", side2);
                sides.Add("RightSide", side3);
                sides.Add("TopSide", side1);

                side2.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side1.GetComponent<FaceScript>().rend.material = materialTopFace;
                side3.GetComponent<FaceScript>().rend.material = materialRightFace;
            }
            else if ((sideNumber == 1 && color == "Right") || (sideNumber == 2 && color == "Left") || (sideNumber == 3 && color == "Top"))
            {
                materials.Add("LeftSide", 1);
                materials.Add("RightSide", 2);
                materials.Add("TopSide", 3);

                sides.Add("LeftSide", side1);
                sides.Add("RightSide", side2);
                sides.Add("TopSide", side3);

                side2.GetComponent<FaceScript>().rend.material = materialRightFace;
                side1.GetComponent<FaceScript>().rend.material = materialLeftFace;
                side3.GetComponent<FaceScript>().rend.material = materialTopFace;
            }
            else if ((sideNumber == 1 && color == "Top") || (sideNumber == 2 && color == "Right") || (sideNumber == 3 && color == "Left"))
            {
                materials.Add("LeftSide", 3);
                materials.Add("RightSide", 1);
                materials.Add("TopSide", 2);

                sides.Add("LeftSide", side3);
                sides.Add("RightSide", side1);
                sides.Add("TopSide", side2);

                side1.GetComponent<FaceScript>().rend.material = materialRightFace;
                side2.GetComponent<FaceScript>().rend.material = materialTopFace;
                side3.GetComponent<FaceScript>().rend.material = materialLeftFace;
            }
            isPreviousAnExtremeSide1 = false;
        }
        havePlayer = true;
        PS.SetCurrentFace(gameObject);
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