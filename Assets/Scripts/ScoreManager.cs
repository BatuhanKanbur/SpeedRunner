using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Oyundaki kullan�c� de�i�kenlerinin hepsini tek bir yerde toplad�m(Ad�n�n ScoreManager oldu�una bakmay�n :)) ve bunlarda olu�acak de�i�ikilikleri de singleton ile buraya ba�lad�m.
    public int _totalScore;
    public int _totalHealth=3;
    public float _maxSpeed = 1;
    public int _scoreMultipler = 1;
    public int _currentScore = 0;
    public int _currentHealth = 0;
    public int _currentLevel = 1;
    public void AddScore()
    {
        _currentScore += 10 * _scoreMultipler;
    }
    public void HitDamage()
    {
        _currentHealth--;
        GameManager.Instance._uM.RemoveHeartObject(_currentHealth);//UserInterfaceye g�rsel de�i�iklik i�in mesaj g�nderdim.
    }
    public void FinishLevel()
    {
        if (_currentScore > _totalScore)
        {
            _totalScore = _currentScore;
        }
        _currentLevel++;
    }
    public void SetMechanic(string _mechStr)//Geli�tirici ayarlar�n� da string de�i�kenine atay�p tek bir voide ba�lad�m.
    {
        switch (_mechStr)
        {
            case "Bonus+":
                _scoreMultipler++;
                break;
            case "Bonus-":
                _scoreMultipler--;
                break;
            case "Health+":
                _totalHealth++;
                break;
            case "Health-":
                _totalHealth--;
                break;
            case "Speed+":
                _maxSpeed+=0.25f;
                break;
            case "Speed-":
                _maxSpeed-=0.25f;
                break;
        }
        //Can say�s� ve h�z i�in izin verdi�im maks ve min de�erlerini ayarlad�m.
        _totalHealth = (int)Mathf.Clamp(_totalHealth, 1, Mathf.Infinity);
        _maxSpeed = Mathf.Clamp(_maxSpeed, 0.25f, Mathf.Infinity);
    }
}