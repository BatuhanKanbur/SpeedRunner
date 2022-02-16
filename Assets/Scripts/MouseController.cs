using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseController : MonoBehaviour
{
    //Daha sonraki basit projelerde kullanmak ad�na EventSistemi ile �al��an bir mouse kontrol scripti haz�rlad�m.
    [Serializable] public class OnControllerMove : UnityEvent { } //��ine atanacak de�i�kenler ile daha detayl� bir hale getirilebilir ama �imdilik gerek yoktu.
    //B�t�n kodlardan ba��ms�z �al��mas� ve de�i�iklik yapman�n kolay olmas� ad�na UnityEvent ile haberle�meyi sa�lad�m.
    public OnControllerMove swipeRight;
    public OnControllerMove swipeLeft;
    public OnControllerMove swipeUp;
    public OnControllerMove swipeDown;

    Vector2 _firstPos;//Mouse hareketindeki ilk pozisyonu kaydetmek i�in de�i�ken olu�turdum.
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
            _currentSwipe = new Vector2(_lastPos.x - _firstPos.x, _lastPos.y - _firstPos.y);//Birinci dokunul ile b�rakma aras�ndaki mesafeyi hesaplad�m.
            _currentSwipe.Normalize();//�ok de�i�ik ve karma��k verileri normalize ederek 0 ve 1 aras�ndaki de�erlere e�itledim.
            if (_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f) //Buradaki if �evirimleri detayland�r�labilirdi ama �imdilik ihtiya� yoktu.
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
