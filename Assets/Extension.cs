using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;

public static class Extensions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T element in source)
        {
            action(element);
        }
    }

    public static void ForEachNotNull<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T element in source)
        {
            if (element == null) continue;
            action(element);
        }
    }

    private static void uwotm8()
    {
        if (double.NaN.Equals(double.NaN) != (double.NaN == double.NaN))
        {
            Console.Write("WHAT");
        }
    }
}

public class OptionDataExt : Dropdown.OptionData
{
    public double ComponentID;
}

public partial class WeaponComponent : ICloneable
{
    public object Clone() {
        WeaponComponent clone = new WeaponComponent();
        typeof (WeaponComponent).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .ForEach(f => f.SetValue(clone, f.GetValue(this)));
        return clone;
    }
}