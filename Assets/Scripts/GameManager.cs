using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mastermind
{
    public class GameManager : SingletonBehavior<GameManager>
    {

        [SerializeField] private int numOfPegs = 4;
        public int NumOfPegs => numOfPegs;

        [SerializeField] private Transform codeMakerPegs;
        [SerializeField] private GameObject pegPrefab;

        [Space, SerializeField] private int maxNumberOfTurns = 10;

        [Space, SerializeField] private GuessPanelController guessPanelController;

        [Space, SerializeField] private GameObject resultPanel;
        [SerializeField] private Text resultText;

        MastermindAI AI;
        List<CodePegColor> codePegColors;
        Peg[] pegs;
        Judge judge;
        int currentTurn = 1;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            pegs = new Peg[numOfPegs];

            codePegColors = Resources.LoadAll<CodePegColor>("").ToList();
            codePegColors = codePegColors.OrderBy(codePegColor => codePegColor.name).ToList();
            /*foreach (CodePegColor codePegColor in codePegColors)
                Debug.Log(codePegColor);*/

            AI = FindObjectOfType(typeof(MastermindAI)) as MastermindAI;
            judge = new Judge();
            CreateCodeMakerPegs();
        }

        public List<CodePegColor> GetAllCodePegColors()
        {
            return codePegColors;
        }

        private void CreateCodeMakerPegs()
        {
            for (int i = 0; i < numOfPegs; i++)
            {
                GameObject peg = Instantiate(pegPrefab, codeMakerPegs);
                pegs[i] = peg.GetComponent<Peg>();
            }
        }

        public void StartGame()
        {
            judge.SetCode(pegs);
            AI.Init(GetAllCodePegColors(), numOfPegs);
            currentTurn = 1;
            guessPanelController.PopulateGuessRows(maxNumberOfTurns);
            PlayTurn();
        }

        void PlayTurn(MatchResult prevMatch = null)
        {
            if (prevMatch == null)
                AI.Guess();
            else
                AI.Guess((MatchResult)prevMatch);
        }

        public void CheckMatch(List<CodePegColor> guessedCode)
        {
            guessPanelController.ShowGuess(guessedCode, currentTurn);
            MatchResult match = judge.Evaluate(guessedCode);
            guessPanelController.ShowMatch(match, currentTurn);

            if (match.CompleteMatch)
            {
                //showWin
                resultPanel.SetActive(true);
                resultText.text = $"AI cracked code in {currentTurn} moves";
                return;
            }
            else if (currentTurn == maxNumberOfTurns)
            {
                //showLoss
                resultPanel.SetActive(true);
                resultText.text = "AI was unable to crack the code";
                return;
            }

            currentTurn++;
            PlayTurn(match);
        }

        public void Reset()
        {
            foreach (Peg peg in pegs)
            {
                peg.SetColor(codePegColors[0]);
            }

            AI.Reset();
        }

        private class Judge
        {
            List<CodePegColor> code;

            public void SetCode(Peg[] codeMakerPegs)
            {
                code = new List<CodePegColor>();
                for (int i = 0, length = codeMakerPegs.Length; i < length; i++)
                {
                    code.Add(codeMakerPegs[i].pegColor);
                }
            }

            public void SetCode(List<CodePegColor> codePegs)
            {
                code = codePegs;
            }

            public MatchResult Evaluate(List<CodePegColor> guessedCode)
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
        }
    }
}
