using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SteelEngine.Base
{
    internal partial class Shader
    {
        private static readonly Dictionary<string, int> _attribCache = [];

       // Get the location of the shader attribute
        public int GetAttribLoc(string attribute)
        {
            if (_attribCache.TryGetValue(attribute, out int value)) return value;

            int attrib = GL.GetAttribLocation(_Handle, attribute);
            _attribCache.Add(attribute, attrib);

            return attrib;
        }

        public void BindAttribLoc(string name, uint location) => GL.BindAttribLocation(_Handle, location, name);

       // Uniform 1 values
        public void SetInt(string name, int value) => GL.Uniform1i(GL.GetUniformLocation(_Handle, name), value);
        public void SetUInt(string name, uint value) => GL.Uniform1ui(GL.GetUniformLocation(_Handle, name), value);
        public void SetFloat(string name, float value) => GL.Uniform1f(GL.GetUniformLocation(_Handle, name), value);
        public void SetDouble(string name, double value) => GL.Uniform1d(GL.GetUniformLocation(_Handle, name), value);

       // Uniform 2 values
        public void SetVec2(string name, Vector2i value) => GL.Uniform2i(GL.GetUniformLocation(_Handle, name), 1, ref value);
        public void SetVec2(string name, Vector2 value) => GL.Uniform2f(GL.GetUniformLocation(_Handle, name), 1, ref value);
        public void SetVec2(string name, Vector2d value) => GL.Uniform2d(GL.GetUniformLocation(_Handle, name), 1, ref value);

       // Uniform 3 values
        public void SetVec3(string name, Vector3i value) => GL.Uniform3i(GL.GetUniformLocation(_Handle, name), 1, ref value);
        public void SetVec3(string name, Vector3 value) => GL.Uniform3f(GL.GetUniformLocation(_Handle, name), 1, ref value);
        public void SetVec3(string name, Vector3d value) => GL.Uniform3d(GL.GetUniformLocation(_Handle, name), 1, ref value);

       // Uniform 4 values
        public void SetVec4(string name, Vector4i value) => GL.Uniform4i(GL.GetUniformLocation(_Handle, name), 1, ref value);
        public void SetVec4(string name, Vector4 value) => GL.Uniform4f(GL.GetUniformLocation(_Handle, name), 1, ref value);
        public void SetVec4(string name, Vector4d value) => GL.Uniform4d(GL.GetUniformLocation(_Handle, name), 1, ref value);

       // Matrix2 values
        public void SetMatrix2(string name, Matrix2 matrix, bool transpose = false) => GL.UniformMatrix2f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix2(string name, Matrix2d matrix, bool transpose = false) => GL.UniformMatrix2d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix2x3(string name, Matrix2x3 matrix, bool transpose = false) => GL.UniformMatrix2x3f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix2x3(string name, Matrix2x3d matrix, bool transpose = false) => GL.UniformMatrix2x3d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix2x4(string name, Matrix2x4 matrix, bool transpose = false) => GL.UniformMatrix2x4f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix2x4(string name, Matrix2x4d matrix, bool transpose = false) => GL.UniformMatrix2x4d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);

       // Matrix3 values
        public void SetMatrix3(string name, Matrix3 matrix, bool transpose = false) => GL.UniformMatrix3f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix3(string name, Matrix3d matrix, bool transpose = false) => GL.UniformMatrix3d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix3x2(string name, Matrix3x2 matrix, bool transpose = false) => GL.UniformMatrix3x2f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix3x2(string name, Matrix3x2d matrix, bool transpose = false) => GL.UniformMatrix3x2d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix3x4(string name, Matrix3x4 matrix, bool transpose = false) => GL.UniformMatrix3x4f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix3x4(string name, Matrix3x4d matrix, bool transpose = false) => GL.UniformMatrix3x4d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);

       // Matrix4 values
        public void SetMatrix4(string name, Matrix4 matrix, bool transpose = false) => GL.UniformMatrix4f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix4(string name, Matrix4d matrix, bool transpose = false) => GL.UniformMatrix4d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix4x2(string name, Matrix4x2 matrix, bool transpose = false) => GL.UniformMatrix4x2f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix4x2(string name, Matrix4x2d matrix, bool transpose = false) => GL.UniformMatrix4x2d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix4x3(string name, Matrix4x3 matrix, bool transpose = false) => GL.UniformMatrix4x3f(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);
        public void SetMatrix4x3(string name, Matrix4x3d matrix, bool transpose = false) => GL.UniformMatrix4x3d(GL.GetUniformLocation(_Handle, name), 1, transpose, ref matrix);

       // Uniform 1 arrays
        public unsafe void SetIntArray(string name, int[] value)
        {
            fixed (int* ptr = value)
            GL.Uniform1iv(GL.GetUniformLocation(_Handle, name), value.Length, ptr);
        }
        public unsafe void SetUIntArray(string name, uint[] value)
        {
            fixed (uint* ptr = value)
            GL.Uniform1uiv(GL.GetUniformLocation(_Handle, name), value.Length, ptr);
        }
        public unsafe void SetFloatArray(string name, float[] value)
        {
            fixed (float* ptr = value)
            GL.Uniform1fv(GL.GetUniformLocation(_Handle, name), value.Length, ptr);
        }
        public unsafe void SetDoubleArray(string name, double[] value)
        {
            fixed (double* ptr = value)
            GL.Uniform1dv(GL.GetUniformLocation(_Handle, name), value.Length, ptr);
        }

       // Uniform 2 arrays
        public unsafe void SetVec2Array(string name, Vector2i[] value)
        {
            fixed (Vector2i* ptr = value)
            GL.Uniform2iv(GL.GetUniformLocation(_Handle, name), value.Length, (int*)ptr);
        }
        public unsafe void SetVec2Array(string name, Vector2[] value)
        {
            fixed (Vector2* ptr = value)
            GL.Uniform2fv(GL.GetUniformLocation(_Handle, name), value.Length, (float*)ptr);
        }
        public unsafe void SetVec2Array(string name, Vector2d[] value)
        {
            fixed (Vector2d* ptr = value)
            GL.Uniform2dv(GL.GetUniformLocation(_Handle, name), value.Length, (double*)ptr);
        }

       // Uniform 3 arrays
        public unsafe void SetVec3Array(string name, Vector3i[] value)
        {
            fixed (Vector3i* ptr = value)
            GL.Uniform3iv(GL.GetUniformLocation(_Handle, name), value.Length, (int*)ptr);
        }
        public unsafe void SetVec3Array(string name, Vector3[] value)
        {
            fixed (Vector3* ptr = value)
            GL.Uniform3fv(GL.GetUniformLocation(_Handle, name), value.Length, (float*)ptr);
        }
        public unsafe void SetVec3Array(string name, Vector3d[] value)
        {
            fixed (Vector3d* ptr = value)
            GL.Uniform3dv(GL.GetUniformLocation(_Handle, name), value.Length, (double*)ptr);
        }

       // Uniform 4 arrays
        public unsafe void SetVec4Array(string name, Vector4i[] value)
        {
            fixed (Vector4i* ptr = value)
            GL.Uniform4iv(GL.GetUniformLocation(_Handle, name), value.Length, (int*)ptr);
        }
        public unsafe void SetVec4Array(string name, Vector4[] value)
        {
            fixed (Vector4* ptr = value)
            GL.Uniform4fv(GL.GetUniformLocation(_Handle, name), value.Length, (float*)ptr);
        }
        public unsafe void SetVec4Array(string name, Vector4d[] value)
        {
            fixed (Vector4d* ptr = value)
            GL.Uniform4dv(GL.GetUniformLocation(_Handle, name), value.Length, (double*)ptr);
        }

       // Metrix2 arrays
        public unsafe void SetMatrix2Array(string name, Matrix2[] value, bool transpose = false)
        {
            fixed (Matrix2* ptr = value)
            GL.UniformMatrix2fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix2Array(string name, Matrix2d[] value, bool transpose = false)
        {
            fixed (Matrix2d* ptr = value)
            GL.UniformMatrix2dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }
        public unsafe void SetMatrix2x3Array(string name, Matrix2x3[] value, bool transpose = false)
        {
            fixed (Matrix2x3* ptr = value)
            GL.UniformMatrix2x3fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix2x3Array(string name, Matrix2x3d[] value, bool transpose = false)
        {
            fixed (Matrix2x3d* ptr = value)
            GL.UniformMatrix2x3dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }
        public unsafe void SetMatrix2x4Array(string name, Matrix2x4[] value, bool transpose = false)
        {
            fixed (Matrix2x4* ptr = value)
            GL.UniformMatrix2x4fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix2x4Array(string name, Matrix2x4d[] value, bool transpose = false)
        {
            fixed (Matrix2x4d* ptr = value)
            GL.UniformMatrix2x4dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }

       // Metrix3 arrays
        public unsafe void SetMatrix3Array(string name, Matrix3[] value, bool transpose = false)
        {
            fixed (Matrix3* ptr = value)
            GL.UniformMatrix3fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix3Array(string name, Matrix3d[] value, bool transpose = false)
        {
            fixed (Matrix3d* ptr = value)
            GL.UniformMatrix3dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }
        public unsafe void SetMatrix3x2Array(string name, Matrix3x2[] value, bool transpose = false)
        {
            fixed (Matrix3x2* ptr = value)
            GL.UniformMatrix3x2fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix3x2Array(string name, Matrix3x2d[] value, bool transpose = false)
        {
            fixed (Matrix3x2d* ptr = value)
            GL.UniformMatrix3x2dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }
        public unsafe void SetMatrix3x4Array(string name, Matrix3x4[] value, bool transpose = false)
        {
            fixed (Matrix3x4* ptr = value)
            GL.UniformMatrix3x4fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix3x4Array(string name, Matrix3x4d[] value, bool transpose = false)
        {
            fixed (Matrix3x4d* ptr = value)
            GL.UniformMatrix3x4dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }

       // Metrix4 arrays
        public unsafe void SetMatrix4Array(string name, Matrix4[] value, bool transpose = false)
        {
            fixed (Matrix4* ptr = value)
            GL.UniformMatrix4fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix4Array(string name, Matrix4d[] value, bool transpose = false)
        {
            fixed (Matrix4d* ptr = value)
            GL.UniformMatrix4dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }
        public unsafe void SetMatrix4x2Array(string name, Matrix4x2[] value, bool transpose = false)
        {
            fixed (Matrix4x2* ptr = value)
            GL.UniformMatrix4x2fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix4x2Array(string name, Matrix4x2d[] value, bool transpose = false)
        {
            fixed (Matrix4x2d* ptr = value)
            GL.UniformMatrix4x2dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }
        public unsafe void SetMatrix4x3Array(string name, Matrix4x3[] value, bool transpose = false)
        {
            fixed (Matrix4x3* ptr = value)
            GL.UniformMatrix4x3fv(GL.GetUniformLocation(_Handle, name), 1, transpose, (float*)ptr);
        }
        public unsafe void SetMatrix4x3Array(string name, Matrix4x3d[] value, bool transpose = false)
        {
            fixed (Matrix4x3d* ptr = value)
            GL.UniformMatrix4x3dv(GL.GetUniformLocation(_Handle, name), 1, transpose, (double*)ptr);
        }
    }
}