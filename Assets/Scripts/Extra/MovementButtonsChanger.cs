using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Linq;

public class MovementButtonsChanger : MonoBehaviour
{
    [SerializeField] private Button buttonRight;
    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonTop;

    [SerializeField] private TextMeshProUGUI buttonRightText;
    [SerializeField] private TextMeshProUGUI buttonLeftText;
    [SerializeField] private TextMeshProUGUI buttonTopText;

    [SerializeField] private Image buttonRightImage;
    [SerializeField] private Image buttonLeftImage;
    [SerializeField] private Image buttonTopImage;

    [SerializeField] private AudioSource errorSound; // Звук ошибки

    private int currentButtonIndex = -1; // Индекс текущей выбранной кнопки

    private KeyCode rightKey = KeyCode.D;
    private KeyCode leftKey = KeyCode.A;
    private KeyCode topKey = KeyCode.W;

    private void Start()
    {
        buttonRightText.text = rightKey.ToString();
        buttonLeftText.text = leftKey.ToString();
        buttonTopText.text = topKey.ToString();

        buttonRight.onClick.AddListener(() => OnButtonClick(0));
        buttonLeft.onClick.AddListener(() => OnButtonClick(1));
        buttonTop.onClick.AddListener(() => OnButtonClick(2));

        LoadSettings();
    }

    private void Update()
    {
        if (currentButtonIndex != -1)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode) && IsValidKey(keyCode))
                    {
                        if (!IsKeyAlreadyAssigned(keyCode)) // Проверка, занята ли клавиша
                        {
                            UpdateButtonTextAndImage(currentButtonIndex, keyCode);
                            currentButtonIndex = -1; // Сброс текущей выбранной кнопки
                            SetButtonsInteractable(true); // Включаем все кнопки
                        }
                        else
                        {
                            errorSound.Play(); // Воспроизводим звук ошибки
                        }
                        break;
                    }
                }
            }
        }
    }

    private void OnButtonClick(int index)
    {
        if (currentButtonIndex == -1) // Если никакая кнопка не выбрана
        {
            SetButtonsInteractable(false); // Выключаем другие кнопки

            if (index == 0)
            {
                buttonRightText.text = "Press";
                buttonRightImage.enabled = false;
            }
            else if (index == 1)
            {
                buttonLeftText.text = "Press";
                buttonLeftImage.enabled = false;
            }
            else if (index == 2)
            {
                buttonTopText.text = "Press";
                buttonTopImage.enabled = false;
            }
            currentButtonIndex = index; // Запоминаем индекс выбранной кнопки
        }
    }

    private void UpdateButtonTextAndImage(int index, KeyCode newKey)
    {
        if (index == 0)
        {
            rightKey = newKey;
            buttonRightText.text = newKey.ToString();
            buttonRightImage.enabled = true;
        }
        else if (index == 1)
        {
            leftKey = newKey;
            buttonLeftText.text = newKey.ToString();
            buttonLeftImage.enabled = true;
        }
        else if (index == 2)
        {
            topKey = newKey;
            buttonTopText.text = newKey.ToString();
            buttonTopImage.enabled = true;
        }
    }

    private bool IsValidKey(KeyCode keyCode)
    {
        return (keyCode >= KeyCode.A && keyCode <= KeyCode.Z) ||
               (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9) ||
               (keyCode >= KeyCode.Keypad0 && keyCode <= KeyCode.Keypad9) ||
               (keyCode == KeyCode.LeftArrow || keyCode == KeyCode.RightArrow || keyCode == KeyCode.UpArrow || keyCode == KeyCode.DownArrow);
    }

    private bool IsKeyAlreadyAssigned(KeyCode keyCode)
    {
        if (currentButtonIndex == 0)
        {
            return keyCode == leftKey || keyCode == topKey;
        }
        else if (currentButtonIndex == 1)
        {
            return keyCode == rightKey || keyCode == topKey;
        }
        else
        {
            return keyCode == rightKey || keyCode == leftKey;
        }
    }

    // Функция для управления состоянием всех кнопок
    private void SetButtonsInteractable(bool interactable)
    {
        buttonRight.interactable = interactable || currentButtonIndex == 0;
        buttonLeft.interactable = interactable || currentButtonIndex == 1;
        buttonTop.interactable = interactable || currentButtonIndex == 2;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetString("RightButtonSymbol", rightKey.ToString());
        PlayerPrefs.SetString("LeftButtonSymbol", leftKey.ToString());
        PlayerPrefs.SetString("TopButtonSymbol", topKey.ToString());
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("RightButtonSymbol"))
        {
            KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
            UpdateButtonTextAndImage(0, keyCode);
        }
        else
        {
            UpdateButtonTextAndImage(0, KeyCode.D);
        }

        if (PlayerPrefs.HasKey("LeftButtonSymbol"))
        {
            KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
            UpdateButtonTextAndImage(1, keyCode);
        }
        else
        {
            UpdateButtonTextAndImage(1, KeyCode.A);
        }

        if (PlayerPrefs.HasKey("TopButtonSymbol"))
        {
            KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));
            UpdateButtonTextAndImage(2, keyCode);
        }
        else
        {
            UpdateButtonTextAndImage(2, KeyCode.W);
        }
    }
}
