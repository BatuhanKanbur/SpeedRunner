using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float[] _charHorizontalPos = new float[3] { -1.5f, 0, 1.5f }; //Karakterin X koordinat�nda bulunabilece�i pozisyonlar� array i�inde �nceden haz�rlad�m.
    Animator _charAnimator; //Karakter animat�r�n� tan�mlad�m
    Rigidbody _charRigid; //Karakterin z�plamas� i�in laz�m olan komponenti tan�mlad�m.
    CapsuleCollider _charColl;
    public float _charSpeed = 0.5f; //Zamanla ya da oyun ayarlar�ndan ayarlanabilir olmas� i�in karakterin h�z�n� de�i�kene atad�m.
    int _currentHPos = 1;
    void Start()
    {
        _charAnimator = GetComponent<Animator>(); //S�rekli GetComponent kullanmamak ad�na tek bir yerde toplad�m hepsini.
        _charRigid = GetComponent<Rigidbody>();
        _charColl = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (GameManager.Instance._gameState != "Game") //Oyun durumu game de�ilse return ile geri �evirdim.
            return;
        float _charXPos= Mathf.Lerp(transform.position.x, _charHorizontalPos[_currentHPos], Time.deltaTime * 5f); //Lerp fonsiyonu ile karakterin yumu�ak X koordinat� de�i�imlerini yapt�m.
        transform.position = new Vector3(_charXPos, transform.position.y, transform.position.z); 
        transform.Translate(0, 0, _charSpeed * Time.deltaTime * 5f);//Oyun �ok fazla ve detayl� fizik gerektirmedi�i i�in basit bir ileri komutu yazd�m.
        string _animName = GameManager.Instance.GetCurrentAnimatorName(); // Karakterin animat�r�ne eri�ip eventler i�in animasyonun ismin tek seferde ald�m.(GarbageCollector tasarrufu i�in bu i�lemleri tek seferde yapmal�y�z)
        if (_animName == "Run")
        {
            _charSpeed = Mathf.Lerp(_charSpeed, GameManager.Instance._sM._maxSpeed, Time.deltaTime * 1.25f); //Yumu�ak bir ge�i� i�in karakterin h�z�n� olabilecek maks h�za e�itledim.
        }
        if (_animName == "Slide") //Karakterin yerden kayd���nda olmas� gereken fiziksel etkile�imler i�in colliderinde �e�itli de�i�iklikler yapt�m.
        {
            _charColl.height = 0.5f;
            _charColl.center = new Vector3(0, 0.25f, 0);
        }
        else
        {
            _charColl.height = 1.5f;
            _charColl.center = new Vector3(0, 0.75f, 0);
        }
        _charAnimator.SetFloat("Speed", _charSpeed); //Karakter �ok h�zland���nda animasyonda senkron sorunu olmamas� i�in karakterin h�z�n� animasyon h�z�na e�itledim.
    }
    private void OnCollisionEnter(Collision _col) //Fizik motorunu k���kte olsa kullanmak ad�na �arp��malar� OnCollisionEnter ile yapt�m, bu gibi durumlarda Ray kullanmak her zaman tasarruflu olmuyor.
    {
        if (_col.transform.tag == "Block")
        {
            Vector3 _spawmPos = _col.transform.position;
            _spawmPos.y += 2;
            _charAnimator.SetTrigger("Hit");
            _charSpeed = 0.1f;
            _col.transform.parent.gameObject.SetActive(false);
            GameManager.Instance._sM.HitDamage();
            GameManager.Instance.SpawmHitEffect(_spawmPos);
        }
        if (_col.transform.name == "Ground")
        {
            for (int i = 1; i < 3; i++)
            {
                GameObject _newObj = GameManager.Instance._pM.GetGameObject(1);
                _newObj.transform.position = new Vector3(_charHorizontalPos[Random.Range(0, _charHorizontalPos.Length)], 1, transform.position.z + (30 * i));
            }
        }
        if (_col.transform.tag == "Point")
        {
            _col.transform.gameObject.SetActive(false);
            GameManager.Instance._sM.AddScore();
            GameManager.Instance.SpawmScoreEffect(_col.transform.position);
        }
    }
    public void MovementControl(string _dir)//MouseController scriptimin karakterime yollad��� eventleri tek �at� alt�nda toplad�m.
    {
        if (_charSpeed < 0.25f || GameManager.Instance._gameState!="Game" || GameManager.Instance.GetCurrentAnimatorName()!="Run")
            return;
        switch (_dir)
        {
            case "R":
                _currentHPos++;
                break;
            case "L":
                _currentHPos--;
                break;
            case "U":
                _charRigid.AddForce(Vector3.up * 225);
                _charAnimator.SetTrigger("Jump");
                break;
            case "D":
                _charAnimator.SetTrigger("Slider");
                break;
        }
        _currentHPos = Mathf.Clamp(_currentHPos, 0, 2);
    }
 
}
