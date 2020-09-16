using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeManager
{
    private float[,] attackValues;
    private float[,] defenseValues;

    public TypeManager()
    {
        attackValues = new float[6, 6];
        fillLine(ref attackValues, 0, new float[] { 1, 1, 1, 1, 1 });
        fillLine(ref attackValues, 1, new float[] { 1, 0.5f, 1, 0.5f, 1.5f });
        fillLine(ref attackValues, 2, new float[] { 1, 0.5f, 1, 1.5f, 0.5f });
        fillLine(ref attackValues, 3, new float[] { 1, 1.5f, 0.5f, 1, 1.5f });
        fillLine(ref attackValues, 4, new float[] { 1, 1, 1, 0.5f, 1.5f });
        fillLine(ref attackValues, 5, new float[] { 1, 1, 1, 1.5f, 0.5f });
        defenseValues = attackValues.Clone() as float[,];
    }

    public float EffectValue(Type attack,Type defense)
    {
        int attackNum = convert(attack);
        int defenseNum= convert(defense);
        return attackValues[attackNum, defenseNum] * defenseValues[attackNum, defenseNum];
    }

    private int convert(Type type)
    {
        switch(type)
        {
            case (Type.DarkMatter): return 5;
            case (Type.LightMatter): return 4;
            case (Type.Gravity): return 3;
            case (Type.Nebula): return 2;
            case (Type.Sun): return 1;
            case (Type.Star): return 0;
            default: return 0;
        }
    }

    private void fillLine(ref float[,] matrix,int index,float[] values)
    {

    }
}

public enum Type : int
{
    Star = 0,
    Sun = 1,
    Nebula = 2,
    Gravity = 3,
    LightMatter = 4,
    DarkMatter = 5
}
