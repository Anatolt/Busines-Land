using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extentions
{
    public static Vector2 Rotate(this Vector2 vector, float eulerAngle)
    {
        //x1 = x * cos(angle) - y * sin(angle);
        //y1 = y * cos(angle) + x * sin(angle);

        eulerAngle *= Mathf.Deg2Rad;

        var x = vector.x;
        var y = vector.y;

        return new Vector2()
        {
            x = x * Mathf.Cos(eulerAngle) - y * Mathf.Sin(eulerAngle),
            y = y * Mathf.Cos(eulerAngle) + x * Mathf.Sin(eulerAngle)
        };
    }

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static string GetSavedDataKey(this UnityEngine.Object savingObject) => savingObject.GetInstanceID().ToString();
}