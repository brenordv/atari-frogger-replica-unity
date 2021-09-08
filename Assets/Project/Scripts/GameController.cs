using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class GameController : MonoBehaviour
    {
        public Player player;
        public Text scoreText;
        public Text levelText;
        public Text gameOverText;
        public AudioClip gameOverClip;
        public AudioClip nextLevelClip;
        
        private float _highestPosition;
        private int _score;
        private int _level;
        private float _restartTimer = 5f;
        private AudioSource _audioSource;
        private bool _playedGameOverSound;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            gameOverText.gameObject.SetActive(false);
            player.OnPlayerMoved += OnPlayerMoved;
            player.OnPlayerEscaped += OnPlayerEscaped;
            _highestPosition = player.transform.position.y;
            ResetScoreboard();
        }

        void Update()
        {
            if (player != null) return;
            GameOver();
        }

        private void GameOver()
        {
            if (!_playedGameOverSound)
            {
                gameOverText.gameObject.SetActive(true);
                _audioSource.PlayOneShot(gameOverClip);
                _playedGameOverSound = true;
            }
            
            _restartTimer -= Time.deltaTime;
            if (_restartTimer > 0) return;
            
            SceneManager.LoadScene("Game");
        }

        private void OnPlayerMoved()
        {
            var y = player.transform.position.y;
            if (y <= _highestPosition) return;

            _highestPosition = y;
            AddScore();
        }

        private void OnPlayerEscaped()
        {
            _highestPosition = player.transform.position.y;
            foreach (var row in GetComponentsInChildren<EnemyRow>())
            {
                row.AdvanceLevel();
            }

            NextLevel();
            _audioSource.PlayOneShot(nextLevelClip);
        }

        private void AddScore(int points = 1)
        {
            _score += points;
            UpdateScoreText();
        }

        private void NextLevel()
        {
            _level++;
            UpdateLevelText();
        }

        private void ResetScoreboard()
        {
            _level = 1;
            _score = 0;
            UpdateLevelText();
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            scoreText.text = $"Score: {_score}";
        }
        
        private void UpdateLevelText()
        {
            levelText.text = $"Level: {_level}";
        }
    }
}