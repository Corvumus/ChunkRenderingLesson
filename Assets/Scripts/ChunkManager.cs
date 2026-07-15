using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private Transform playerTransform;

    [Header("Settings")]
    [SerializeField] private int chunkRenderDistance = 8; //Количество отображаемых чанков с каждой стороны
    [SerializeField] private int chunkSize = 30; //Длина чанка в юнитах

    //Ключ - позиция чанка на сетке. Значение - чанк на этой позиции 
    private readonly Dictionary<Vector2Int, GameObject> chunkDict = new();
    //Список отображаемых чанков
    private readonly List<GameObject> activeChunks = new();
    //Позиция чанка, на котором находится игрок
    private Vector2Int currentPlayerGridPosition;
    private float halfChunkSize;

    private void Awake()
    {
        halfChunkSize = chunkSize * 0.5f;
        UpdateChunks();
    }

    private void Update()
    {
        Vector2Int newPlayerGridPosition = GetGridPos(playerTransform.position);

        if (currentPlayerGridPosition != newPlayerGridPosition)
        { 
            currentPlayerGridPosition = newPlayerGridPosition;
            UpdateChunks();
        }
    }

    private void UpdateChunks()
    {
        HashSet<GameObject> chunksToRemove = new(activeChunks);
        activeChunks.Clear();
        //Вычисляем границы области, в которой рендерим чанки
        int minX = currentPlayerGridPosition.x - chunkRenderDistance;
        int maxX = currentPlayerGridPosition.x + chunkRenderDistance;
        int minZ = currentPlayerGridPosition.y - chunkRenderDistance;
        int maxZ = currentPlayerGridPosition.y + chunkRenderDistance;

        for (int x = minX; x <= maxX; x++)
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                Vector2Int chunkGridPos = new(x, z);

                GameObject chunkGO = CreateOrEnableChunk(chunkGridPos);

                activeChunks.Add(chunkGO);
                chunksToRemove.Remove(chunkGO);
            }
        }

        //Выключаем чанки за пределами дальности прорисовки
        foreach (var chunk in chunksToRemove)
        {
            chunk.SetActive(false);
        }
    }

    private GameObject CreateOrEnableChunk(Vector2Int chunkGridPos)
    {
        //Пытаемся найти чанк в словаре по его позиции
        if (chunkDict.TryGetValue(chunkGridPos, out GameObject chunkGO))
        {
            chunkGO.SetActive(true);
            return chunkGO;
        }
        //Если не находим, то создаём новый
        else
        {
            Vector3 chunkPos = new(chunkGridPos.x * chunkSize, 0, chunkGridPos.y * chunkSize);
            GameObject newChunk = Instantiate(chunkPrefab, chunkPos, Quaternion.identity);
            chunkDict.Add(chunkGridPos, newChunk);
            return newChunk;
        }
    }

    private Vector2Int GetGridPos(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.FloorToInt((pos.x + halfChunkSize) / chunkSize),
            Mathf.FloorToInt((pos.z + halfChunkSize) / chunkSize));
    }
}
