using OpenTK.Mathematics;

namespace SteelEngine.Core
{
    public abstract class EngineScript
    {
        protected EngineScript()
        {
            OnInit();
            BehaviourManager.Add(this);
        }

        public Vector2i windowSize { get; set; }  // Move to Window
        public Vector2i windowPosition { get; set; }

        public virtual void OnStart() { }
        public virtual void OnInit() { }
        public virtual void OnExit() { }
        public virtual void OnResize() { }
        public virtual void OnWindowPosChange() { }
        public virtual void OnFrameBufferResize() { }
        public virtual void OnFrameUpdate() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
    }
}