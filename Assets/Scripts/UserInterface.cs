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
        devSpeedText.text = "Koþu Hýzý : " + GameManager.Instance._sM._maxSpeed.ToString("0.00");
        //Bu tarz animasyonlarý animasyon panelinde yapmak herzaman daha iyidir, ama iþimi hýzlandýrmasý adýna kod ile yaptým.
        //Oyun durumunu GameManagerden singleton ile öðrendim.
        //Yazýlarý ise tek bir textde toplayýp rich text methodlarý ile düzenledim, böylece UI.Text deðiþkenleri arasýnda kaybolmadým.Ama oyunun yapýsý buna müsade etmeseydi farklý bir yol izlenilebilridi.
        //UI objelerinin animasyonu için Vector2.Lerp methodunu kullandým, uzun soluklu karýþýk projelerde kullanmaktan kaçýnsam da bu proje için iþimi gördü.
        switch (GameManager.Instance._gameState)
        {
            case "Intro":
                menuInfoText.text = "LEVEL " + GameManager.Instance._sM._currentLevel.ToString("00") + "\n" +
                    "<i><color=red>YÜKSEK SKOR</color></i>\n" +
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
                finishInfoText.text = "TEBRÝKLER\n" +
                "<b><color=red>LEVEL " + GameManager.Instance._sM._currentLevel.ToString("00") + "</color></b>\n" +
                "<b><color=blue>SKOR : " + GameManager.Instance._sM._currentScore.ToString("00") + "</color></b>\n" +
                "<b><color=green>YÜKSEK SKOR : " + GameManager.Instance._sM._totalScore.ToString("00") + "</color></b>\n";
                MenuLabel.anchoredPosition = Vector2.Lerp(MenuLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                GameLabel.anchoredPosition = Vector2.Lerp(GameLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                FinishLabel.anchoredPosition = Vector2.Lerp(FinishLabel.anchoredPosition, new Vector2(0, 0), Time.deltaTime * 3f);
                RestartButton.SetActive(false);
                ResumeButton.SetActive(true);
                break;
            case "Lose":
                finishInfoText.text = "KAYBETTÝN\n" +
                "<b><color=red>LEVEL " + GameManager.Instance._sM._currentLevel.ToString("00") + "</color></b>\n" +
                "<b><color=blue>SKOR : " + GameManager.Instance._sM._currentScore.ToString("00") + "</color></b>\n" +
                "<b><color=green>YÜKSEK SKOR : " + GameManager.Instance._sM._totalScore.ToString("00") + "</color></b>\n";
                MenuLabel.anchoredPosition = Vector2.Lerp(MenuLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                GameLabel.anchoredPosition = Vector2.Lerp(GameLabel.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3f);
                FinishLabel.anchoredPosition = Vector2.Lerp(FinishLabel.anchoredPosition, new Vector2(0, 0), Time.deltaTime * 3f);
                RestartButton.SetActive(true);
                ResumeButton.SetActive(false);
                break;
        }

    }
    public void InstallHeartObjects() //Kalp objelerini oyun baþlarken burada oluþturdum.
    {
        for (int y = 0; y < _healthObjs.Count; y++)//Önce eski kalp objelerini yok ettim, çünkü geliþtirici ayarlarýndan toplam sayýsý deðiþtirilmiþ olabilirdi.
        {
            Destroy(_healthObjs[y].gameObject);
        }
        _healthObjs.Clear();//Kalp obje listesini sýfýrladým.
        for (int i = 0; i < GameManager.Instance._sM._totalHealth; i++)//Yeni kalp objelerini yarattým ve onlarý tek tek listeye ekledim.
        {
            Image _newHeartObject = Instantiate(heartPrefab, heartLabel.transform).GetComponent<Image>();
            _newHeartObject.sprite = _healthFullSprite;//Kalp objesine dolu olan sprite atadým.
            _healthObjs.Add(_newHeartObject);
        }
    }
    public void RemoveHeartObject(int _currentHeart)//Çarpmalarda ne kadar caný kaldýðný buraya bildirdim.
    {
        for (int i = _currentHeart; i < _healthObjs.Count; i++)//For döngüsünü kalan canýndan baþlatarak son kalp objesinin içinin boþalmasýný saðladým.
        {
            _healthObjs[i].sprite = _healthEmptySprite;
        }
    }
}
