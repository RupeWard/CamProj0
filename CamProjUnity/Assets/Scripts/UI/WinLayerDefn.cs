using UnityEngine;
using System.Collections;

namespace RW.Win
{
    [RequireComponent (typeof(RectTransform))]
    public class WinLayerDefn : MonoBehaviour
    {
        static private readonly bool DEBUG_LOCAL = true;

        #region Interface

        public int LayerNum
        {
            get { return layerNum_; }
        }

        #endregion Interface

        #region cached hooks

        private RectTransform rectTransform_;

        #endregion cached hooks

        #region private data

        private WinLayerManager winLayerManager_;
        private int layerNum_;

        #endregion private data

        #region MonoBehaviour

        private void Awake()
        {
            rectTransform_ = GetComponent< RectTransform >();
        }

        #endregion MonoBehaviour

        #region Setup

        public void Init(WinLayerManager wlm)
        {
            winLayerManager_ = wlm;
            layerNum_ = wlm.NumLayers;
            gameObject.name = "Layer_" + layerNum_.ToString("00");
            rectTransform_.SetParent(winLayerManager_.RectTransform);
            rectTransform_.anchoredPosition = Vector2.zero;
            rectTransform_.localScale = Vector3.one;

            if (DEBUG_LOCAL)
            {
                Debug.Log("WLD: Init " + layerNum_ + " " + gameObject.name);
            }
        }

        #endregion Setup
    }
}

