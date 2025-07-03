using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TicTacToe
{
    // 没有使用，目的是可拖拽MessageBox之类的界面，防挡屏幕的作用
    public class UIDragHandler : MonoBehaviour
    {
        private RectTransform m_rectTransform;
        private Canvas m_canvas;
        private Vector2 m_offset;

        private void Awake()
        {
            Debug.Log("Drag awake");
            m_rectTransform = GetComponent<RectTransform>();
            m_canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_rectTransform, eventData.position, eventData.pressEventCamera, out m_offset
                );
            m_offset = m_rectTransform.anchoredPosition - m_offset;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag");
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                m_rectTransform.anchoredPosition = localPoint + m_offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");
        }
    }
}