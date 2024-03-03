using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastermind
{
    [CreateAssetMenu(menuName = "Mastermind/Code Peg Color")]
    public class CodePegColor : ScriptableObject
    {
        public Color color;

        public override string ToString()
        {
            return this.name;
        }
    }
}