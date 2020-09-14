using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Astra.Tracking
{
    public class OrbbecTrackingUserIcon : MonoBehaviour
    {
        [SerializeField] Image iconImage = null;
        [SerializeField] Image progressImage = null;
        [SerializeField] Sprite[] outIconSprites = null;
        [SerializeField] Sprite[] inIconSprites = null;

        private float _progress = 0.0f;

        public float progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                progressImage.fillAmount = _progress;
            }
        }

        private RectTransform _rectTransform = null;

        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        private int _id;

        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                UpdateView();
            }
        }

        private bool _isInInBox = false;

        public bool isInInBox
        {
            get
            {
                return _isInInBox;
            }
            set
            {
                _isInInBox = value;
                UpdateView();
            }
        }

        void UpdateView()
        {
            if (_id <= 0)
            {
                return;
            }

            if (_isInInBox)
            {
                iconImage.sprite = inIconSprites[(_id-1)%6];
            }
            else
            {
                iconImage.sprite = outIconSprites[(_id-1)%6];
            }
        }
    }

}