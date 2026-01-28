namespace SteelEngine.Base
{
    internal partial class Shader
    {
        public const string defaultFrag = """
            #version 330 core

            in vec2 TexCoord;

            uniform sampler2D texture0;
            uniform sampler2D texture1;
            uniform sampler2D texture2;
            uniform float time = 0.5f;

            out vec4 FragColor;

            void main() {
                vec4 image0 = texture(texture0, TexCoord);
                vec4 image1 = texture(texture1, TexCoord);

                float pulse = abs(sin(time));
                FragColor = mix(image0, image1, image1.a * pulse);
            }
            """;

        public const string depthPassFrag = """
            #version 150 core

            void main() { }
            """;

        public const string defaultVert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            
            out vec2 TexCoord;
            
            uniform float tilingX = 1.0f;
            uniform float tilingY = 1.0f;
            uniform mat4 projection = mat4(1.0f);
            uniform mat4 model = mat4(1.0f);
            uniform mat4 view;
            
            void main()
            {
                gl_Position =  projection * view * model * vec4(aPosition, 1.0f);
                TexCoord = vec2(aTexCoord.x * tilingX, aTexCoord.y * tilingY);
            }
            """;

        public const string defaultInstancingVert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            layout (location = 2) in mat4 instanceModel;
            
            out vec2 TexCoord;
            
            uniform float tilingX = 1.0f;
            uniform float tilingY = 1.0f;
            uniform mat4 projection = mat4(1.0f);
            uniform mat4 view;
            
            void main()
            {
                gl_Position =  projection * view * instanceModel * vec4(aPosition, 1.0f);
                TexCoord = vec2(aTexCoord.x * tilingX, aTexCoord.y * tilingY);
            }
            """;

        public const string skyboxVert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            
            out vec2 TexCoord;
            
            uniform mat4 projection = mat4(1.0f);
            uniform mat4 model = mat4(1.0f);
            uniform mat4 view;
            
            void main()
            {
                gl_Position =  projection * view * model * vec4(aPosition, 1.0f);
                gl_Position.z = gl_Position.w;
                TexCoord = aTexCoord;
            }
            """;
    }
}
