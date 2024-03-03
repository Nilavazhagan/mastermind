using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastermind
{
    public class MastermindAI_FiveGuess : MastermindAI_MethodOfElimination
    {
        HashSet<int> GuessedCodeIndices = new HashSet<int>();
        MatchResult previousMatch;
        const int ITERATIONS_PER_FRAME = 10;

        public override void Guess()
        {
            MakeNextGuess();   
        }
        public override void Guess(MatchResult match)
        {
            previousMatch = match;
            base.Guess(match);
        }

        void MakeNextGuess()
        {
            List<CodePegColor> nextCode;

            if (SolutionSpace.Count == 0)
            {
                int[] prelimIndices = new int[numPegs];
                for (int i = (int)Mathf.Ceil((float)numPegs / 2); i < numPegs; i++)
                {
                    prelimIndices[i] = 1;
                }
                nextCode = GetCodeForIndices(prelimIndices);
                GuessedCodeIndices.Add(ToBase10(prelimIndices, numColors));
                SetNextGuess(nextCode);
            }
            else
            {
                StartCoroutine(GetNextCodeOverFrames());

            }
        }

        IEnumerator GetNextCodeOverFrames()
        {
            List<CodePegColor> nextCode;

            int minScore = int.MaxValue;
            HashSet<int> minScoreIndices = new HashSet<int>();
            for (int i = 0; i < maxCombinations; i++)
            {
                if (GuessedCodeIndices.Contains(i)) continue;

                List<CodePegColor> potentialCode = GetCodeForIndex(i);
                Dictionary<string, int> hitMap = new Dictionary<string, int>();
                foreach (int index in SolutionSpace)
                {
                    string matchRes = CompareCodes(potentialCode, GetCodeForIndex(index)).ToString();
                    if (!hitMap.ContainsKey(matchRes))
                    {
                        hitMap.Add(matchRes, 0);
                    }

                    hitMap[matchRes] += 1;
                }

                int maxHitCount = 0;
                foreach (KeyValuePair<string, int> entry in hitMap)
                {
                    if (entry.Value > maxHitCount)
                    {
                        maxHitCount = entry.Value;
                    }
                }

                int score = maxHitCount;

                if (score < minScore)
                {
                    minScore = score;
                    minScoreIndices.Clear();
                    minScoreIndices.Add(i);
                }
                else if (score == minScore)
                {
                    minScoreIndices.Add(i);
                }

                if (i % ITERATIONS_PER_FRAME == 0)
                {
                    yield return null;
                }
            }

            int codeIndex = -1;

            foreach (int index in minScoreIndices)
            {
                if (codeIndex == -1)
                {
                    codeIndex = index;
                }

                if (SolutionSpace.Contains(index))
                {
                    codeIndex = index;
                    break;
                }
            }

            GuessedCodeIndices.Add(codeIndex);
            nextCode = GetCodeForIndex(codeIndex);
            SetNextGuess(nextCode);
        }

        public override void Reset()
        {
            base.Reset();
            GuessedCodeIndices.Clear();
            previousMatch = null;
        }

        int ToBase10(int[] baseNValue, int n)
        {
            int value = 0;
            for(int i = 0, length = baseNValue.Length; i < length; i++)
            {
                value += baseNValue[(length - 1) - i] * (int)Mathf.Pow(n, i);
            }
            return value;
        }
    }
}