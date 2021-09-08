using System;
using System.Collections.Generic;
using Project.Scripts.Core;
using UnityEngine;

namespace Project.Scripts
{
    [Serializable]
    public class EnemyRowDifficultyLevel
    {
        public int level;
        [Range(0.6f, 1.0f)] public float enemyPercent;
        public float difficultyMultiplier = 1.2f;
    }
    
    public class EnemyRow : MonoBehaviour
    {
        public bool moveLeft;
        public Enemy enemyPrefab;
        public EnemyRowDifficultyLevel[] difficultyMap;
        public float[] enemySlots;
        public Sprite[] enemySprites;
        private Enemy[] _enemies;

        private int _currentDifficultyIndex;
        
        private void Start()
        {
            SpawnEnemies();
        }

        public void AdvanceLevel()
        {
            var i = _currentDifficultyIndex + 1;
            if (i > difficultyMap.Length - 1) return;
            _currentDifficultyIndex = i;
            SpawnEnemies();
        }
        
        private void SpawnEnemies()
        {
            var enemies = new Enemy[enemySlots.Length];
            var position = transform.position;
            var numEnemies = Mathf.FloorToInt(enemySlots.Length * difficultyMap[_currentDifficultyIndex].enemyPercent);
            for (var i = 0; i < numEnemies; i++)
            {
                enemies[i] = Instantiate(enemyPrefab, transform);
                enemies[i].GetComponent<SpriteRenderer>().sprite = enemySprites.Random();
                enemies[i].gameObject.transform.position = new Vector3(enemySlots[i], position.y, 0.0f);
                enemies[i].speed *= difficultyMap[_currentDifficultyIndex].difficultyMultiplier;
                
                if ((moveLeft && enemies[i].speed > 0) || 
                    (!moveLeft && enemies[i].speed < 0))
                {
                    enemies[i].speed *= -1;
                }
            }

            if (_enemies != null && _enemies.Length > 0)
            {
                foreach (var enemy in _enemies)
                {
                    if (enemy?.gameObject == null) continue;
                    Destroy(enemy.gameObject);
                }
            }

            _enemies = enemies;
        }
    }
}
