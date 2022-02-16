using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseController : MonoBehaviour
{
    //Daha sonraki basit projelerde kullanmak adýna EventSistemi ile çalýþan bir mouse kontrol scripti hazýrladým.
    [Serializable] public class OnControllerMove : UnityEvent { } //Ýçine atanacak deðiþkenler ile daha detaylý bir hale getirilebilir ama þimdilik gerek yoktu.
    //Bütün kodlardan baðýmsýz çalýþmasý ve deðiþiklik yapmanýn kolay olmasý adýna UnityEvent ile haberleþmeyi saðladým.
    public OnControllerMove swipeRight;
    public OnControllerMove swipeLeft;
    public OnControllerMove swipeUp;
    public OnControllerMove swipeDown;

    Vector2 _firstPos;//Mouse hareketindeki ilk pozisyonu kaydetmek için deðiþken oluþturdum.
    Vector2 _lastPos;
    Vector2 _currentSwipe;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _firstPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            _lastPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _currentSwipe = new Vector2(_lastPos.x - _firstPos.x, _lastPos.y - _firstPos.y);//Birinci dokunul ile býrakma arasýndaki mesafeyi hesapladým.
            _currentSwipe.Normalize();//Çok deðiþik ve karmaþýk verileri normalize ederek 0 ve 1 arasýndaki deðerlere eþitledim.
            if (_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f) //Buradaki if çevirimleri detaylandýrýlabilirdi ama þimdilik ihtiyaç yoktu.
            {
                swipeUp.Invoke();
            }
            if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
            {
                swipeDown.Invoke();
            }
            if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
            {
                swipeLeft.Invoke();
            }
            if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
            {
                swipeRight.Invoke();
            }
        }
    }
}
