using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WeatherOrNot.Utils
{
    [RequireComponent(typeof(Collider))]
    public abstract class AbstractButtonPresenter : MonoBehaviour, IPointerClickHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        public event Action OnPress;
        public event Action OnRelease;
        public event Action OnClick;

        protected virtual void Awake()
        {
            EnsureCollider();
        }

        private void EnsureCollider()
        {
            if (GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
            OnSelect();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPress?.Invoke();
            OnPressed();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnRelease?.Invoke();
            OnReleased();
        }

        protected virtual void OnSelect()
        {
        }

        protected virtual void OnPressed()
        {
        }

        protected virtual void OnReleased()
        {
        }
    }
}