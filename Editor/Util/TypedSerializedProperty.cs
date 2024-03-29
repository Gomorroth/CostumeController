﻿using System;
using UnityEngine;
using UnityEditor;
namespace gomoru.su.CostumeController
{
    internal readonly struct SerializedProperty<T>
    {
        private readonly SerializedProperty @base;
        public SerializedProperty(SerializedProperty @base) => this.@base = @base;
        public SerializedProperty Base => @base;
        public T Value
        {
            get
            {
                if (typeof(T) == typeof(sbyte)) return (T)(object)(sbyte)@base.intValue;
                if (typeof(T) == typeof(byte)) return (T)(object)(byte)@base.uintValue;
                if (typeof(T) == typeof(short)) return (T)(object)(short)@base.intValue;
                if (typeof(T) == typeof(ushort)) return (T)(object)(ushort)@base.uintValue;
                if (typeof(T) == typeof(int)) return (T)(object)@base.intValue;
                if (typeof(T) == typeof(uint)) return (T)(object)@base.uintValue;
                if (typeof(T) == typeof(long)) return (T)(object)@base.longValue;
                if (typeof(T) == typeof(ulong)) return (T)(object)@base.ulongValue;
                if (typeof(T) == typeof(float)) return (T)(object)@base.floatValue;
                if (typeof(T) == typeof(double)) return (T)(object)@base.doubleValue;
                if (typeof(T) == typeof(bool)) return (T)(object)@base.boolValue;
                if (typeof(T) == typeof(string)) return (T)(object)@base.stringValue;
                if (typeof(T) == typeof(Color)) return (T)(object)@base.colorValue;
                if (typeof(T) == typeof(Vector2)) return (T)(object)@base.vector2Value;
                if (typeof(T) == typeof(Vector2Int)) return (T)(object)@base.vector2IntValue;
                if (typeof(T) == typeof(Vector3)) return (T)(object)@base.vector3Value;
                if (typeof(T) == typeof(Vector3Int)) return (T)(object)@base.vector3IntValue;
                if (typeof(T) == typeof(Vector4)) return (T)(object)@base.vector4Value;
                if (typeof(T) == typeof(Rect)) return (T)(object)@base.rectValue;
                if (typeof(T) == typeof(RectInt)) return (T)(object)@base.rectIntValue;
                if (typeof(T) == typeof(char)) return (T)(object)(char)(ushort)@base.uintValue;
                if (typeof(T) == typeof(AnimationCurve)) return (T)(object)@base.animationCurveValue;
                if (typeof(T) == typeof(Bounds)) return (T)(object)@base.boundsValue;
                if (typeof(T) == typeof(BoundsInt)) return (T)(object)@base.boundsIntValue;
                if (typeof(T) == typeof(Gradient)) return (T)(object)@base.gradientValue;
                if (typeof(T) == typeof(Bounds)) return (T)(object)@base.boundsValue;
                if (typeof(T) == typeof(Hash128)) return (T)(object)@base.hash128Value;
                return (T)@base.boxedValue;
            }
            set
            {
                if (typeof(T) == typeof(sbyte)) @base.intValue = (int)(object)value;
                else if (typeof(T) == typeof(byte)) @base.uintValue = (byte)(object)value;
                else if (typeof(T) == typeof(short)) @base.intValue = (short)(object)value;
                else if (typeof(T) == typeof(ushort)) @base.uintValue = (ushort)(object)value;
                else if (typeof(T) == typeof(int)) @base.intValue = (int)(object)value;
                else if (typeof(T) == typeof(uint)) @base.uintValue = (uint)(object)value;
                else if (typeof(T) == typeof(long)) @base.longValue = (long)(object)value;
                else if (typeof(T) == typeof(ulong)) @base.ulongValue = (ulong)(object)value;
                else if (typeof(T) == typeof(float)) @base.floatValue = (float)(object)value;
                else if (typeof(T) == typeof(double)) @base.doubleValue = (double)(object)value;
                else if (typeof(T) == typeof(bool)) @base.boolValue = (bool)(object)value;
                else if (typeof(T) == typeof(string)) @base.stringValue = (string)(object)value;
                else if (typeof(T) == typeof(Color)) @base.colorValue = (Color)(object)value;
                else if (typeof(T) == typeof(Vector2)) @base.vector2Value = (Vector2)(object)value;
                else if (typeof(T) == typeof(Vector2Int)) @base.vector2IntValue = (Vector2Int)(object)value;
                else if (typeof(T) == typeof(Vector3)) @base.vector3Value = (Vector3)(object)value;
                else if (typeof(T) == typeof(Vector3Int)) @base.vector3IntValue = (Vector3Int)(object)value;
                else if (typeof(T) == typeof(Vector4)) @base.vector4Value = (Vector4)(object)value;
                else if (typeof(T) == typeof(Rect)) @base.rectValue = (Rect)(object)value;
                else if (typeof(T) == typeof(RectInt)) @base.rectIntValue = (RectInt)(object)value;
                else if (typeof(T) == typeof(char)) @base.uintValue = (char)(object)value;
                else if (typeof(T) == typeof(AnimationCurve)) @base.animationCurveValue = (AnimationCurve)(object)value;
                else if (typeof(T) == typeof(Bounds)) @base.boundsValue = (Bounds)(object)value;
                else if (typeof(T) == typeof(BoundsInt)) @base.boundsIntValue = (BoundsInt)(object)value;
                else if (typeof(T) == typeof(Gradient)) @base.gradientValue = (Gradient)(object)value;
                else if (typeof(T) == typeof(Bounds)) @base.boundsValue = (Bounds)(object)value;
                else if (typeof(T) == typeof(Hash128)) @base.hash128Value = (Hash128)(object)value;
                else @base.boxedValue = value;
            }
        }
        public static implicit operator SerializedProperty(in SerializedProperty<T> property) => property.@base;
    }
}