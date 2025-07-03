using UnityEngine;

namespace TicTacToe
{
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindAnyObjectByType<T>() ?? new GameObject().AddComponent<T>();
                }
                return m_instance;
            }
        }

        //protected abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    }
} // namespace TicTacToe