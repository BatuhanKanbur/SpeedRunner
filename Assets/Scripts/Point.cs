using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    Color[] _colors = new Color[5] { Color.red, Color.white, Color.blue, Color.yellow, Color.green }; //Puanlarýn alabileceði renkleri önceden girdim.
    Renderer _mRenderer; //Puan objesinin meshini tanýlmadým.
    MaterialPropertyBlock _mPB;//Bir objeye farklý renkler vermek ayný materyal üzerinde olsa daha yeni materyaller oluþturmayý gerektirir.Bu performansý olumsuz etkiler.
    //Bu nedenle materialpropertyblock kullanarak tek bir materyal üzerinden renk deðiþimini saðladým.
    private void Awake()
    {
        _mRenderer = GetComponent<Renderer>();
        _mPB = new MaterialPropertyBlock();
        _mPB.SetColor("_Color", _colors[Random.Range(0, _colors.Length)]);
        _mRenderer.SetPropertyBlock(_mPB);
    }
    private void FixedUpdate()
    {
        transform.Rotate(0, 5f, 0);//Hoþ gözüksün :)
    }

}
