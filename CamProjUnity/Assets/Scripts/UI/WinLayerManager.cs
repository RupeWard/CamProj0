using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RW.Win
{
    [RequireComponent (typeof(RectTransform))]
    public class WinLayerManager : MonoBehaviour
    {
        private static readonly bool DEBUG_LOCAL = true;

        #region Interface

        public int NumLayers
        {
            get { return winLayerDefns_.Count;  }
        }

        #endregion Interface

        #region settings

        #region settings

        public int numStartingLayers = 0;

        #endregion inspector hooks

        public GameObject winLayerPrefab;

        #endregion inspector hooks

        #region cached hooks

        private RectTransform rectTransform_;
        public RectTransform RectTransform
        {
            get { return rectTransform_; }
        }

        #endregion cached hooks

        #region private data

        private List<WinLayerDefn> winLayerDefns_ = new List<WinLayerDefn>();

        #endregion private data


        #region MonoBehaviour

        private void Awake()
        {
            rectTransform_ = GetComponent<RectTransform>();

            if (numStartingLayers > 0)
            {
                if (DEBUG_LOCAL)
                {
                    Debug.Log("WLM: Adding "+numStartingLayers+" starting layers");                       
                }
                for (int i = 0; i < numStartingLayers; i++)
                {
                    AddLayer();
                }
            }
        }

        #endregion MonoBehaviour

        #region Setup

        private WinLayerDefn AddLayer()
        {
            GameObject go = Instantiate(winLayerPrefab) as GameObject;
            WinLayerDefn wld = go.GetComponent<WinLayerDefn>();
            wld.Init(this);
            winLayerDefns_.Add(wld);

            if (DEBUG_LOCAL)
            {
                Debug.Log("WLM: Added layer " + NumLayers);
            }
            return wld;
        }

        #endregion Setup
    }

}

