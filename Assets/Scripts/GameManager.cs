////////BATUHAN KANBUR - SPEEDRUNNER/////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //Oyunun genel gidiþatýný kontrol eden GameManager scriptimde Singleton Design Patternine baþvurdum böylece haberleþmeyi tek bir noktada topladým.
    public ScoreManager _sM;
    public PoolMechanic _pM;
    public UserInterface _uM;
    public CharacterMovement _cM;

    public string _gameState = "Intro"; //Oyun durumu için basit bir string deðer atadým.
    public GameObject _hitEffect; //Parçacýk efektlerinin tek bir noktada basitce deðiþmesi için bunu GameManager scriptinden atadým.
    public GameObject _scoreEffect;//Parçacýk efektlerinin tek bir noktada basitce deðiþmesi için bunu GameManager scriptinden atadým.


    Transform _charObject; 
    Transform _nearTileObject;
    Animator _charAnimator;
    float _gameTimer;
    private void Awake()
    {
        if (Instance != null && Instance != this) //GameManagerin haberleþeceði bütün sýnýflarý deðiþkenlerime eþitledim.
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _sM = FindObjectOfType<ScoreManager>();
        _uM = FindObjectOfType<UserInterface>();
        _cM = FindObjectOfType<CharacterMovement>();
        _charObject = _cM.transform;
    }
    void Start()
    {
        _nearTileObject = FindClosestTileByChar(); //ObjectPooling için en yakýn yol objecisi buldum.
        _charAnimator = _charObject.GetComponent<Animator>();
    }
    void Update()
    {
        switch (_gameState)
        {
            case "Intro":
                if (GetCurrentAnimatorName() == "Run") //Karakterin dönme animasyonuna event vererek de bu iþlemi gerçekleþtirebilridim ama biraz kolaya kaçtým.
                {
                    _gameState = "Game";
                }
                break;
            case "Game":
                float _tileDist = Vector3.Distance(_nearTileObject.position, _charObject.transform.position);//En yakýn yol objesi ile karakterin mesafesini ölçtüm.
                if (_tileDist > 10f) //Karakter bulunan en yakýn yol objesinden uzaklaþtýðý takdirde object pool yöntemi ile arkada kalan objenin tekrar öne gelmesini saðladým.
                {
                    GameObject _newRoadObject = _pM.GetGameObject(0);
                    _newRoadObject.transform.position = new Vector3(0, 0, _pM._lastRoadEndPoint);
                    _nearTileObject.gameObject.SetActive(false);//Artýk lazým olmadýðý için yol objesini devre dýþý býraktým.
                    _nearTileObject = FindClosestTileByChar();
                }
                _gameTimer += Time.deltaTime;
                if (_gameTimer > 45)
                {
                    _gameState = "Win";
                    _charAnimator.SetInteger("State", 3);
                    _sM.FinishLevel();
                }
                if (_sM._currentHealth <= 0)
                {
                    _gameState = "Lose";
                    _charAnimator.SetInteger("State", 2);
                }
                break;
        }
    }
    public void StartGame()
    {
        _cM._charSpeed = _sM._maxSpeed; //Hýzý en yüksek hýza eþitledim.
        _sM._currentHealth = _sM._totalHealth; //Hak sayýsý deðiþebilir olduðu için ScoreManagerdeki sayýysa eþitledim.
        _uM.InstallHeartObjects(); //Oyundaki haklarýn görsel olarak oluþturmasý için UserInterface scriptime çaðrýda bulundum.
        _gameTimer = 0;
        _gameState = "Intro";
        _charAnimator.SetInteger("State", 1);
    }
    public void Restart_Button()
    {
        _gameState = "Intro";
        _charAnimator.SetInteger("State", 0);
        _sM._currentScore = 0;
        _sM._currentHealth = _sM._totalHealth;
        Invoke("StartGame", 1); //InvokeRepeating korkutucu derecede performans etkilese de Invoke bizi þimdilik çok üzmez :)
    }
    public void Resume_Button()
    {
        _gameState = "Intro";
        _charAnimator.SetInteger("State", 0);
        Invoke("StartGame", 1);
    }
    public void BackMenu_Button()
    {
        _charAnimator.SetInteger("State", 0);
        _gameState = "Intro";
    }
    public void SpawmHitEffect(Vector3 _effectPos)
    {
        GameObject _newHitEffect = Instantiate(_hitEffect, _effectPos, Quaternion.identity);
        Destroy(_newHitEffect, 5f);
    }
    public void SpawmScoreEffect(Vector3 _effectPos)
    {
        GameObject _newScoreEffect = Instantiate(_scoreEffect, _effectPos, Quaternion.identity);
        Destroy(_newScoreEffect, 5f);
    }
    public Transform FindClosestTileByChar() //Sahnedeki bütün Road tagýna sahip objeleri arrayýn içine alarak karaktere en yakýn olanýný Transform olarak geri çevirdim.
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Road");
        GameObject _closetTile = null;
        float _closetDist = Mathf.Infinity;
        Vector3 _charPos = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 _closetDiff = go.transform.position - _charPos;
            float curDistance = _closetDiff.sqrMagnitude;
            if (curDistance < _closetDist)
            {
                _closetTile = go;
                _closetDist = curDistance;
            }
        }
        return _closetTile.transform;
    }
    public string GetCurrentAnimatorName()//O anki aktif olan animasyonun adýný öðrendim.
    {
        AnimatorClipInfo[] animState = _charAnimator.GetCurrentAnimatorClipInfo(0);
        string currentName = animState[0].clip.name;
        return currentName;
    }
}
