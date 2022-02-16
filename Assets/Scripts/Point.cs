using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    Color[] _colors = new Color[5] { Color.red, Color.white, Color.blue, Color.yellow, Color.green }; //Puanlar�n alabilece�i renkleri �nceden girdim.
    Renderer _mRenderer; //Puan objesinin meshini tan�lmad�m.
    MaterialPropertyBlock _mPB;//Bir objeye farkl� renkler vermek ayn� materyal �zerinde olsa daha yeni materyaller olu�turmay� gerektirir.Bu performans� olumsuz etkiler.
    //Bu nedenle materialpropertyblock kullanarak tek bir materyal �zerinden renk de�i�imini sa�lad�m.
    private void Awake()
    {
        _mRenderer = GetComponent<Renderer>();
        _mPB = new MaterialPropertyBlock();
        _mPB.SetColor("_Color", _colors[Random.Range(0, _colors.Length)]);
        _mRenderer.SetPropertyBlock(_mPB);
    }
    private void FixedUpdate()
    {
        transform.Rotate(0, 5f, 0);//Ho� g�z�ks�n :)
    }

}
