using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMechanic : MonoBehaviour
{
    //Oyun t�r� ne olursa olsun s�rekli obje yarat�p silmek performans� ciddi anlamda olumsuz etkilemektedir.Bu konuda kurtar�c�m�z olan ObjectPooling design patternine ba�vurdum.
    [Serializable]
    public struct PoolObject //Struct ile pool elementim i�in ihtiyac�m olan de�i�kenleri girdim.
    {
        public Queue<GameObject> poolObjects;
        public GameObject objectPrefab;
        public int poolLenght;
    }
    public PoolObject[] _pools; //Birden fazla object pool objem olaca�� i�in array i�inde hepsini toplad�m.
    public float _lastRoadEndPoint = 0; //Olu�an son yolun koordinat�n� de�i�kene kaydettim, ileride �ok yard�mc� olacak :)
    void Awake()
    {
        for (int i = 0; i < _pools.Length; i++) //For d�ng�s� ile pool elementlerimi s�ralad�m.
        {
            _pools[i].poolObjects = new Queue<GameObject>(); //Pool elementlerimin objelerini s�ral� tutmas� i�in Queue kulland�m.
            float _roadPos = -7.6f; //Olu�acak yollar�n varaca�� ba�lang�� koordinat� i�in karakterin bir tile gerisindeki de�eri girdim, b�ylece ba�lang�� ekran�nda yolun kesikli�i g�z�kmeyecek.
            for (int y = 0; y < _pools[i].poolLenght; y++) //Pool elementlerimin objelerinin olu�mas� i�in for d�ng�s� yapt�m.
            {
                GameObject _gO = Instantiate(_pools[i].objectPrefab); //D�zenli durmas� i�in bir parentin i�inde olu�turabilirdim ama proje k���k oldu�u i�in worldspacede olu�turdum.
                if (_pools[i].objectPrefab.name == "Road")//�e�itli de�i�iklikler i�in tespit etmem gereken objeleri basit bir if d�ng�s� ile tespit ettim.
                {
                    _gO.transform.position = new Vector3(0, 0, _roadPos);//Olu�acak pool objemin koordinatlar�n� girdim.
                    _roadPos += 7.6f;
                    _gO.GetComponent<Road>().ClearRoadType();//Yol objemin �zerindeki engelleri, oyun ba�lang�c� g�z�kmemesi ve oyuncuya kontrolleri tan�mas�s� i�in vakit vermesi ad�na kald�rd�m.
                    if (_pools[i].poolLenght - 1 == y)
                    {
                        _lastRoadEndPoint = _roadPos - 7.6f;
                    }
                } 
                else
                {
                    _gO.SetActive(false);
                }
                _pools[i].poolObjects.Enqueue(_gO);//Olu�an pool objesinin s�raya al�nmas�n� sa�lad�m.
            }
        }
    }

    public GameObject GetGameObject(int _type) //GameManager ya da di�er scriptlerin ihtiyac� olan objeleri buradan GameObject olarak almas�n� sa�lad�m.Tip i�in pool element idisini girmesi yeterli.
    {
        if (_pools[_type].objectPrefab.name == "Road")
        {
            _lastRoadEndPoint += 7.6f;
        }
        GameObject _gO = _pools[_type].poolObjects.Dequeue();//Pool s�ras�n�n tekrar d�zenlenmesini sa�lad�m.
        _gO.SetActive(true);//Objenin g�steri zaman� geldi�i i�in aktifle�tirdim.
        _pools[_type].poolObjects.Enqueue(_gO);
        return _gO;
    }
}
