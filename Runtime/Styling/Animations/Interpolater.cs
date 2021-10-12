using System;
using System.Runtime.CompilerServices;
using Facebook.Yoga;
using ReactUnity.Styling.Computed;
using ReactUnity.Types;
using UnityEngine;

namespace ReactUnity.Styling.Animations
{
    public static class Interpolater
    {
        #region Linear interpolations
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Interpolate(float t, bool mirror = false)
        {
            if (mirror && t < 0) return Mathf.Abs(t);
            else return t;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Interpolate(float from, float to, float t)
        {
            return TimingFunctions.Linear(t, from, to);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Interpolate(int from, int to, float t)
        {
            return Mathf.RoundToInt(TimingFunctions.Linear(t, from, to));
        }
        public static Color Interpolate(Color from, Color to, float t)
        {
            if (from == Color.clear || to == Color.clear)
            {
                if (from.a == 0) return new Color(to.r, to.g, to.b, Interpolate(0, to.a, t));
                return new Color(from.r, from.g, from.b, Interpolate(from.a, 0, t));
            }

            return Color.LerpUnclamped(from, to, t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Interpolate(Vector2 from, Vector2 to, float t)
        {
            return Vector2.LerpUnclamped(from, to, t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Interpolate(Vector3 from, Vector3 to, float t)
        {
            return Vector3.LerpUnclamped(from, to, t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Interpolate(Vector4 from, Vector4 to, float t)
        {
            return Vector4.LerpUnclamped(from, to, t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Interpolate(Quaternion from, Quaternion to, float t)
        {
            return Quaternion.SlerpUnclamped(from, to, t);
        }
        public static YogaValue Interpolate(YogaValue from, YogaValue to, float t)
        {
            var canAnimate = (from.Unit == YogaUnit.Point || from.Unit == YogaUnit.Percent) &&
                (from.Unit == to.Unit || from.Value == 0 || to.Value == 0);

            if (!canAnimate) return to;

            if (from.Value == 0 && to.Value == 0) return YogaValue.Point(0);

            var unit = from.Value != 0 ? from.Unit : to.Unit;

            if (unit == YogaUnit.Percent) return YogaValue.Percent(Interpolate(from.Value, to.Value, t));
            return YogaValue.Point(Interpolate(from.Value, to.Value, t));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static YogaValue2 Interpolate(YogaValue2 from, YogaValue2 to, float t)
        {
            return new YogaValue2(Interpolate(from.X, to.X, t), Interpolate(from.Y, to.Y, t));
        }

        public static BoxShadow Interpolate(BoxShadow from, BoxShadow to, float t)
        {
            if (from == null || to == null) return t > 0.5 ? to : from;

            if (from.inset != to.inset && from != BoxShadow.Default && to != BoxShadow.Default)
            {
                if (t < 0.5) return Interpolate(from, from.inset ? BoxShadow.DefaultInset : BoxShadow.Default, t * 2);
                else return Interpolate(to.inset ? BoxShadow.DefaultInset : BoxShadow.Default, to, (t - 0.5f) * 2);
            }

            return new BoxShadow(
                Interpolate(from.offset, to.offset, t),
                Interpolate(from.blur, to.blur, t),
                Interpolate(from.spread, to.spread, t),
                Interpolate(from.color, to.color, t),
                from != BoxShadow.Default ? from.inset : to.inset
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICssValueList<T> Interpolate<T>(ICssValueList<T> from, ICssValueList<T> to, float t)
        {
            return new CssValueListInterpolated<T>(from, to, t);
        }

        #endregion


        #region Enum interpolations
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Interpolate(float t, TimingFunctionType easeType, bool mirror = false)
        {
            if (mirror && t < 0) return TimingFunctions.Get(easeType)(Mathf.Abs(t));
            else return TimingFunctions.Get(easeType)(t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Interpolate(float from, float to, float t, TimingFunctionType easeType)
        {
            return TimingFunctions.Get(easeType)(from, to, t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Interpolate(Color from, Color to, float t, TimingFunctionType easeType)
        {
            return Interpolate(from, to, Interpolate(t, easeType));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Interpolate(Vector2 from, Vector2 to, float t, TimingFunctionType easeType)
        {
            return Vector2.LerpUnclamped(from, to, Interpolate(t, easeType));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Interpolate(Vector3 from, Vector3 to, float t, TimingFunctionType easeType)
        {
            return Vector3.LerpUnclamped(from, to, Interpolate(t, easeType));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Interpolate(Vector4 from, Vector4 to, float t, TimingFunctionType easeType)
        {
            return Vector4.LerpUnclamped(from, to, Interpolate(t, easeType));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Interpolate(Quaternion from, Quaternion to, float t, TimingFunctionType easeType)
        {
            return Quaternion.SlerpUnclamped(from, to, Interpolate(t, easeType));
        }
        #endregion


        #region Function interpolations
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Interpolate(float t, TimingFunction timingFunction, bool mirror = false)
        {
            if (mirror && t < 0) return timingFunction(Mathf.Abs(t));
            else return timingFunction(t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Interpolate(float from, float to, float t, TimingFunction timingFunction)
        {
            return timingFunction(from, to, t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Interpolate(Color from, Color to, float t, TimingFunction timingFunction)
        {
            return Interpolate(from, to, Interpolate(t, timingFunction));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Interpolate(Vector2 from, Vector2 to, float t, TimingFunction timingFunction)
        {
            return Vector2.LerpUnclamped(from, to, Interpolate(t, timingFunction));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Interpolate(Vector3 from, Vector3 to, float t, TimingFunction timingFunction)
        {
            return Vector3.LerpUnclamped(from, to, Interpolate(t, timingFunction));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Interpolate(Vector4 from, Vector4 to, float t, TimingFunction timingFunction)
        {
            return Vector4.LerpUnclamped(from, to, Interpolate(t, timingFunction));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Interpolate(Quaternion from, Quaternion to, float t, TimingFunction timingFunction)
        {
            return Quaternion.SlerpUnclamped(from, to, Interpolate(t, timingFunction));
        }
        #endregion


        #region Object interpolations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ForceTypedInterpolate<T>(object from, object to, float t)
        {
            var res = Interpolate(from, to, t);
            if (res is IComputedValue) return default;
            return (T) res;
        }

        public static object Interpolate(object from, object to, float t)
        {
            if (from is float f1 && to is float f2) return Interpolate(f1, f2, t);
            if (from is int i1 && to is int i2) return Interpolate(i1, i2, t);
            if (from is Color c1 && to is Color c2) return Interpolate(c1, c2, t);
            if (from is Vector2 x1 && to is Vector2 x2) return Interpolate(x1, x2, t);
            if (from is Vector3 y1 && to is Vector3 y2) return Interpolate(y1, y2, t);
            if (from is Vector4 z1 && to is Vector4 z2) return Interpolate(z1, z2, t);
            if (from is Quaternion q1 && to is Quaternion q2) return Interpolate(q1, q2, t);
            if (from is YogaValue b1 && to is YogaValue b2) return Interpolate(b1, b2, t);
            if (from is YogaValue2 g1 && to is YogaValue2 g2) return Interpolate(g1, g2, t);
            if (from is BoxShadow s1 && to is BoxShadow s2) return Interpolate(s1, s2, t);
            if (from is ICssValueList<YogaValue2> cv1 && to is ICssValueList<YogaValue2> cv2) return Interpolate(cv1, cv2, t);
            if (from is ICssValueList<BoxShadow> bv1 && to is ICssValueList<BoxShadow> bv2) return Interpolate(bv1, bv2, t);

            if (from is IComputedValue d1)
            {
                if (to is IComputedValue d2) return new ComputedInterpolation(d1, d2, t);
                else return new ComputedInterpolation(d1, new ComputedConstant(to), t);
            }
            if (to is IComputedValue dt) return new ComputedInterpolation(new ComputedConstant(from), dt, t);

            if (from == null) return to;
            if (to == null) return from;

            return t > 0.5 ? to : from;
        }

        public static object Interpolate(object from, object to, float t, Type type)
        {
            if (type == null) return Interpolate(from, to, t);
            else if (type == typeof(float)) { if (from is float f1 && to is float f2) return Interpolate(f1, f2, t); }
            else if (type == typeof(int)) { if (from is int i1 && to is int i2) return Interpolate(i1, i2, t); }
            else if (type == typeof(Color)) { if (from is Color c1 && to is Color c2) return Interpolate(c1, c2, t); }
            else if (type == typeof(Vector2)) { if (from is Vector2 x1 && to is Vector2 x2) return Interpolate(x1, x2, t); }
            else if (type == typeof(Vector3)) { if (from is Vector3 y1 && to is Vector3 y2) return Interpolate(y1, y2, t); }
            else if (type == typeof(Vector4)) { if (from is Vector4 z1 && to is Vector4 z2) return Interpolate(z1, z2, t); }
            else if (type == typeof(Quaternion)) { if (from is Quaternion q1 && to is Quaternion q2) return Interpolate(q1, q2, t); }
            else if (type == typeof(YogaValue)) { if (from is YogaValue y1 && to is YogaValue y2) return Interpolate(y1, y2, t); }
            else if (type == typeof(YogaValue2)) { if (from is YogaValue2 g1 && to is YogaValue2 g2) return Interpolate(g1, g2, t); }
            else if (type == typeof(BoxShadow)) { if (from is BoxShadow s1 && to is BoxShadow s2) return Interpolate(s1, s2, t); }

            else if (type == typeof(ICssValueList<YogaValue2>))
            {
                if (from is ICssValueList<YogaValue2> cv1 && to is ICssValueList<YogaValue2> cv2) return Interpolate(cv1, cv2, t);
            }

            else if (type == typeof(ICssValueList<BoxShadow>))
            {
                if (from is ICssValueList<BoxShadow> cv1 && to is ICssValueList<BoxShadow> cv2) return Interpolate(cv1, cv2, t);
            }

            return Interpolate(from, to, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Interpolate(object from, object to, float t, TimingFunction timingFunction)
        {
            return Interpolate(from, to, timingFunction(t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Interpolate(object from, object to, float t, TimingFunction timingFunction, Type type)
        {
            return Interpolate(from, to, timingFunction(t), type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Interpolate(object from, object to, float t, TimingFunctionType timingFunctionType)
        {
            return Interpolate(from, to, TimingFunctions.Get(timingFunctionType)(t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Interpolate(object from, object to, float t, TimingFunctionType timingFunctionType, Type type)
        {
            return Interpolate(from, to, TimingFunctions.Get(timingFunctionType)(t), type);
        }
        #endregion

    }
}