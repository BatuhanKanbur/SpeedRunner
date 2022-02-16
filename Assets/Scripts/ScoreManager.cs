using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Oyundaki kullanýcý deðiþkenlerinin hepsini tek bir yerde topladým(Adýnýn ScoreManager olduðuna bakmayýn :)) ve bunlarda oluþacak deðiþikilikleri de singleton ile buraya baðladým.
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
        GameManager.Instance._uM.RemoveHeartObject(_currentHealth);//UserInterfaceye görsel deðiþiklik için mesaj gönderdim.
    }
    public void FinishLevel()
    {
        if (_currentScore > _totalScore)
        {
            _totalScore = _currentScore;
        }
        _currentLevel++;
    }
    public void SetMechanic(string _mechStr)//Geliþtirici ayarlarýný da string deðiþkenine atayýp tek bir voide baðladým.
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
        //Can sayýsý ve hýz için izin verdiðim maks ve min deðerlerini ayarladým.
        _totalHealth = (int)Mathf.Clamp(_totalHealth, 1, Mathf.Infinity);
        _maxSpeed = Mathf.Clamp(_maxSpeed, 0.25f, Mathf.Infinity);
    }
}