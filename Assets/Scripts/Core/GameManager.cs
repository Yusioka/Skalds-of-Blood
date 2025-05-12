using Combat;
using Control;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private Progression _progression;

        [SerializeField] private GameObject _player;
        [SerializeField] private List<Entity> _enemyPrefabs;
        [SerializeField] private GameObject[] _spawnPoints;
        [SerializeField] private BoxCollider _tutorialCollider;

        [SerializeField] private Text _levelText;
        [SerializeField] private Text _loseText, _winText;
        [SerializeField] private Button _menuButton, _exitButton;
        [SerializeField] private AudioSource _backgroundAudio, _levelCompleteAudio, _winAudio, _loseAudio;

        [SerializeField] private GameObject _healthPowerUpPrefab;
        [SerializeField] private GameObject[] _powerUpSpawnPoints;

        private readonly List<EnemyHealth> _enemies = new();
        private readonly List<EnemyHealth> _killedEnemies = new();
        private readonly HashSet<Ability> _activatedAbilities = new();

        private const float _enemySpawnInterval = 2f;
        private const int _maxPowerUps = 3;
        private const int _finalLevel = 10;

        private EnemyFactory _enemyFactory;

        private int _currentLevel = 0;
        private bool _isGameOver;
        private bool _isGamePaused;
        private bool _isCursorActive = true;
        private bool _isGodMode;

        private int _levelsWithoutPowerUps;
        private int _powerUpsCount;
        private bool _isTookPowerUpInLevel;

        private int step = 0;
        private bool waitingForAction = false;

        private float _spawnTimer;

        public bool IsSkillUsing { get; set; }

        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                if (_isGameOver == value)
                {
                    return;
                }

                _isGameOver = value;
            }
        }

        public bool IsPaused
        {
            get => _isGamePaused;
            set
            {
                if (_isGamePaused == value)
                {
                    return;
                }

                _isGamePaused = value;
            }
        }

        public bool IsCursorActive
        {
            get => _isCursorActive;
            set
            {
                if (_isCursorActive == value)
                {
                    return;
                }

                _isCursorActive = value;
                if (_isCursorActive)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                Cursor.visible = _isCursorActive;
            }
        }

        public bool IsGodMode
        {
            get => _isGodMode;
            set
            {
                if (_isGodMode == value)
                {
                    return;
                }

                _isGodMode = value;
            }
        }

        public bool CanUseAbility { get; set; } = true;

        public GameObject Player => _player;
        public bool CanSpawnPowerUp { get; set; } = true;
        public AudioSource BackgroundAudio => _backgroundAudio;
        public TraitController TraitController { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);

            _enemyFactory = new EnemyFactory(_enemyPrefabs);

            TraitController = GetComponent<TraitController>();
        }

        private void Start()
        {
          //  PlayerPrefs.DeleteKey("TutorialCompleted");

            _backgroundAudio = GetComponent<AudioSource>();
            _backgroundAudio.Play();

            if (PlayerPrefs.HasKey("TutorialCompleted") == false)
            {
                StartTutorial();
            }
            else
            {
                _currentLevel = 1;
                _levelText.text = "Level " + _currentLevel;
                HandleEnemySpawning();
            }
        }

        private void Update()
        {
            Time.timeScale = IsPaused ? 0 : 1;
            if (IsSkillUsing == false)
            {
                IsCursorActive = IsPaused;
            }

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                TriggerWin();
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                TriggerLose();
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                NextWave();
            }
            if (Input.GetKeyUp(KeyCode.Keypad5))
            {
                ToggleGodMode();
            }

            if (PlayerPrefs.HasKey("TutorialCompleted") == false)
            {
                if (waitingForAction == false)
                {
                    return;
                }

                switch (step)
                {
                    case 1:
                        if (PlayerMoved())
                        {
                            NextStep();
                        }
                        break;
                    case 2:
                        if (EnemyKilled())
                        {
                            NextStep();
                        }
                        break;
                    case 3:
                        if (TraitOpened())
                        {
                            NextStep();
                        }
                        break;
                    case 4:
                        if (SkillsUsed())
                        {
                            NextStep();
                        }
                        break;
                }
            }
            else
            {
                if (_isGameOver)
                {
                    return;
                }

                _spawnTimer += Time.deltaTime;

                HandleEnemySpawning();
                HandlePowerUpSpawning();
                HandleWinCondition();
            }
        }

        public void StartTutorial()
        {
            step = 0;
            NextStep();
        }

        private void NextStep()
        {
            step++;
            waitingForAction = false;

            switch (step)
            {
                case 1:
                    _levelText.text = "Use WASD to move.";
                    waitingForAction = true;
                    CanUseAbility = false;
                    break;
                case 2:
                    _levelText.text = "Attack the enemy! LMB - normal attack, RMB - strong attack.";
                    SpawnEnemy();
                    waitingForAction = true;
                    break;
                case 3:
                    _levelText.text = "Familiarize yourself with the TRAITS by pressing the 0 button.";
                    waitingForAction = true;
                    break;
                case 4:
                    _levelText.text = "Now use your skills! Q - Skill 1, E - Skill 2.";
                    SpawnTwoEnemies();
                    CanUseAbility = true;
                    waitingForAction = true;
                    break;
                case 5:
                    _levelText.text = "All right! With each round, the monsters will be stronger. Survive as long as possible!";
                    Invoke(nameof(StartGame), 3f);
                    break;
            }
        }

        private void StartGame()
        {
            PlayerPrefs.SetString("TutorialCompleted", "true");
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        private bool PlayerMoved()
        {
            return _tutorialCollider.bounds.Contains(Player.transform.position);
        }

        private bool EnemyKilled()
        {
            return _killedEnemies.Count > 0;
        }

        private bool SkillsUsed()
        {
            return _activatedAbilities.Count > 1 && _killedEnemies.Count > 2;
        }

        private bool TraitOpened()
        {
            return Input.GetKeyDown(KeyCode.Keypad0);
        }

        private void SpawnEnemy()
        {
            var randomPoint = Random.Range(0, _spawnPoints.Length);
            _enemyFactory.CreateEnemy(CharacterType.Soldier, _spawnPoints[randomPoint].transform.position, _spawnPoints[randomPoint].transform.rotation);
        }

        private void SpawnTwoEnemies()
        {
            for (int i = 0; i < 2; i++)
            {
                SpawnEnemy();
            }
        }

        public void RegisterEnemy(EnemyHealth enemy) => _enemies.Add(enemy);
        public void RegisterPowerUp() => _powerUpsCount++;

        public void RegisterAbility(Ability ability) => _activatedAbilities.Add(ability);

        public void RegisterKilledEnemy(EnemyHealth enemy)
        {
            _killedEnemies.Add(enemy);
            if (PlayerPrefs.HasKey("TutorialCompleted"))
            {
                AdvanceLevel();
            }
        }

        public float GetStat(StatType statType, CharacterType characterType)
        {
            if (characterType != CharacterType.Player)
            {
                return _progression.GetStat(statType, characterType, _currentLevel);
            }

            return _progression.GetStat(statType, characterType, _currentLevel) * (1 + GetModifier(statType, characterType) / 100);
        }

        private float GetModifier(StatType statType, CharacterType characterType)
        {
            float total = 0;

            foreach (var modifier in TraitController.GetModifiers(statType))
            {
                total += modifier;
            }

            return total;
        }

        public void PlayerHit(float currentHP)
        {
            if (currentHP > 0)
            {
                return;
            }

            _isGameOver = true;
            _isGamePaused = true;

            //_loseText.gameObject.SetActive(true);
            //_exitButton.gameObject.SetActive(true);
            //_menuButton.gameObject.SetActive(true);

            _backgroundAudio.Stop();
            _loseAudio.Play();
        }

        private void HandleEnemySpawning()
        {
            if (_spawnTimer < _enemySpawnInterval || _enemies.Count >= _currentLevel || _isGameOver)
            {
                return;
            }

            _spawnTimer = 0;

            int randomPoint = Random.Range(0, _spawnPoints.Length);
            CharacterType randomEnemyType = (CharacterType)Random.Range(0, _enemyPrefabs.Count);
            Entity newEnemy = _enemyFactory.CreateEnemy(randomEnemyType, _spawnPoints[randomPoint].transform.position, _spawnPoints[randomPoint].transform.rotation);
        }

        private void AdvanceLevel()
        {
            if (_killedEnemies.Count < _currentLevel)
            {
                return;
            }

            _enemies.Clear();
            _killedEnemies.Clear();

            _currentLevel++;
            _levelText.text = "Level " + _currentLevel;

            if (_currentLevel < _finalLevel)
            {
                _levelCompleteAudio.Play();
            }
            else
            {
                _winAudio.Play();
            }

            _isTookPowerUpInLevel = false;
            _levelsWithoutPowerUps++;
        }

        private void HandlePowerUpSpawning()
        {
            if (_currentLevel < 3 || !CanSpawnPowerUp || _isTookPowerUpInLevel || _powerUpsCount >= _maxPowerUps || _levelsWithoutPowerUps < 2)
            {
                return;
            }

            int randomPoint = Random.Range(0, _powerUpSpawnPoints.Length);
            Instantiate(_healthPowerUpPrefab, _powerUpSpawnPoints[randomPoint].transform.position, Quaternion.identity);

            CanSpawnPowerUp = false;
            _isTookPowerUpInLevel = true;
            _levelsWithoutPowerUps = 0;
        }

        public void TriggerWin()
        {
            _currentLevel = _finalLevel;
            HandleWinCondition();
        }

        public void TriggerLose()
        {
            PlayerHit(0);
        }

        public void NextWave()
        {
            foreach (var enemy in FindObjectsOfType<EnemyHealth>())
            {
                enemy.Die();
            }

            AdvanceLevel();
        }

        public void ToggleGodMode()
        {
            IsGodMode = !IsGodMode;
        }

        private void HandleWinCondition()
        {
            if (_currentLevel != _finalLevel || _isGameOver)
            {
                return;
            }

            _isGameOver = true;
            _isGamePaused = true;
            _winAudio.Play();

            _backgroundAudio.Stop();

            //_winText.gameObject.SetActive(true);
            //_exitButton.gameObject.SetActive(true);
            //_menuButton.gameObject.SetActive(true);
        }
    }
}