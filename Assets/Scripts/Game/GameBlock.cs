using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class GameBlock : MonoBehaviour
    {
        // can be visabled in the inspector
        [SerializeField] private Color m_color;
        [SerializeField] private Sprite m_noughtSprite;
        [SerializeField] private Sprite m_crossSprite;
        [SerializeField] private SpriteRenderer m_markerSpriteRenderer;
        [SerializeField] private GameObject m_highlight;

        public static event Action<Cell, Transform, Transform> OnMouseClicked;

        public Cell Cell => Util.StringToCell(transform.name);

        private SpriteRenderer m_spriteRenderer;
        private Side m_owner = Side.None;

        public Side Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                UpdateMarkerSprite();
            }
        }

        private void UpdateMarkerSprite()
        {
            switch (m_owner)
            {
                case Side.Cross:
                    m_markerSpriteRenderer.sprite = m_crossSprite;
                    break;
                case Side.Circle:
                    m_markerSpriteRenderer.sprite = m_noughtSprite;
                    break;
                default:
                    m_markerSpriteRenderer.sprite = null;
                    break;
            }
        }

        private void Start()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_spriteRenderer.color = m_color;
        }

        private void OnMouseUpAsButton()
        {
            if (enabled)
            {
                // from -> to
                Transform thisTransform = transform; ;
                Transform toTransform = transform; // TODO: IMPLEMENT ME
                OnMouseClicked?.Invoke(Cell, thisTransform, toTransform);
            }
        }

        private void OnMouseEnter()
        {
            if(enabled)
            {
                m_highlight.SetActive(true);
            }
        }

        private void OnMouseExit()
        {
            if(enabled)
            {
                m_highlight.SetActive(false);
            }
        }

    }
} // namespace TicTacToe
