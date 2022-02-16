using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMechanic : MonoBehaviour
{
    //Oyun türü ne olursa olsun sürekli obje yaratýp silmek performansý ciddi anlamda olumsuz etkilemektedir.Bu konuda kurtarýcýmýz olan ObjectPooling design patternine baþvurdum.
    [Serializable]
    public struct PoolObject //Struct ile pool elementim için ihtiyacým olan deðiþkenleri girdim.
    {
        public Queue<GameObject> poolObjects;
        public GameObject objectPrefab;
        public int poolLenght;
    }
    public PoolObject[] _pools; //Birden fazla object pool objem olacaðý için array içinde hepsini topladým.
    public float _lastRoadEndPoint = 0; //Oluþan son yolun koordinatýný deðiþkene kaydettim, ileride çok yardýmcý olacak :)
    void Awake()
    {
        for (int i = 0; i < _pools.Length; i++) //For döngüsü ile pool elementlerimi sýraladým.
        {
            _pools[i].poolObjects = new Queue<GameObject>(); //Pool elementlerimin objelerini sýralý tutmasý için Queue kullandým.
            float _roadPos = -7.6f; //Oluþacak yollarýn varacaðý baþlangýç koordinatý için karakterin bir tile gerisindeki deðeri girdim, böylece baþlangýç ekranýnda yolun kesikliði gözükmeyecek.
            for (int y = 0; y < _pools[i].poolLenght; y++) //Pool elementlerimin objelerinin oluþmasý için for döngüsü yaptým.
            {
                GameObject _gO = Instantiate(_pools[i].objectPrefab); //Düzenli durmasý için bir parentin içinde oluþturabilirdim ama proje küçük olduðu için worldspacede oluþturdum.
                if (_pools[i].objectPrefab.name == "Road")//Çeþitli deðiþiklikler için tespit etmem gereken objeleri basit bir if döngüsü ile tespit ettim.
                {
                    _gO.transform.position = new Vector3(0, 0, _roadPos);//Oluþacak pool objemin koordinatlarýný girdim.
                    _roadPos += 7.6f;
                    _gO.GetComponent<Road>().ClearRoadType();//Yol objemin üzerindeki engelleri, oyun baþlangýcý gözükmemesi ve oyuncuya kontrolleri tanýmasýsý için vakit vermesi adýna kaldýrdým.
                    if (_pools[i].poolLenght - 1 == y)
                    {
                        _lastRoadEndPoint = _roadPos - 7.6f;
                    }
                } 
                else
                {
                    _gO.SetActive(false);
                }
                _pools[i].poolObjects.Enqueue(_gO);//Oluþan pool objesinin sýraya alýnmasýný saðladým.
            }
        }
    }

    public GameObject GetGameObject(int _type) //GameManager ya da diðer scriptlerin ihtiyacý olan objeleri buradan GameObject olarak almasýný saðladým.Tip için pool element idisini girmesi yeterli.
    {
        if (_pools[_type].objectPrefab.name == "Road")
        {
            _lastRoadEndPoint += 7.6f;
        }
        GameObject _gO = _pools[_type].poolObjects.Dequeue();//Pool sýrasýnýn tekrar düzenlenmesini saðladým.
        _gO.SetActive(true);//Objenin gösteri zamaný geldiði için aktifleþtirdim.
        _pools[_type].poolObjects.Enqueue(_gO);
        return _gO;
    }
}
