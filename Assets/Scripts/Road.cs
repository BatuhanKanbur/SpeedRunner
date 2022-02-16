using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    int _randomType;
    public GameObject[] roadTypes; //Editörde oluþturduðum çeþitli engel varyantlarýný array içine attým.
    void OnEnable()//Tek seferli deðiþiklikleri ,sadece obje aktif olduðunda çalýþan iþleve atadým.(Kahrol UPDATE :) :))
    {
        _randomType = Random.Range(0, roadTypes.Length);//Random.Range ile 0dan engel varyantlarýmýn uzunluðu arasýnda bir sayý buldum.
        roadTypes[_randomType].SetActive(true);//O sayýya denk gelen engel varyantýný aktif ettim.
    }
    void OnDisable()
    {
        roadTypes[_randomType].SetActive(false); //Engel varyant tipini hafýzada tutmak burada iþime yaradý... 
    }
    public void ClearRoadType()//Baþlangýç için engelleri devre dýþý býraktým.
    {
        roadTypes[_randomType].SetActive(false);
    }
}
