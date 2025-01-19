using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyGameObjects; // Массив врагов для спавна
    private Collider2D[] _spawnZones2D;
    private Random _random;

    private List<SaveManager.EnemyData> _enemyDataList = new List<SaveManager.EnemyData>();
    private List<EnemyHealth> _activeEnemies = new List<EnemyHealth>(); // Активные враги

    private void Start()
    {
        _spawnZones2D = GetComponentsInChildren<Collider2D>();
        if (_spawnZones2D.Length == 0)
        {
            Debug.LogError("Нет зон спавна.");
            return;
        }

        if (enemyGameObjects.Length == 0)
        {
            Debug.LogError("Нет врагов для спавна.");
            return;
        }

        _random = new Random((uint)DateTime.UtcNow.Ticks);

        // Загружаем врагов, если есть сохранение
        _enemyDataList = SaveManager.LoadEnemies();
        if (_enemyDataList.Count > 0)
        {
            RespawnEnemies();
            Debug.Log($"Есть сохранение, загружаем... | {_enemyDataList.Count}");
        }
        else
        {
            SpawnAllEnemies();
            Debug.Log($"Нет сохранения | {_enemyDataList.Count}");
        }
    }

    void SpawnAllEnemies()
    {
        //Для каждого врага выбираем случайную зону, и случайное место в зоне спавна
        foreach (var enemyPrefab in enemyGameObjects)
        {
            var randomZoneIndex = _random.NextInt(0, _spawnZones2D.Length);
            var spawnZone = _spawnZones2D[randomZoneIndex];

            var bounds = spawnZone.bounds;
            var posX = _random.NextFloat(bounds.min.x, bounds.max.x);
            var posY = _random.NextFloat(bounds.min.y, bounds.max.y);
            var spawnPosition = new Vector3(posX, posY, 0);

            var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            var enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                string uniqueId = Guid.NewGuid().ToString();
                enemyHealth.Id = uniqueId; // Передаем ID в компонент врага
                RegisterEnemy(enemyHealth);
                _enemyDataList.Add(new SaveManager.EnemyData(uniqueId, enemyPrefab.name, spawnPosition, true));
            }
        }

        SaveManager.SaveEnemies(_enemyDataList);
    }

    void RespawnEnemies()
    {
        foreach (var enemyData in _enemyDataList)
        {
            if (!enemyData.IsAlive) continue;

            var enemyPrefab = FindEnemyPrefabByName(enemyData.EnemyType);
            if (enemyPrefab == null)
            {
                Debug.LogError($"Префаб для врага {enemyData.EnemyType} не найден.");
                continue;
            }

            var enemy = Instantiate(enemyPrefab, enemyData.Position, Quaternion.identity);
            var enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.Id = enemyData.Id; // Передаем ID из данных
                RegisterEnemy(enemyHealth);
            }
        }
    }

    void RegisterEnemy(EnemyHealth enemy)
    {
        _activeEnemies.Add(enemy);
        enemy.OnEnemyDied += HandleEnemyDeath; // Подписываемся на событие
    }

    private void HandleEnemyDeath(EnemyHealth enemy)
    {
        _activeEnemies.Remove(enemy);

        Debug.Log($"Враг {enemy.Id} удален из активного списка.");

        // Обновляем данные врага в списке
        foreach (var enemyData in _enemyDataList)
        {
            if (enemyData.Id == enemy.Id)
            {
                enemyData.IsAlive = false;
                break;
            }
        }

        SaveManager.SaveEnemies(_enemyDataList);
    }

    private void OnDestroy()
    {
        SaveManager.SaveEnemies(_enemyDataList);
        foreach (var enemy in _activeEnemies)
        {
            enemy.OnEnemyDied -= HandleEnemyDeath;
        }
    }

    GameObject FindEnemyPrefabByName(string prefabName)
    {
        foreach (var prefab in enemyGameObjects)
        {
            if (prefab.name == prefabName) return prefab;
        }

        return null;
    }
}
