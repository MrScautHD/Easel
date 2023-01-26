using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Easel.Math;

[StructLayout(LayoutKind.Sequential)]
public struct Vector2T<T> : IEquatable<Vector2T<T>> where T : INumber<T>
{
    public static Vector2T<T> Zero => new Vector2T<T>(T.Zero);

    public static Vector2T<T> One => new Vector2T<T>(T.One);

    public static Vector2T<T> UnitX => new Vector2T<T>(T.One, T.Zero);

    public static Vector2T<T> UnitY => new Vector2T<T>(T.Zero, T.One);

    public T X;

    public T Y;

    public Vector2T(T scalar)
    {
        X = scalar;
        Y = scalar;
    }

    public Vector2T(T x, T y)
    {
        X = x;
        Y = y;
    }
    
    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2T<T> operator +(Vector2T<T> left, Vector2T<T> right) =>
        new Vector2T<T>(left.X + right.X, left.Y + right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2T<T> operator -(Vector2T<T> left, Vector2T<T> right) =>
        new Vector2T<T>(left.X - right.X, left.Y - right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2T<T> operator *(Vector2T<T> left, Vector2T<T> right) =>
        new Vector2T<T>(left.X * right.X, left.Y * right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2T<T> operator *(Vector2T<T> left, T right) =>
        new Vector2T<T>(left.X * right, left.Y * right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2T<T> operator *(T left, Vector2T<T> right) =>
        new Vector2T<T>(left * right.X, left * right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2T<T> operator /(Vector2T<T> left, Vector2T<T> right) =>
        new Vector2T<T>(left.X / right.X, left.Y / right.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2T<T> operator /(Vector2T<T> left, T right) =>
        new Vector2T<T>(left.X / right, left.Y / right);

    #endregion

    public bool Equals(Vector2T<T> other)
    {
        return EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector2T<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(Vector2T<T> left, Vector2T<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2T<T> left, Vector2T<T> right)
    {
        return !left.Equals(right);
    }

    public static implicit operator Vector2(Vector2T<T> vector)
    {
        float x = Convert.ToSingle(vector.X);
        float y = Convert.ToSingle(vector.Y);
        return new Vector2(x, y);
    }
    
    public static implicit operator Vector2T<T>(Vector2 vector)
    {
        return new Vector2T<T>(T.CreateChecked(vector.X), T.CreateChecked(vector.Y));
    }
}

public static class Vector2T
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Magnitude<T>(Vector2T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        return T.Sqrt(MagnitudeSquared(vector));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MagnitudeSquared<T>(Vector2T<T> vector) where T : INumber<T>
    {
        return Dot(vector, vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Dot<T>(Vector2T<T> left, Vector2T<T> right) where T : INumber<T>
    {
        return T.CreateChecked(left.X * right.X + left.Y * right.Y);
    }
}