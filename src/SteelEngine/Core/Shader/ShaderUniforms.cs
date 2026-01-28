using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    public partial class Shader
    {
       // Misc
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTexture2D(string name, Texture2D texture, TextureUnit unit)
        {
            texture.SetUnit(unit);
            texture.Enable();

            SetInt(name, (int)unit-33984);
        }

       // Uniform 1 values
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetInt(string name, int value) => GL.Uniform1i(GetUniformLoc(name), value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetUInt(string name, uint value) => GL.Uniform1ui(GetUniformLoc(name), value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetFloat(string name, float value) => GL.Uniform1f(GetUniformLoc(name), value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetDouble(string name, double value) => GL.Uniform1d(GetUniformLoc(name), value);

       // Uniform 2 values
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec2(string name, Vector2i value) => GL.Uniform2i(GetUniformLoc(name), 1, ref value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec2(string name, Vector2 value) => GL.Uniform2f(GetUniformLoc(name), 1, ref value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec2(string name, Vector2d value) => GL.Uniform2d(GetUniformLoc(name), 1, ref value);

       // Uniform 3 values
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec3(string name, Vector3i value) => GL.Uniform3i(GetUniformLoc(name), 1, ref value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec3(string name, Vector3 value) => GL.Uniform3f(GetUniformLoc(name), 1, ref value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec3(string name, Vector3d value) => GL.Uniform3d(GetUniformLoc(name), 1, ref value);

       // Uniform 4 values
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec4(string name, Vector4i value) => GL.Uniform4i(GetUniformLoc(name), 1, ref value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec4(string name, Vector4 value) => GL.Uniform4f(GetUniformLoc(name), 1, ref value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetVec4(string name, Vector4d value) => GL.Uniform4d(GetUniformLoc(name), 1, ref value);

       // Matrix2 values
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix2(string name, Matrix2 matrix, bool transpose = false) => GL.UniformMatrix2f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix2(string name, Matrix2d matrix, bool transpose = false) => GL.UniformMatrix2d(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix2x3(string name, Matrix2x3 matrix, bool transpose = false) => GL.UniformMatrix2x3f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix2x3(string name, Matrix2x3d matrix, bool transpose = false) => GL.UniformMatrix2x3d(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix2x4(string name, Matrix2x4 matrix, bool transpose = false) => GL.UniformMatrix2x4f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix2x4(string name, Matrix2x4d matrix, bool transpose = false) => GL.UniformMatrix2x4d(GetUniformLoc(name), 1, transpose, ref matrix);

       // Matrix3 values
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix3(string name, Matrix3 matrix, bool transpose = false) => GL.UniformMatrix3f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix3(string name, Matrix3d matrix, bool transpose = false) => GL.UniformMatrix3d(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix3x2(string name, Matrix3x2 matrix, bool transpose = false) => GL.UniformMatrix3x2f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix3x2(string name, Matrix3x2d matrix, bool transpose = false) => GL.UniformMatrix3x2d(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix3x4(string name, Matrix3x4 matrix, bool transpose = false) => GL.UniformMatrix3x4f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix3x4(string name, Matrix3x4d matrix, bool transpose = false) => GL.UniformMatrix3x4d(GetUniformLoc(name), 1, transpose, ref matrix);

       // Matrix4 values
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix4(string name, Matrix4 matrix, bool transpose = false) => GL.UniformMatrix4f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix4(string name, Matrix4d matrix, bool transpose = false) => GL.UniformMatrix4d(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix4x2(string name, Matrix4x2 matrix, bool transpose = false) => GL.UniformMatrix4x2f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix4x2(string name, Matrix4x2d matrix, bool transpose = false) => GL.UniformMatrix4x2d(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix4x3(string name, Matrix4x3 matrix, bool transpose = false) => GL.UniformMatrix4x3f(GetUniformLoc(name), 1, transpose, ref matrix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetMatrix4x3(string name, Matrix4x3d matrix, bool transpose = false) => GL.UniformMatrix4x3d(GetUniformLoc(name), 1, transpose, ref matrix);

       // Uniform 1 arrays
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetIntArray(string name, int[] value)
        {
            fixed (int* ptr = value)
            GL.Uniform1iv(GetUniformLoc(name), value.Length, ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetUIntArray(string name, uint[] value)
        {
            fixed (uint* ptr = value)
            GL.Uniform1uiv(GetUniformLoc(name), value.Length, ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetFloatArray(string name, float[] value)
        {
            fixed (float* ptr = value)
            GL.Uniform1fv(GetUniformLoc(name), value.Length, ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetDoubleArray(string name, double[] value)
        {
            fixed (double* ptr = value)
            GL.Uniform1dv(GetUniformLoc(name), value.Length, ptr);
        }

       // Uniform 2 arrays
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec2Array(string name, Vector2i[] value)
        {
            fixed (Vector2i* ptr = value)
            GL.Uniform2iv(GetUniformLoc(name), value.Length, (int*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec2Array(string name, Vector2[] value)
        {
            fixed (Vector2* ptr = value)
            GL.Uniform2fv(GetUniformLoc(name), value.Length, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec2Array(string name, Vector2d[] value)
        {
            fixed (Vector2d* ptr = value)
            GL.Uniform2dv(GetUniformLoc(name), value.Length, (double*)ptr);
        }

       // Uniform 3 arrays
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec3Array(string name, Vector3i[] value)
        {
            fixed (Vector3i* ptr = value)
            GL.Uniform3iv(GetUniformLoc(name), value.Length, (int*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec3Array(string name, Vector3[] value)
        {
            fixed (Vector3* ptr = value)
            GL.Uniform3fv(GetUniformLoc(name), value.Length, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec3Array(string name, Vector3d[] value)
        {
            fixed (Vector3d* ptr = value)
            GL.Uniform3dv(GetUniformLoc(name), value.Length, (double*)ptr);
        }

       // Uniform 4 arrays
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec4Array(string name, Vector4i[] value)
        {
            fixed (Vector4i* ptr = value)
            GL.Uniform4iv(GetUniformLoc(name), value.Length, (int*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec4Array(string name, Vector4[] value)
        {
            fixed (Vector4* ptr = value)
            GL.Uniform4fv(GetUniformLoc(name), value.Length, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetVec4Array(string name, Vector4d[] value)
        {
            fixed (Vector4d* ptr = value)
            GL.Uniform4dv(GetUniformLoc(name), value.Length, (double*)ptr);
        }

       // Metrix2 arrays
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix2Array(string name, Matrix2[] value, bool transpose = false)
        {
            fixed (Matrix2* ptr = value)
            GL.UniformMatrix2fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix2Array(string name, Matrix2d[] value, bool transpose = false)
        {
            fixed (Matrix2d* ptr = value)
            GL.UniformMatrix2dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix2x3Array(string name, Matrix2x3[] value, bool transpose = false)
        {
            fixed (Matrix2x3* ptr = value)
            GL.UniformMatrix2x3fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix2x3Array(string name, Matrix2x3d[] value, bool transpose = false)
        {
            fixed (Matrix2x3d* ptr = value)
            GL.UniformMatrix2x3dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix2x4Array(string name, Matrix2x4[] value, bool transpose = false)
        {
            fixed (Matrix2x4* ptr = value)
            GL.UniformMatrix2x4fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix2x4Array(string name, Matrix2x4d[] value, bool transpose = false)
        {
            fixed (Matrix2x4d* ptr = value)
            GL.UniformMatrix2x4dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }

       // Metrix3 arrays
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix3Array(string name, Matrix3[] value, bool transpose = false)
        {
            fixed (Matrix3* ptr = value)
            GL.UniformMatrix3fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix3Array(string name, Matrix3d[] value, bool transpose = false)
        {
            fixed (Matrix3d* ptr = value)
            GL.UniformMatrix3dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix3x2Array(string name, Matrix3x2[] value, bool transpose = false)
        {
            fixed (Matrix3x2* ptr = value)
            GL.UniformMatrix3x2fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix3x2Array(string name, Matrix3x2d[] value, bool transpose = false)
        {
            fixed (Matrix3x2d* ptr = value)
            GL.UniformMatrix3x2dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix3x4Array(string name, Matrix3x4[] value, bool transpose = false)
        {
            fixed (Matrix3x4* ptr = value)
            GL.UniformMatrix3x4fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix3x4Array(string name, Matrix3x4d[] value, bool transpose = false)
        {
            fixed (Matrix3x4d* ptr = value)
            GL.UniformMatrix3x4dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }

       // Metrix4 arrays
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix4Array(string name, Matrix4[] value, bool transpose = false)
        {
            fixed (Matrix4* ptr = value)
            GL.UniformMatrix4fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix4Array(string name, Matrix4d[] value, bool transpose = false)
        {
            fixed (Matrix4d* ptr = value)
            GL.UniformMatrix4dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix4x2Array(string name, Matrix4x2[] value, bool transpose = false)
        {
            fixed (Matrix4x2* ptr = value)
            GL.UniformMatrix4x2fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix4x2Array(string name, Matrix4x2d[] value, bool transpose = false)
        {
            fixed (Matrix4x2d* ptr = value)
            GL.UniformMatrix4x2dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix4x3Array(string name, Matrix4x3[] value, bool transpose = false)
        {
            fixed (Matrix4x3* ptr = value)
            GL.UniformMatrix4x3fv(GetUniformLoc(name), 1, transpose, (float*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetMatrix4x3Array(string name, Matrix4x3d[] value, bool transpose = false)
        {
            fixed (Matrix4x3d* ptr = value)
            GL.UniformMatrix4x3dv(GetUniformLoc(name), 1, transpose, (double*)ptr);
        }
    }
}