﻿using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class BlockBar : MonoBehaviour
    {
        public Image ImageCurrent;

        public void SetValue(float current, float max) =>
            ImageCurrent.fillAmount = current / max;
    }
}