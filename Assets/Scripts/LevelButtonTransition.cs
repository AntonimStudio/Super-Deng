using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelButtonTransition : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private bool _isRight;
    [SerializeField] private List<Transform> _points;
    public List<Transform> _screens;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _wall;
    [SerializeField] private  LevelButtonTransition BSM;
    [SerializeField] private float _epsilion;
    private bool _isRightClicked;
    private bool _isLeftClicked;
    public int _numberMenu;


    private void Start()
    {
        _numberMenu = 0;
        _isRightClicked = false;
        _isLeftClicked = false;
        _wall.SetActive(false);
    }

    private void Update()
    {
        if (_isRightClicked && _numberMenu < _screens.Count - 1)
        {
            _wall.SetActive(true);
            if (_numberMenu > 0)
            {
                _screens[_numberMenu - 1].transform.position = Vector3.MoveTowards(_screens[_numberMenu - 1].transform.position, _points[0].position, Time.deltaTime * _speed);
            }
            _screens[_numberMenu].transform.position = Vector3.MoveTowards(_screens[_numberMenu].transform.position, _points[1].position, Time.deltaTime * _speed);
            _screens[_numberMenu + 1].transform.position = Vector3.MoveTowards(_screens[_numberMenu + 1].transform.position, _points[2].position, Time.deltaTime * _speed);
            if (_numberMenu < _screens.Count - 2)
            {
                _screens[_numberMenu + 2].transform.position = Vector3.MoveTowards(_screens[_numberMenu + 2].transform.position, _points[3].position, Time.deltaTime * _speed);
            }

            if (Mathf.Abs(_screens[_numberMenu].transform.position.x - _points[1].position.x) <= _epsilion)
            {
                _numberMenu += 1;
                BSM.ChangeNumberMenu(_numberMenu);
                Debug.Log(_numberMenu);
                _isRightClicked = false;
                _isLeftClicked = false;
                _wall.SetActive(false);
            }
        }
        else { _isRightClicked = false; }

        if (_isLeftClicked && _numberMenu > 0)
        {
            _wall.SetActive(true);
            if (_numberMenu < _screens.Count-1)
            {
                _screens[_numberMenu + 1].transform.position = Vector3.MoveTowards(_screens[_numberMenu + 1].transform.position, _points[4].position, Time.deltaTime * _speed);
            }
            _screens[_numberMenu].transform.position = Vector3.MoveTowards(_screens[_numberMenu].transform.position, _points[3].position, Time.deltaTime * _speed);
            _screens[_numberMenu - 1].transform.position = Vector3.MoveTowards(_screens[_numberMenu - 1].transform.position, _points[2].position, Time.deltaTime * _speed);
            if (_numberMenu > 1)
            {
                _screens[_numberMenu - 2].transform.position = Vector3.MoveTowards(_screens[_numberMenu - 2].transform.position, _points[1].position, Time.deltaTime * _speed);
            }
            if (Mathf.Abs(_screens[_numberMenu].transform.position.x - _points[3].position.x) <= _epsilion)
            {
                _numberMenu -= 1;
                BSM.ChangeNumberMenu(_numberMenu);
                Debug.Log(_numberMenu);
                _isLeftClicked = false;
                _isRightClicked = false;
                _wall.SetActive(false);
            }
        }
        else { _isLeftClicked = false; }

    }

    public void ChangeNumberMenu(int num)
    {
        _numberMenu = num;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (_isRight) { _isRightClicked = true; }
        else { _isLeftClicked = true; }
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }
}