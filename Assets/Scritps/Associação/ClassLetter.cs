using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LetraData", menuName = "ScriptableObjects/LetraData", order = 1)]
public class ClassLetter : ScriptableObject
{
    [Tooltip("A letra representada pelo objeto")]
    public string letra;

    [Tooltip("O sprite correspondente à letra")]
    public Sprite sprite;

    public bool IsEqual(string compararCom)
    {
        return letra.Equals(compararCom, System.StringComparison.OrdinalIgnoreCase);
    }
}
