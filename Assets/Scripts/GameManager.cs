////////BATUHAN KANBUR - SPEEDRUNNER/////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //Oyunun genel gidi�at�n� kontrol eden GameManager scriptimde Singleton Design Patternine ba�vurdum b�ylece haberle�meyi tek bir noktada toplad�m.
    public ScoreManager _sM;
    public PoolMechanic _pM;
    public UserInterface _uM;
    public CharacterMovement _cM;

    public string _gameState = "Intro"; //Oyun durumu i�in basit bir string de�er atad�m.
    public GameObject _hitEffect; //Par�ac�k efektlerinin tek bir noktada basitce de�i�mesi i�in bunu GameManager scriptinden atad�m.
    public GameObject _scoreEffect;//Par�ac�k efektlerinin tek bir noktada basitce de�i�mesi i�in bunu GameManager scriptinden atad�m.


    Transform _charObject; 
    Transform _nearTileObject;
    Animator _charAnimator;
    float _gameTimer;
    private void Awake()
    {
        if (Instance != null && Instance != this) //GameManagerin haberle�ece�i b�t�n s�n�flar� de�i�kenlerime e�itledim.
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
        _nearTileObject = FindClosestTileByChar(); //ObjectPooling i�in en yak�n yol objecisi buldum.
        _charAnimator = _charObject.GetComponent<Animator>();
    }
    void Update()
    {
        switch (_gameState)
        {
            case "Intro":
                if (GetCurrentAnimatorName() == "Run") //Karakterin d�nme animasyonuna event vererek de bu i�lemi ger�ekle�tirebilridim ama biraz kolaya ka�t�m.
                {
                    _gameState = "Game";
                }
                break;
            case "Game":
                float _tileDist = Vector3.Distance(_nearTileObject.position, _charObject.transform.position);//En yak�n yol objesi ile karakterin mesafesini �l�t�m.
                if (_tileDist > 10f) //Karakter bulunan en yak�n yol objesinden uzakla�t��� takdirde object pool y�ntemi ile arkada kalan objenin tekrar �ne gelmesini sa�lad�m.
                {
                    GameObject _newRoadObject = _pM.GetGameObject(0);
                    _newRoadObject.transform.position = new Vector3(0, 0, _pM._lastRoadEndPoint);
                    _nearTileObject.gameObject.SetActive(false);//Art�k laz�m olmad��� i�in yol objesini devre d��� b�rakt�m.
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
        _cM._charSpeed = _sM._maxSpeed; //H�z� en y�ksek h�za e�itledim.
        _sM._currentHealth = _sM._totalHealth; //Hak say�s� de�i�ebilir oldu�u i�in ScoreManagerdeki say�ysa e�itledim.
        _uM.InstallHeartObjects(); //Oyundaki haklar�n g�rsel olarak olu�turmas� i�in UserInterface scriptime �a�r�da bulundum.
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
        Invoke("StartGame", 1); //InvokeRepeating korkutucu derecede performans etkilese de Invoke bizi �imdilik �ok �zmez :)
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
    public Transform FindClosestTileByChar() //Sahnedeki b�t�n Road tag�na sahip objeleri array�n i�ine alarak karaktere en yak�n olan�n� Transform olarak geri �evirdim.
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
    public string GetCurrentAnimatorName()//O anki aktif olan animasyonun ad�n� ��rendim.
    {
        AnimatorClipInfo[] animState = _charAnimator.GetCurrentAnimatorClipInfo(0);
        string currentName = animState[0].clip.name;
        return currentName;
    }
}
