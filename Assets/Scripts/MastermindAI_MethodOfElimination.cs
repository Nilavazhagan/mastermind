using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastermind
{
    public class MastermindAI_MethodOfElimination : MastermindAI
    {
        protected HashSet<int> SolutionSpace = new HashSet<int>();
        List<CodePegColor> guessedCode;
        public override void Guess()
        {
            SetNextGuess(GetNextCode());
        }

        public override void Guess(MatchResult match)
        {
            ReduceSolutionSpace(match, guessedCode);
            Guess();
        }

        protected virtual List<CodePegColor> GetNextCode()
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
            }
            else
            {
                nextCode = GetCodeForIndex(SolutionSpace.First());
            }

            return nextCode;
        }

        public void ReduceSolutionSpace(MatchResult match, List<CodePegColor> guessedCode)
        {
            if (SolutionSpace.Count == 0)
            {
                for (int i = 0; i < maxCombinations; i++)
                {
                    if (match.Equals(CompareCodes(GetCodeForIndex(i), guessedCode)))
                    {
                        SolutionSpace.Add(i);
                    }
                }
            }
            else
            {
                HashSet<int> tempSet = new HashSet<int>();
                foreach (int i in SolutionSpace)
                {
                    if (match.Equals(CompareCodes(GetCodeForIndex(i), guessedCode)))
                    {
                        tempSet.Add(i);
                    }
                }
                SolutionSpace = tempSet;
            }
        }

        protected MatchResult CompareCodes(List<CodePegColor> code, List<CodePegColor> guessedCode)
        {
            if (guessedCode.Count != code.Count)
            {
                throw new System.Exception("Code Length Mismatch");
            }

            MatchResult matchResult = new MatchResult();
            Dictionary<int, int> matchIndices = new Dictionary<int, int>();
            for (int i = 0, length = code.Count; i < length; i++)
            {
                if (code[i] == guessedCode[i])
                {
                    matchIndices.Add(i, i);
                    matchResult.redKey++;
                }
            }

            for (int i = 0, length = code.Count; i < length; i++)
            {
                if (!matchIndices.ContainsKey(i))
                {
                    for (int j = 0; j < length; j++)
                    {
                        if (code[i] == guessedCode[j] && !matchIndices.ContainsValue(j))
                        {
                            matchIndices.Add(i, j);
                            matchResult.whiteKey++;
                            break;
                        }
                    }
                }
            }

            return matchResult;
        }

        Dictionary<int, List<CodePegColor>> IndexToCodeMap = new Dictionary<int, List<CodePegColor>>();
        protected List<CodePegColor> GetCodeForIndex(int index)
        {
            if (!IndexToCodeMap.ContainsKey(index))
            {
                IndexToCodeMap[index] = GetCodeForIndices(ToBaseN(index, numColors, numPegs));
            }
            return IndexToCodeMap[index];
        }

        protected List<CodePegColor> GetCodeForIndices(int[] indices)
        {
            List<CodePegColor> code = new List<CodePegColor>();

            for (int i = 0, length = indices.Length; i < length; i++)
            {
                code.Add(allPegColors[indices[i]]);
            }

            return code;
        }

        public override void Reset()
        {
            base.Reset();
            SolutionSpace.Clear();
        }

        public int[] ToBaseN(int decimalNumber, int n, int digits)
        {
            int[] baseNValue = new int[digits];

            int index = digits - 1;
            while (decimalNumber > 0)
            {
                baseNValue[index] = decimalNumber % n;
                decimalNumber = decimalNumber / n;
                index--;
            }

            return baseNValue;
        }

        protected void SetNextGuess(List<CodePegColor> guessedCode)
        {
            this.guessedCode = guessedCode;
            GameManager.Instance.CheckMatch(guessedCode);
        }
    }
}