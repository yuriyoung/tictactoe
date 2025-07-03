using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TicTacToe
{
    public sealed class BoardManager : MonoBehaviourSingleton<BoardManager>
    {
        private Dictionary<Cell, GameObject> m_positionDict;

        private const float MaxIndex = (Constants.BoardSize - 1) * 1.0f;

        private void Awake()
        {
            GameManager.NewGameStartedEvent += OnNewGameStarted;

            m_positionDict = new Dictionary<Cell, GameObject>(9);
            Transform boardTransform = transform;
            Vector3 boardPosition = boardTransform.position;

            for (int row = 0; row < Constants.BoardSize; row++)
            {
                for (int col = 0; col < Constants.BoardSize; col++)
                {
                    m_positionDict[new Cell(row, col)] = null; // Initialize with null
                }
            }
        }

        private void OnDestroy()
        {
            GameManager.NewGameStartedEvent -= OnNewGameStarted;
        }

        public void PlaceBlockOnBoard(Block block, Cell position)
        {
            if (!m_positionDict.ContainsKey(position))
            {
                Debug.LogError($"Invalid position: {position}");
                return;
            }

            // ¿ÉÌæ»»£¿
            //if (m_positionDict[position] != null)
            //{
            //    Debug.LogWarning($"Position already occupied: {position}");
            //    return;
            //}

            string prefabPath = $"Prefabs/{block.GetType().Name}";
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found: {prefabPath}");
                return;
            }

            Vector3 spawnPosition = SpawnPosition(position.column, position.row, transform);
            GameObject blockObj = Instantiate(prefab, spawnPosition, Quaternion.Euler(90, 0, 0), transform);
            blockObj.name = position.ToString();
            m_positionDict[position] = blockObj;
        }

        public void SetAllBlocksActive(bool active)
        {
            GameBlock[] blocks = GetComponentsInChildren<GameBlock>(true);
            foreach (GameBlock block in blocks)
            {
                block.enabled = active;
                Collider collider = block.gameObject.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = active;
                }
            }
        }

        public GameObject GetBlockAtPosition(Cell position)
        {
            if (m_positionDict.TryGetValue(position, out GameObject block))
            {
                return block;
            }
            return null;
        }

        public void RemoveBlockAtPosition(Cell position)
        {
            if (m_positionDict.ContainsKey(position) && m_positionDict[position] != null)
            {
                Destroy(m_positionDict[position]);
                m_positionDict[position] = null;
            }
        }

        public bool IsPositionEmpty(Cell position)
        {
            return m_positionDict.ContainsKey(position) && m_positionDict[position] == null;
        }

        public void UpdateBlockMarker(Cell position, Side owner)
        {
            if (!m_positionDict.TryGetValue(position, out var blockObj) || blockObj == null)
            {
                Debug.LogWarning($"Position {position} not found or empty");
                return;
            }

            GameBlock block = blockObj.GetComponentInChildren<GameBlock>();
            if (block != null)
            {
                block.Owner = owner;
                block.enabled = false;
                if (block.TryGetComponent<Collider>(out var collider))
                {
                    collider.enabled = false;
                }
            }
        }

        private void ClearBoard()
        {
            GameBlock[] blocks = GetComponentsInChildren<GameBlock>(true);
            foreach (GameBlock block in blocks)
            {
                DestroyImmediate(block.gameObject);
            }
            foreach (var key in m_positionDict.Keys.ToList())
            {
                m_positionDict[key] = null;
            }
        }

        private static Vector3 SpawnPosition(int row, int column, Transform transform)
        {
            Vector3 boardPosition = transform.position;
            return new Vector3(
                        boardPosition.x + AdjustedPosition(row),
                        boardPosition.y + 0.1f,
                        boardPosition.z + AdjustedPosition(column)
                    );
        }

        private static float AdjustedPosition(int index)
        {
            const float Padding = 0.1f;
            float t = index / MaxIndex;
            return Mathf.Lerp(-MaxIndex - Padding, MaxIndex + Padding, t);
        }

        private void OnNewGameStarted()
        {
            ClearBoard();

            foreach((Cell cell, Block block) in GameManager.Instance.CurrentBlocks)
            {
                PlaceBlockOnBoard(block, cell);
            }
        }
    }
} // namespace TicTacToe
