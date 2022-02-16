using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public RectTransform MenuLabel;
    public Text menuInfoText;
    public Text devHealthCountText;
    public Text devBonusText;
    public Text devSpeedText;

    public RectTransform GameLabel;
    public Text gameInfoText;
    public GameObject heartLabel;
    public GameObject heartPrefab;
    public List<Image> _healthObjs;
    public Sprite _healthFullSprite;
    public Sprite _healthEmptySprite;

    public RectTransform FinishLabel;
    public Text finishInfoText;
    public GameObject RestartButton;
    public GameObject ResumeButton;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        devHealthCountText.text = "Toplam Can : " + GameManager.Instance._sM._totalHealth.ToString("0");
        devBonusText.text = "Skor Bonusu : " + GameManager.Instance._sM._scoreMultipler.ToString("0");
        devSpeedText.text = "Ko�u H�z� : " + GameManager.Instance._sM._maxSpeed.ToString("0.00");
        //Bu tarz animasyonlar� animasyon panelinde yapmak herzaman daha iyidir, ama i�imi h�zland�rmas� ad�na kod ile yapt�m.
        //Oyun durumunu GameManagerden singleton ile ��rendim.
        //Yaz�lar� ise tek bir textde toplay�p rich text methodlar� ile d�zenledim, b�ylece UI.Text de�i�kenleri aras�nda kaybolmad�m.Ama oyunun yap�s� buna m�sade etmeseydi farkl� bir yol izlenilebilridi.
        //UI objelerinin animasyonu i�in Vector2.Lerp methodunu kulland�m, uzun soluklu kar���k projelerde kullanmaktan ka��nsam da bu proje i�in i�imi g�rd�.
        switch (GameManager.Instance._gameState)
        {
            case "Intro":
                menuInfoText.text = "LEVEL " + GameManager.Instance._sM._currentLevel.ToString("00") + "\n" +
                    "<i><color=red>Y�KSEK SKOR</color></i>\n" +
                    "<b>"+GameManager.Instance._sM._totalScore+"</b>";
                MenuLabel.anchoredPosition = Vector2.Lerp(MenuLabel.anchoredPosition, new Vector2(0, 0), Time.deltaTime * 3f);
                GameLabel.anchoredPosition = Vector2.Lerp(GameLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                FinishLabel.anchoredPosition = Vector2.Lerp(FinishLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                break;
            case "Game":
                gameInfoText.text = "LEVEL " + GameManager.Instance._sM._currentLevel.ToString("00") + "\n" +
                    "<b><color=red>" + GameManager.Instance._sM._currentScore + "</color></b>";
                MenuLabel.anchoredPosition = Vector2.Lerp(MenuLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                GameLabel.anchoredPosition = Vector2.Lerp(GameLabel.anchoredPosition, new Vector2(0, 0), Time.deltaTime * 3f);
                FinishLabel.anchoredPosition = Vector2.Lerp(FinishLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                break;
            case "Win":
                finishInfoText.text = "TEBR�KLER\n" +
                "<b><color=red>LEVEL " + GameManager.Instance._sM._currentLevel.ToString("00") + "</color></b>\n" +
                "<b><color=blue>SKOR : " + GameManager.Instance._sM._currentScore.ToString("00") + "</color></b>\n" +
                "<b><color=green>Y�KSEK SKOR : " + GameManager.Instance._sM._totalScore.ToString("00") + "</color></b>\n";
                MenuLabel.anchoredPosition = Vector2.Lerp(MenuLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                GameLabel.anchoredPosition = Vector2.Lerp(GameLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                FinishLabel.anchoredPosition = Vector2.Lerp(FinishLabel.anchoredPosition, new Vector2(0, 0), Time.deltaTime * 3f);
                RestartButton.SetActive(false);
                ResumeButton.SetActive(true);
                break;
            case "Lose":
                finishInfoText.text = "KAYBETT�N\n" +
                "<b><color=red>LEVEL " + GameManager.Instance._sM._currentLevel.ToString("00") + "</color></b>\n" +
                "<b><color=blue>SKOR : " + GameManager.Instance._sM._currentScore.ToString("00") + "</color></b>\n" +
                "<b><color=green>Y�KSEK SKOR : " + GameManager.Instance._sM._totalScore.ToString("00") + "</color></b>\n";
                MenuLabel.anchoredPosition = Vector2.Lerp(MenuLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                GameLabel.anchoredPosition = Vector2.Lerp(GameLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                FinishLabel.anchoredPosition = Vector2.Lerp(FinishLabel.anchoredPosition, new Vector2(0, 0), Time.deltaTime * 3f);
                RestartButton.SetActive(true);
                ResumeButton.SetActive(false);
                break;
        }

    }
    public void InstallHeartObjects() //Kalp objelerini oyun ba�larken burada olu�turdum.
    {
        for (int y = 0; y < _healthObjs.Count; y++)//�nce eski kalp objelerini yok ettim, ��nk� geli�tirici ayarlar�ndan toplam say�s� de�i�tirilmi� olabilirdi.
        {
            Destroy(_healthObjs[y].gameObject);
        }
        _healthObjs.Clear();//Kalp obje listesini s�f�rlad�m.
        for (int i = 0; i < GameManager.Instance._sM._totalHealth; i++)//Yeni kalp objelerini yaratt�m ve onlar� tek tek listeye ekledim.
        {
            Image _newHeartObject = Instantiate(heartPrefab, heartLabel.transform).GetComponent<Image>();
            _newHeartObject.sprite = _healthFullSprite;//Kalp objesine dolu olan sprite atad�m.
            _healthObjs.Add(_newHeartObject);
        }
    }
    public void RemoveHeartObject(int _currentHeart)//�arpmalarda ne kadar can� kald��n� buraya bildirdim.
    {
        for (int i = _currentHeart; i < _healthObjs.Count; i++)//For d�ng�s�n� kalan can�ndan ba�latarak son kalp objesinin i�inin bo�almas�n� sa�lad�m.
        {
            _healthObjs[i].sprite = _healthEmptySprite;
        }
    }
}
