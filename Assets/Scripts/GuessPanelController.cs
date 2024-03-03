using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mastermind
{
    public class GuessPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject guessRowPrefab;
        [Space, SerializeField] private GameObject guessPegPrefab;

        [Space, SerializeField] private GameObject keyPegPrefab;
        [SerializeField] private Color whiteKeyColor = Color.white;
        [SerializeField] private Color redKeyColor = Color.red;

        List<GameObject> guessRows;
        RectTransform rectTransform;
        // Start is called before the first frame update
        void Awake()
        {
            guessRows = new List<GameObject>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void PopulateGuessRows(int numOfTurns)
        {
            Clean();
            for(int i = 0; i < numOfTurns; i++)
            {
                guessRows.Add(Instantiate(guessRowPrefab, transform));
            }
        }

        public void ShowGuess(List<CodePegColor> code, int turn)
        {
            int index = turn - 1;
            GameObject guessRow = guessRows[index];
            Transform guessParent = guessRow.transform.Find("Guess");

            foreach (CodePegColor codePegColor in code)
            {
                Instantiate(guessPegPrefab, guessParent).GetComponent<Peg>().SetColor(codePegColor);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        public void ShowMatch(MatchResult match, int turn)
        {
            int index = turn - 1;
            GameObject guessRow = guessRows[index];
            Transform matchParent = guessRow.transform.Find("Match");

            int redKey = match.redKey, whiteKey = match.whiteKey;

            for (int i = 0; i < GameManager.Instance.NumOfPegs; i++)
            {
                GameObject keyPeg = Instantiate(keyPegPrefab, matchParent);
                if (redKey > 0)
                {
                    keyPeg.GetComponent<Image>().color = redKeyColor;
                    redKey--;
                }
                else if(whiteKey > 0)
                {
                    keyPeg.GetComponent<Image>().color = whiteKeyColor;
                    whiteKey--;
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        void Clean()
        {
            foreach (GameObject guessRow in guessRows)
            {
                Destroy(guessRow);
            }
            guessRows.Clear();
        }
    }
}