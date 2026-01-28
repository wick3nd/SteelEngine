using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SteelEngine.Base
{
    internal partial class Shader
    {
       // Uniform 1 values
        public static void SetProgramInt(Shader shader, string name, int value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform1i(handle, GL.GetUniformLocation(handle, name), value);
        }
        public static void SetProgramUInt(Shader shader, string name, uint value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform1ui(handle, GL.GetUniformLocation(handle, name), value);
        }
        public static void SetProgramFloat(Shader shader, string name, float value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform1f(handle, GL.GetUniformLocation(handle, name), value);
        }
        public static void SetProgramDouble(Shader shader, string name, double value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform1d(handle, GL.GetUniformLocation(handle, name), value);
        }

       // Uniform 2 values
        public static void SetProgramVec2(Shader shader, string name, Vector2i value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform2i(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }
        public static void SetProgramVec2(Shader shader, string name, Vector2 value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform2f(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }
        public static void SetProgramVec2(Shader shader, string name, Vector2d value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform2d(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }

       // Uniform 3 values
        public static void SetProgramVec3(Shader shader, string name, Vector3i value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform3i(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }
        public static void SetProgramVec3(Shader shader, string name, Vector3 value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform3f(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }
        public static void SetProgramVec3(Shader shader, string name, Vector3d value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform3d(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }

       // Uniform 4 values
        public static void SetProgramVec4(Shader shader, string name, Vector4i value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform4i(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }
        public static void SetProgramVec4(Shader shader, string name, Vector4 value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform4f(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }
        public static void SetProgramVec4(Shader shader, string name, Vector4d value)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniform4d(handle, GL.GetUniformLocation(handle, name), 1, ref value);
        }

       // Matrix2 values
        public static void SetProgramMatrix2(Shader shader, string name, Matrix2 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix2f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix2(Shader shader, string name, Matrix2d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix2d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix2x3(Shader shader, string name, Matrix2x3 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix2x3f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix2x3(Shader shader, string name, Matrix2x3d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix2x3d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix2x4(Shader shader, string name, Matrix2x4 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix2x4f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix2x4(Shader shader, string name, Matrix2x4d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix2x4d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }

       // Matrix3 values
        public static void SetProgramMatrix3(Shader shader, string name, Matrix3 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix3f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix3(Shader shader, string name, Matrix3d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix3d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix3x2(Shader shader, string name, Matrix3x2 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix3x2f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix3x2(Shader shader, string name, Matrix3x2d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix3x2d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix3x4(Shader shader, string name, Matrix3x4 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix3x4f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix3x4(Shader shader, string name, Matrix3x4d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix3x4d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }

       // Matrix4 values
        public static void SetProgramMatrix4(Shader shader, string name, Matrix4 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix4f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix4(Shader shader, string name, Matrix4d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix4d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix4x2(Shader shader, string name, Matrix4x2 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix4x2f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix4x2(Shader shader, string name, Matrix4x2d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix4x2d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix4x3(Shader shader, string name, Matrix4x3 matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix4x3f(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }
        public static void SetProgramMatrix4x3(Shader shader, string name, Matrix4x3d matrix, bool transpose = false)
        {
            int handle = shader.GetHandle();
            GL.ProgramUniformMatrix4x3d(handle, GL.GetUniformLocation(handle, name), 1, transpose, ref matrix);
        }

       // Uniform 1 arrays
        public static unsafe void SetProgramIntArray(Shader shader, string name, int[] value)
        {
            int handle = shader.GetHandle();

            fixed (int* ptr = value)
            GL.ProgramUniform1iv(handle, GL.GetUniformLocation(handle, name), value.Length, ptr);
        }
        public static unsafe void SetProgramUIntArray(Shader shader, string name, uint[] value)
        {
            int handle = shader.GetHandle();

            fixed (uint* ptr = value)
            GL.ProgramUniform1uiv(handle, GL.GetUniformLocation(handle, name), value.Length, ptr);
        }
        public static unsafe void SetProgramFloatArray(Shader shader, string name, float[] value)
        {
            int handle = shader.GetHandle();

            fixed (float* ptr = value)
            GL.ProgramUniform1fv(handle, GL.GetUniformLocation(handle, name), value.Length, ptr);
        }
        public static unsafe void SetProgramDoubleArray(Shader shader, string name, double[] value)
        {
            int handle = shader.GetHandle();

            fixed (double* ptr = value)
            GL.ProgramUniform1dv(handle, GL.GetUniformLocation(handle, name), value.Length, ptr);
        }

       // Uniform 2 arrays
        public static unsafe void SetProgramVec2Array(Shader shader, string name, Vector2i[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector2i* ptr = value)
            GL.ProgramUniform2iv(handle, GL.GetUniformLocation(handle, name), value.Length, (int*)ptr);
        }
        public static unsafe void SetProgramVec2Array(Shader shader, string name, Vector2[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector2* ptr = value)
            GL.ProgramUniform2fv(handle, GL.GetUniformLocation(handle, name), value.Length, (float*)ptr);
        }
        public static unsafe void SetProgramVec2Array(Shader shader, string name, Vector2d[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector2d* ptr = value)
            GL.ProgramUniform2dv(handle, GL.GetUniformLocation(handle, name), value.Length, (double*)ptr);
        }

       // Uniform 3 arrays
        public static unsafe void SetProgramVec3Array(Shader shader, string name, Vector3i[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector3i* ptr = value)
            GL.ProgramUniform3iv(handle, GL.GetUniformLocation(handle, name), value.Length, (int*)ptr);
        }
        public static unsafe void SetProgramVec3Array(Shader shader, string name, Vector3[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector3* ptr = value)
            GL.ProgramUniform3fv(handle, GL.GetUniformLocation(handle, name), value.Length, (float*)ptr);
        }
        public static unsafe void SetProgramVec3Array(Shader shader, string name, Vector3d[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector3d* ptr = value)
            GL.ProgramUniform3dv(handle, GL.GetUniformLocation(handle, name), value.Length, (double*)ptr);
        }

       // Uniform 4 arrays
        public static unsafe void SetProgramVec4Array(Shader shader, string name, Vector4i[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector4i* ptr = value)
            GL.ProgramUniform4iv(handle, GL.GetUniformLocation(handle, name), value.Length, (int*)ptr);
        }
        public static unsafe void SetProgramVec4Array(Shader shader, string name, Vector4[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector4* ptr = value)
            GL.ProgramUniform4fv(handle, GL.GetUniformLocation(handle, name), value.Length, (float*)ptr);
        }
        public static unsafe void SetProgramVec4Array(Shader shader, string name, Vector4d[] value)
        {
            int handle = shader.GetHandle();

            fixed (Vector4d* ptr = value)
            GL.ProgramUniform4dv(handle, GL.GetUniformLocation(handle, name), value.Length, (double*)ptr);
        }

       // Metrix2 arrays
        public static unsafe void SetProgramMatrix2Array(Shader shader, string name, Matrix2[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix2* ptr = value)
            GL.ProgramUniformMatrix2fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix2Array(Shader shader, string name, Matrix2d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix2d* ptr = value)
            GL.ProgramUniformMatrix2dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }
        public static unsafe void SetProgramMatrix2x3Array(Shader shader, string name, Matrix2x3[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix2x3* ptr = value)
            GL.ProgramUniformMatrix2x3fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix2x3Array(Shader shader, string name, Matrix2x3d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix2x3d* ptr = value)
            GL.ProgramUniformMatrix2x3dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }
        public static unsafe void SetProgramMatrix2x4Array(Shader shader, string name, Matrix2x4[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix2x4* ptr = value)
            GL.ProgramUniformMatrix2x4fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix2x4Array(Shader shader, string name, Matrix2x4d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix2x4d* ptr = value)
            GL.ProgramUniformMatrix2x4dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }

       // Metrix3 arrays
        public static unsafe void SetProgramMatrix3Array(Shader shader, string name, Matrix3[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix3* ptr = value)
            GL.ProgramUniformMatrix3fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix3Array(Shader shader, string name, Matrix3d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix3d* ptr = value)
            GL.ProgramUniformMatrix3dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }
        public static unsafe void SetProgramMatrix3x2Array(Shader shader, string name, Matrix3x2[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix3x2* ptr = value)
            GL.ProgramUniformMatrix3x2fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix3x2Array(Shader shader, string name, Matrix3x2d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix3x2d* ptr = value)
            GL.ProgramUniformMatrix3x2dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }
        public static unsafe void SetProgramMatrix3x4Array(Shader shader, string name, Matrix3x4[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix3x4* ptr = value)
            GL.ProgramUniformMatrix3x4fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix3x4Array(Shader shader, string name, Matrix3x4d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix3x4d* ptr = value)
            GL.ProgramUniformMatrix3x4dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }

       // Metrix4 arrays
        public static unsafe void SetProgramMatrix4Array(Shader shader, string name, Matrix4[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix4* ptr = value)
            GL.ProgramUniformMatrix4fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix4Array(Shader shader, string name, Matrix4d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix4d* ptr = value)
            GL.ProgramUniformMatrix4dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }
        public static unsafe void SetProgramMatrix4x2Array(Shader shader, string name, Matrix4x2[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix4x2* ptr = value)
            GL.ProgramUniformMatrix4x2fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix4x2Array(Shader shader, string name, Matrix4x2d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix4x2d* ptr = value)
            GL.ProgramUniformMatrix4x2dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }
        public static unsafe void SetProgramMatrix4x3Array(Shader shader, string name, Matrix4x3[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix4x3* ptr = value)
            GL.ProgramUniformMatrix4x3fv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (float*)ptr);
        }
        public static unsafe void SetProgramMatrix4x3Array(Shader shader, string name, Matrix4x3d[] value, bool transpose = false)
        {
            int handle = shader.GetHandle();

            fixed (Matrix4x3d* ptr = value)
            GL.ProgramUniformMatrix4x3dv(handle, GL.GetUniformLocation(handle, name), 1, transpose, (double*)ptr);
        }
    }
}