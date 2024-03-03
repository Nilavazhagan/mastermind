using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mastermind
{
    [RequireComponent(typeof(Image))]
    public class Peg : MonoBehaviour
    {
        Image imageRenderer;
        public CodePegColor pegColor { get; private set; }
        int colorIndex;
        // Start is called before the first frame update
        void Awake()
        {
            imageRenderer = GetComponent<Image>();
            List<CodePegColor> codePegColors = GameManager.Instance.GetAllCodePegColors();
            SetColor(codePegColors[0]);
        }

        public void SetColor(CodePegColor codePegColor)
        {
            colorIndex = GameManager.Instance.GetAllCodePegColors().IndexOf(codePegColor);
            pegColor = codePegColor;
            imageRenderer.color = codePegColor.color;
        }

        public void ChangeColor()
        {
            List<CodePegColor> codePegColors = GameManager.Instance.GetAllCodePegColors();
            colorIndex = (colorIndex + 1) % codePegColors.Count;
            SetColor(codePegColors[colorIndex]);
        }
    }
}