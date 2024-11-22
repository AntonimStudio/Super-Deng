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
    public int pathObjectCount = -1;
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

    [HideInInspector] public FaceScript FS2;
    [HideInInspector] public FaceScript FS1;
    [HideInInspector] public FaceScript FS3;

    [Space]
    [Header("Materials")]
    [FormerlySerializedAs("materialWhite")]
    [SerializeField] private Material materialBasicFace;
    [FormerlySerializedAs("materialRed")]
    [SerializeField] private Material materialKillerFace;
    [FormerlySerializedAs("materialLightBlue")]
    [SerializeField] private Material materialPlayerFace;
    [FormerlySerializedAs("heheheheh")]
    [SerializeField] private Material materialSecretFace;
    [FormerlySerializedAs("materialBlue")]
    public Material materialRightFace;
    [FormerlySerializedAs("materialOrange")]
    public Material materialLeftFace;
    [FormerlySerializedAs("materialGreen")]
    public Material materialTopFace;
    public Dictionary<string, int> materials;

    [Space]
    [Header("Glowing&Rendering")]
    public MeshRenderer rend;
    public GameObject glowingPart;
    private Animator animator;

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
    [SerializeField] private PathCounterScript PCS;
    [SerializeField] private PortalSpawnerScript PSS;

    [Space]
    [Header("Questions")]
    public bool havePlayer = false;
    [SerializeField] private bool isAnExtremeSide = false;
    [SerializeField] private bool isTutorial = false;
    private bool transferInProgress = false;
    private bool isPreviousAnExtremeSide1 = false;
    public bool isKilling = false;
    public bool isBlinking = false;
    public bool isColored = false;
    public bool isBlocked = false;
    public bool isPortal = false;
    public bool isBonus = false;
    [HideInInspector] public bool isLeft = false;
    [HideInInspector] public bool isRight = false;
    [HideInInspector] public bool isTop = false;

    private void Awake() 
    {
        rend = glowingPart.GetComponent<MeshRenderer>();
        keyRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
        keyLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
        keyTop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));

        textRight.text = keyRight.ToString();
        textLeft.text = keyLeft.ToString();
        textTop.text = keyTop.ToString();

        if (havePlayer)
        {
            pathObjectCount = 0;
        }
        else pathObjectCount = -1;

        FS1 = side1.GetComponent<FaceScript>();
        FS2 = side2.GetComponent<FaceScript>();
        FS3 = side3.GetComponent<FaceScript>();
    }

    private void Start()
    {
        materials = new Dictionary<string, int>();
        sides = new Dictionary<string, GameObject>();

        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        if (havePlayer)
        {
            materials.Add("LeftSide", 1);
            materials.Add("RightSide", 2);
            materials.Add("TopSide", 3);

            sides.Add("LeftSide", side1);
            sides.Add("RightSide", side2);
            sides.Add("TopSide", side3);

            FS1.isLeft = true;
            FS2.isRight = true;
            FS3.isTop = true;
            /*        /\  
                     /  \
                    /  3 \
                   /______\
                  / \    / \
                 / 1 \  / 2 \
                /_____\/_____\
            */
            PS.SetCurrentFace(gameObject);

            if (TC != null && !TC._tutorialSettings[TC._index].isMoving)
            {
                player.SetActive(false);
                havePlayer = false;
            }
            else
            {
                //if (!isKilling) gameObject.GetComponent<FaceScript>().rend.material = materialPlayerFace;

                if (!FS2.isKilling) FS2.rend.material = materialRightFace;
                if (!FS1.isKilling) FS1.rend.material = materialLeftFace;
                if (!FS3.isKilling) FS3.rend.material = materialTopFace;
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
            HealthBonus bonusHealth = GetComponentInChildren<HealthBonus>(true);
            if (bonusHealth != null)
            {
                PS.TakeHP();
                isBonus = false;
                bonusHealth.DestroyMe();
            }
            ComboBonus bonusCombo = GetComponentInChildren<ComboBonus>(true);
            if (bonusCombo != null)
            {
                Debug.Log("COMBOTIME");
                BSS.GetComboBonus();
                isBonus = false;
                bonusCombo.DestroyMe();
            }
        }

        if (havePlayer && isPortal && PSS != null)
        {
            Portal portal = GetComponentInChildren<Portal>(true);
            isPortal = false;
            portal.DestroyMe();
            PSS.LoadSecretScene();  
        }
    }

    public void TurnOnInTutorial()
    {
        player.SetActive(true);
        havePlayer = true;
        //gameObject.GetComponent<FaceScript>().rend.material = materialPlayerFace;
        FS2.rend.material = materialRightFace;
        FS1.rend.material = materialLeftFace;
        FS3.rend.material = materialTopFace;
    }

    private void StartTransfer(GameObject targetSide, int sideNumber, string color)
    {
        transferInProgress = true;
        FS1.isLeft = false;
        FS1.isRight = false;
        FS1.isTop = false;

        FS2.isLeft = false;
        FS2.isRight = false;
        FS2.isTop = false;

        FS3.isLeft = false;
        FS3.isRight = false;
        FS3.isTop = false;

        if (FS1.isBonus) FS1.rend.material = materialPlayerFace;
        else if (FS1.isKilling) FS1.rend.material = materialKillerFace;
        else if (FS1.isPortal) FS1.rend.material = materialSecretFace;
        else FS1.rend.material = materialBasicFace;

        if (FS2.isBonus) FS2.rend.material = materialPlayerFace;
        else if (FS2.isKilling) FS2.rend.material = materialKillerFace;
        else if (FS2.isPortal) FS2.rend.material = materialSecretFace;
        else FS2.rend.material = materialBasicFace;

        if (FS3.isBonus) FS3.rend.material = materialPlayerFace;
        else if (FS3.isKilling) FS3.rend.material = materialKillerFace;
        else if (FS3.isPortal) FS3.rend.material = materialSecretFace;
        else FS3.rend.material = materialBasicFace;

        StartCoroutine(TransferPlayer(targetSide, sideNumber, color));
    }

    private IEnumerator TransferPlayer(GameObject targetSide, int sideNumber, string color)
    {
        yield return new WaitForSeconds(0.01f);
        FaceScript targetFace = targetSide.GetComponent<FaceScript>();
        if (!targetFace.havePlayer)
        {
            havePlayer = false;
            targetFace.ReceivePlayer(player, sideNumber, color, isAnExtremeSide, isPreviousAnExtremeSide1);
        }
        transferInProgress = false;
    }

    public void ReceivePlayer(GameObject newPlayer, int sideNumber, string color, bool isPreviousAnExtremeSide, bool isPreviousPreviousAnExtremeSide) //GameObject newPlayer, int sideNumber, string color)
    {
        //if (!isKilling) rend.material = materialPlayerFace;
        

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

                if (!FS2.isKilling) FS2.rend.material = materialLeftFace;
                if (!FS1.isKilling) FS1.rend.material = materialRightFace;
                if (!FS3.isKilling) FS3.rend.material = materialTopFace;

                FS2.isLeft = true;
                FS1.isRight = true;
                FS3.isTop = true;
            }
            else if (sideNumber == 2 && color == "Top")
            {
                materials.Add("LeftSide", 3);
                materials.Add("RightSide", 2);
                materials.Add("TopSide", 1);

                sides.Add("LeftSide", side3);
                sides.Add("RightSide", side2);
                sides.Add("TopSide", side1);

                if (!FS2.isKilling) FS2.rend.material = materialRightFace;
                if (!FS1.isKilling) FS1.rend.material = materialTopFace;
                if (!FS3.isKilling) FS3.rend.material = materialLeftFace;

                FS2.isRight = true;
                FS1.isTop = true;
                FS3.isLeft = true;
            }
            else if (sideNumber == 3 && color == "Right")
            {
                materials.Add("LeftSide", 1);
                materials.Add("RightSide", 3);
                materials.Add("TopSide", 2);

                sides.Add("LeftSide", side1);
                sides.Add("RightSide", side3);
                sides.Add("TopSide", side2);

                if (!FS2.isKilling) FS2.rend.material = materialTopFace;
                if (!FS1.isKilling) FS1.rend.material = materialLeftFace;
                if (!FS3.isKilling) FS3.rend.material = materialRightFace;

                FS2.isTop = true;
                FS1.isLeft = true;
                FS3.isRight = true;
            }
            else if (sideNumber == 1 && color == "Right")
            {
                materials.Add("LeftSide", 3);
                materials.Add("RightSide", 2);
                materials.Add("TopSide", 1);

                sides.Add("LeftSide", side3);
                sides.Add("RightSide", side2);
                sides.Add("TopSide", side1);

                if (!FS2.isKilling) FS2.rend.material = materialRightFace;
                if (!FS1.isKilling) FS1.rend.material = materialTopFace;
                if (!FS3.isKilling) FS3.rend.material = materialLeftFace;

                FS2.isRight = true;
                FS1.isTop = true;
                FS3.isLeft = true;
            }
            else if (sideNumber == 2 && color == "Left")
            {
                materials.Add("LeftSide", 1);
                materials.Add("RightSide", 3);
                materials.Add("TopSide", 2);

                sides.Add("LeftSide", side1);
                sides.Add("RightSide", side3);
                sides.Add("TopSide", side2);

                if (!FS2.isKilling) FS2.rend.material = materialTopFace;
                if (!FS1.isKilling) FS1.rend.material = materialLeftFace;
                if (!FS3.isKilling) FS3.rend.material = materialRightFace;

                FS2.isTop = true;
                FS1.isLeft = true;
                FS3.isRight = true;
            }
            else if (sideNumber == 3 && color == "Top")
            {
                materials.Add("LeftSide", 2);
                materials.Add("RightSide", 1);
                materials.Add("TopSide", 3);

                sides.Add("LeftSide", side2);
                sides.Add("RightSide", side1);
                sides.Add("TopSide", side3);

                if (!FS2.isKilling) FS2.rend.material = materialLeftFace;
                if (!FS1.isKilling) FS1.rend.material = materialRightFace;
                if (!FS3.isKilling) FS3.rend.material = materialTopFace;

                FS2.isLeft = true;
                FS1.isRight = true;
                FS3.isTop = true;
            }
            else if (sideNumber == 1 && color == "Top")
            {
                materials.Add("LeftSide", 1);
                materials.Add("RightSide", 3);
                materials.Add("TopSide", 2);

                sides.Add("LeftSide", side1);
                sides.Add("RightSide", side3);
                sides.Add("TopSide", side2);

                if (!FS2.isKilling) FS2.rend.material = materialTopFace;
                if (!FS1.isKilling) FS1.rend.material = materialLeftFace;
                if (!FS3.isKilling) FS3.rend.material = materialRightFace;

                FS2.isTop = true;
                FS1.isLeft = true;
                FS3.isRight = true;
            }
            else if (sideNumber == 2 && color == "Right")
            {
                materials.Add("LeftSide", 2);
                materials.Add("RightSide", 1);
                materials.Add("TopSide", 3);

                sides.Add("LeftSide", side2);
                sides.Add("RightSide", side1);
                sides.Add("TopSide", side3);

                if (!FS2.isKilling) FS2.rend.material = materialLeftFace;
                if (!FS1.isKilling) FS1.rend.material = materialRightFace;
                if (!FS3.isKilling) FS3.rend.material = materialTopFace;

                FS2.isLeft = true;
                FS1.isRight = true;
                FS3.isTop = true;
            }
            else if (sideNumber == 3 && color == "Left")
            {
                materials.Add("LeftSide", 3);
                materials.Add("RightSide", 2);
                materials.Add("TopSide", 1);

                sides.Add("LeftSide", side3);
                sides.Add("RightSide", side2);
                sides.Add("TopSide", side1);

                if (!FS2.isKilling) FS2.rend.material = materialRightFace;
                if (!FS1.isKilling) FS1.rend.material = materialTopFace;
                if (!FS3.isKilling) FS3.rend.material = materialLeftFace;

                FS2.isRight = true;
                FS1.isTop = true;
                FS3.isLeft = true;
            }
            isPreviousAnExtremeSide1 = true;
        }
        else
        {
            if ((sideNumber == 1 && color == "Left") || (sideNumber == 2 && color == "Top") || (sideNumber == 3 && color == "Right"))
            {
                materials.Add("LeftSide", 2);
                materials.Add("RightSide", 3);
                materials.Add("TopSide", 1);

                sides.Add("LeftSide", side2);
                sides.Add("RightSide", side3);
                sides.Add("TopSide", side1);

                if (!FS2.isKilling) FS2.rend.material = materialLeftFace;
                if (!FS1.isKilling) FS1.rend.material = materialTopFace;
                if (!FS3.isKilling) FS3.rend.material = materialRightFace;

                FS2.isLeft = true;
                FS1.isTop = true;
                FS3.isRight = true;
            }
            else if ((sideNumber == 1 && color == "Right") || (sideNumber == 2 && color == "Left") || (sideNumber == 3 && color == "Top"))
            {
                materials.Add("LeftSide", 1);
                materials.Add("RightSide", 2);
                materials.Add("TopSide", 3);

                sides.Add("LeftSide", side1);
                sides.Add("RightSide", side2);
                sides.Add("TopSide", side3);

                if (!FS2.isKilling) FS2.rend.material = materialRightFace;
                if (!FS1.isKilling) FS1.rend.material = materialLeftFace;
                if (!FS3.isKilling) FS3.rend.material = materialTopFace;

                FS2.isRight = true;
                FS1.isLeft = true;
                FS3.isTop = true;
            }
            else if ((sideNumber == 1 && color == "Top") || (sideNumber == 2 && color == "Right") || (sideNumber == 3 && color == "Left"))
            {
                materials.Add("LeftSide", 3);
                materials.Add("RightSide", 1);
                materials.Add("TopSide", 2);

                sides.Add("LeftSide", side3);
                sides.Add("RightSide", side1);
                sides.Add("TopSide", side2);

                if (!FS2.isKilling) FS2.rend.material = materialTopFace;
                if (!FS1.isKilling) FS1.rend.material = materialRightFace;
                if (!FS3.isKilling) FS3.rend.material = materialLeftFace;

                FS2.isTop = true;
                FS1.isRight = true;
                FS3.isLeft = true;
            }
            isPreviousAnExtremeSide1 = false;
        }
        havePlayer = true;
        PS.ResetMaterials();
        PCS.SetPathCount();
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

    public void ResetRightLeftTop()
    {
        if (isLeft) rend.material = materialLeftFace;
        else if (isRight) rend.material = materialRightFace;
        else if (isTop) rend.material = materialTopFace;
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