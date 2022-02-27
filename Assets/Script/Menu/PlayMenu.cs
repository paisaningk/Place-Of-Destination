using System.Collections;
using Script.Base;
using Script.Controller;
using Script.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Menu
{
    public class PlayMenu : MonoBehaviour
    {
        [Header("Script")]
        [SerializeField] private ShopController shopController;
        [Header("UI")]
        [SerializeField] private GameObject pauseUi;
        [SerializeField] private GameObject deadUI;
        [SerializeField] private GameObject waveUI;
        [SerializeField] private GameObject StatusUI;
        [Header("Button")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button deadquitButton;
        [SerializeField] private Button deadwinButton;
        [SerializeField] private Button restartwinButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button restartpauseButton;
        [Header("Player")]
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private PlayerCharacter PlayerCharacter;
        [SerializeField] private Image blood;
        [SerializeField] private Image Dash;
        [Header("Status")]
        [SerializeField] private TextMeshProUGUI MaxHPText;
        [SerializeField] private TextMeshProUGUI AtkText;
        [SerializeField] private TextMeshProUGUI SpeedText;
        [SerializeField] private TextMeshProUGUI DashCdText;
        [SerializeField] private TextMeshProUGUI CritRateText;
        [SerializeField] private TextMeshProUGUI GoldText;
        [SerializeField] private Button quitStatusButton;
        [Header("Gun")] 
        [SerializeField] private GameObject Ammoui;
        [SerializeField] private GameObject[] Ammo;
        [SerializeField] private GameObject AmmoText;
        [SerializeField] private SpriteRenderer statusSpriteRenderer;
        [SerializeField] private Sprite PlayerWGun;
        [SerializeField] private SceneName sceneName;
        private PlayerController playerController;
        
        public static bool Isphone;
        public bool isPause = false;
        private float DashCd = 0;
        private bool candash = true;
        private bool StatusShow = false;
        private bool isReload;
        

        private void Awake()
        {
            resumeButton.onClick.AddListener(Resume);
            quitButton.onClick.AddListener(Quit);
            deadquitButton.onClick.AddListener(Quit);
            deadwinButton.onClick.AddListener(Quit);
            restartButton.onClick.AddListener(Restart);
            restartwinButton.onClick.AddListener(Restart);
            restartpauseButton.onClick.AddListener(Restart);
            quitStatusButton.onClick.AddListener(Back);
            Dash.fillAmount = 1;
        }

        private void Start()
        {
            var a = GameObject.FindWithTag("Player");
            PlayerCharacter = a.GetComponent<PlayerCharacter>();
            playerController = a.GetComponent<PlayerController>();
            Ammoui.SetActive(PlayerCharacter.PlayerType == PlayerType.Gun);
            PlayerController.playerInput.PlayerAction.Status.performed += context => OpenStatus();
            if (PlayerCharacter.PlayerType == PlayerType.Gun)
            {
                PlayerController.playerInput.PlayerAction.Reload.performed += context => Reloadative();
                statusSpriteRenderer.sprite = PlayerWGun;
            }
        }

        private void Update()
        {
            if (PlayerCharacter.PlayerType == PlayerType.Gun)
            {
                var ammo = playerController.Ammo;

                if (isReload)
                {
                    foreach (var VARIABLE in Ammo)
                    {
                        VARIABLE.SetActive(true);
                    }
                    isReload = false;
                }
                else if (ammo == 1)
                {
                    Ammo[ammo-1].SetActive(false);
                }
                else if (ammo == 2)
                {
                    Ammo[ammo-1].SetActive(false);
                }
                else if (ammo == 3)
                {
                    Ammo[ammo-1].SetActive(false);
                }
                else if (ammo == 4)
                {
                    Ammo[ammo-1].SetActive(false);
                }
                else if (ammo == 5)
                {
                    Ammo[ammo-1].SetActive(false);
                    StartCoroutine(Reload());
                }
            }

            goldText.text = $"Gold : {PlayerCharacter.Gold}";
            var playerHp = PlayerCharacter.Hp / PlayerCharacter.MaxHp;
            blood.fillAmount = playerHp;
            SetStatus();
            if (PlayerController.CanDash == false)
            {
                if (candash == true)
                {
                    Dash.fillAmount = 0;
                    DashCd = 0;
                    candash = false;
                }
            }

            if (candash == false)
            {
                DashCd += Time.deltaTime;
                Dash.fillAmount = DashCd / PlayerCharacter.DashCd;
                if (DashCd / PlayerCharacter.DashCd >= 1)
                {
                    StartCoroutine(SetDashCd());
                }
            }
        }

        public void Reloadative()
        {
            if (playerController.Ammo > 0)
            {
                StartCoroutine(Reload());
            }
        }
        
        IEnumerator Reload()
        {
            SoundManager.Instance.Play(SoundManager.Sound.Reload);
            foreach (var VARIABLE in Ammo)
            {
                VARIABLE.SetActive(false);
            }
            AmmoText.SetActive(true);
            yield return new WaitForSeconds(playerController.ReloadTime);
            AmmoText.SetActive(false);
            isReload = true;
        }
        
        public void OpenStatus()
        {
            if (StatusShow == false)
            {
                //phoneUI.SetActive(false);
                StatusUI.SetActive(true);
                StatusShow = true;
                Time.timeScale = 0;
            }
            else
            {
                Back();
            }
            
        }

        private void Back()
        {
            StatusUI.SetActive(false);
            StatusShow = false;
            Time.timeScale = 1;
            //phoneUI.SetActive(true);
        }

        IEnumerator SetDashCd()
        {
            yield return new WaitForSeconds(0.1f);
            candash = true;
            StopCoroutine(SetDashCd());
        }

        private void SetStatus()
        {
            MaxHPText.text = $"{PlayerCharacter.MaxHp}";
            SpeedText.text = $"{PlayerCharacter.Speed}";
            AtkText.text = $"{PlayerCharacter.Atk}";
            DashCdText.text = $"{PlayerCharacter.DashCd}";
            CritRateText.text = $"{PlayerCharacter.CritRate}";
            GoldText.text = $"{PlayerCharacter.Gold}";
        }
        
        public void Pause()
        {
            if (shopController.shoping == false)
            {
                //phoneUI.SetActive(false);
                PlayerController.playerInput.PlayerAction.Attack.Disable();
                PlayerController.playerInput.PlayerAction.Dash.Disable();
                PlayerController.playerInput.PlayerAction.Move.Disable();
                pauseUi.SetActive(true);
                Time.timeScale = 0;
                isPause = true;
            }
        }
        public void Resume()
        {
            //phoneUI.SetActive(true);
            pauseUi.SetActive(false);
            Time.timeScale = 1;
            isPause = false;
            PlayerController.playerInput.PlayerAction.Attack.Enable();
            PlayerController.playerInput.PlayerAction.Dash.Enable();
            PlayerController.playerInput.PlayerAction.Move.Enable();
        }

        private void Quit()
        {
            SceneManager.LoadScene("MainMenu_PC");
        }
        
        private void Restart()
        {
            SceneManager.LoadScene($"{sceneName}");
        }

        public void Dead()
        {
            pauseUi.SetActive(false);
            waveUI.SetActive(false);
            deadUI.SetActive(true);
        }

        
    }
}
