using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    int _randomType;
    public GameObject[] roadTypes; //Edit�rde olu�turdu�um �e�itli engel varyantlar�n� array i�ine att�m.
    void OnEnable()//Tek seferli de�i�iklikleri ,sadece obje aktif oldu�unda �al��an i�leve atad�m.(Kahrol UPDATE :) :))
    {
        _randomType = Random.Range(0, roadTypes.Length);//Random.Range ile 0dan engel varyantlar�m�n uzunlu�u aras�nda bir say� buldum.
        roadTypes[_randomType].SetActive(true);//O say�ya denk gelen engel varyant�n� aktif ettim.
    }
    void OnDisable()
    {
        roadTypes[_randomType].SetActive(false); //Engel varyant tipini haf�zada tutmak burada i�ime yarad�... 
    }
    public void ClearRoadType()//Ba�lang�� i�in engelleri devre d��� b�rakt�m.
    {
        roadTypes[_randomType].SetActive(false);
    }
}
