using System.ComponentModel;
using UnityEngine;

namespace GameFramework
{
    public class CoreDefinition : ScriptableObject
    {
        [ReadOnly(true)] public long InternalIdentifier;
        public string DisplayName;
        [TextArea] public string FlavourText = "NO DESCRIPTION FOUND!";
    }
}