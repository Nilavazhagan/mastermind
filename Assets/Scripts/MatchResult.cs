using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastermind
{
    public class MatchResult
    {
        public int whiteKey;
        public int redKey;

        public bool CompleteMatch
        {
            get
            {
                return redKey == GameManager.Instance.NumOfPegs;
            }
        }
        public bool Equals(MatchResult matchResult)
        {
            return whiteKey == matchResult.whiteKey && redKey == matchResult.redKey;
        }

        public override string ToString()
        {
            string res = "";

            for (int i = 0; i < redKey; i++) res += "R";
            for (int i = 0; i < whiteKey; i++) res += "W";

            return res;
        }
    }
}