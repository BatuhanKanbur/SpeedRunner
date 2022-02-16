using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float[] _charHorizontalPos = new float[3] { -1.5f, 0, 1.5f }; //Karakterin X koordinatýnda bulunabileceði pozisyonlarý array içinde önceden hazýrladým.
    Animator _charAnimator; //Karakter animatörünü tanýmladým
    Rigidbody _charRigid; //Karakterin zýplamasý için lazým olan komponenti tanýmladým.
    CapsuleCollider _charColl;
    public float _charSpeed = 0.5f; //Zamanla ya da oyun ayarlarýndan ayarlanabilir olmasý için karakterin hýzýný deðiþkene atadým.
    int _currentHPos = 1;
    void Start()
    {
        _charAnimator = GetComponent<Animator>(); //Sürekli GetComponent kullanmamak adýna tek bir yerde topladým hepsini.
        _charRigid = GetComponent<Rigidbody>();
        _charColl = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (GameManager.Instance._gameState != "Game") //Oyun durumu game deðilse return ile geri çevirdim.
            return;
        float _charXPos= Mathf.Lerp(transform.position.x, _charHorizontalPos[_currentHPos], Time.deltaTime * 5f); //Lerp fonsiyonu ile karakterin yumuþak X koordinatý deðiþimlerini yaptým.
        transform.position = new Vector3(_charXPos, transform.position.y, transform.position.z); 
        transform.Translate(0, 0, _charSpeed * Time.deltaTime * 5f);//Oyun çok fazla ve detaylý fizik gerektirmediði için basit bir ileri komutu yazdým.
        string _animName = GameManager.Instance.GetCurrentAnimatorName(); // Karakterin animatörüne eriþip eventler için animasyonun ismin tek seferde aldým.(GarbageCollector tasarrufu için bu iþlemleri tek seferde yapmalýyýz)
        if (_animName == "Run")
        {
            _charSpeed = Mathf.Lerp(_charSpeed, GameManager.Instance._sM._maxSpeed, Time.deltaTime * 1.25f); //Yumuþak bir geçiþ için karakterin hýzýný olabilecek maks hýza eþitledim.
        }
        if (_animName == "Slide") //Karakterin yerden kaydýðýnda olmasý gereken fiziksel etkileþimler için colliderinde çeþitli deðiþiklikler yaptým.
        {
            _charColl.height = 0.5f;
            _charColl.center = new Vector3(0, 0.25f, 0);
        }
        else
        {
            _charColl.height = 1.5f;
            _charColl.center = new Vector3(0, 0.75f, 0);
        }
        _charAnimator.SetFloat("Speed", _charSpeed); //Karakter çok hýzlandýðýnda animasyonda senkron sorunu olmamasý için karakterin hýzýný animasyon hýzýna eþitledim.
    }
    private void OnCollisionEnter(Collision _col) //Fizik motorunu küçükte olsa kullanmak adýna çarpýþmalarý OnCollisionEnter ile yaptým, bu gibi durumlarda Ray kullanmak her zaman tasarruflu olmuyor.
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
    public void MovementControl(string _dir)//MouseController scriptimin karakterime yolladýðý eventleri tek çatý altýnda topladým.
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
