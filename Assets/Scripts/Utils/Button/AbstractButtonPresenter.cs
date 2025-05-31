using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WeatherOrNot.Utils
{
    public abstract class AbstractButtonPresenter : BaseUIAnimatedView, IPointerClickHandler, IPointerDownHandler,
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
            if (GetComponent<Collider2D>() == null)
            {
                gameObject.AddComponent<BoxCollider2D>();
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