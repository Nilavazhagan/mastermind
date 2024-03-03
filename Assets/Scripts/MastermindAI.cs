using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastermind
{
    public abstract class MastermindAI : MonoBehaviour
    {
        protected int numColors, numPegs, maxCombinations;
        protected List<CodePegColor> allPegColors;
        public abstract void Guess();
        public abstract void Guess(MatchResult match);

        public virtual void Reset() { }

        public virtual void Init(List<CodePegColor> allPegColors, int numOfPegs)
        {
            this.allPegColors = allPegColors;
            this.numColors = allPegColors.Count;
            this.numPegs = numOfPegs;
            this.maxCombinations = (int)Mathf.Pow(numColors, numPegs);
        }
    }
}