using OpenTK.Mathematics;
using SteelEngine.Elements.Structs;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    internal class Frustum : EngineScript
    {
        internal Matrix4 camProj = Matrix4.Identity;
        internal Matrix4 camView = Matrix4.Identity;

        private Matrix4 _prevProj;
        private Matrix4 _prevView;

        public Plane[] planes = new Plane[6];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update()
        {
            if (camProj == _prevProj && camView == _prevView) 
            {
                _prevProj = camProj;
                _prevView = camView;

                return;
            }
            if (camProj == _prevProj && camView == _prevView) 
            {
                _prevProj = camProj;
                _prevView = camView;

                return;
            }

            Matrix4 _viewProjection = camView * camProj;

            // left plane
            planes[0] = new Plane(new Vector4(
                _viewProjection.M14 + _viewProjection.M11,
                _viewProjection.M24 + _viewProjection.M21,
                _viewProjection.M34 + _viewProjection.M31,
                _viewProjection.M44 + _viewProjection.M41
            )).Normalize();

            // right plane
            planes[1] = new Plane(new Vector4(
                _viewProjection.M14 - _viewProjection.M11,
                _viewProjection.M24 - _viewProjection.M21,
                _viewProjection.M34 - _viewProjection.M31,
                _viewProjection.M44 - _viewProjection.M41
            )).Normalize();

            // bottom plane
            planes[2] = new Plane(new Vector4(
                _viewProjection.M14 + _viewProjection.M12,
                _viewProjection.M24 + _viewProjection.M22,
                _viewProjection.M34 + _viewProjection.M32,
                _viewProjection.M44 + _viewProjection.M42
            )).Normalize();

            // upper plane
            planes[3] = new Plane(new Vector4(
                _viewProjection.M14 - _viewProjection.M12,
                _viewProjection.M24 - _viewProjection.M22,
                _viewProjection.M34 - _viewProjection.M32,
                _viewProjection.M44 - _viewProjection.M42
            )).Normalize();

            // near plane
            planes[4] = new Plane(new Vector4(
                _viewProjection.M14 + _viewProjection.M13,
                _viewProjection.M24 + _viewProjection.M23,
                _viewProjection.M34 + _viewProjection.M33,
                _viewProjection.M44 + _viewProjection.M43
            )).Normalize();

            // far plane
            planes[5] = new Plane(new Vector4(
                _viewProjection.M14 - _viewProjection.M13,
                _viewProjection.M24 - _viewProjection.M23,
                _viewProjection.M34 - _viewProjection.M33,
                _viewProjection.M44 - _viewProjection.M43
            )).Normalize();
        }
    }
}