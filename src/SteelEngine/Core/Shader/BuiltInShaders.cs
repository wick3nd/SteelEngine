namespace SteelEngine.Core
{
    public partial class Shader
    {
        public const string default_frag = """
            #version 330 core

            in vec2 TexCoord;

            uniform sampler2D texture0;
            uniform sampler2D texture1;
            uniform float time = 0.5f;

            out vec4 FragColor;

            void main() {
                vec4 image0 = texture(texture0, TexCoord);
                vec4 image1 = texture(texture1, TexCoord);

                float pulse = abs(sin(time));
                FragColor = mix(image0, image1, image1.a * pulse);
            }
            """;

        public const string depthPass_frag = """
            #version 150 core

            void main() { }
            """;

        public const string default_vert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 2) in vec2 aTexCoord;
            
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

        public const string defaultInstancing_vert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 2) in vec2 aTexCoord;
            layout (location = 9) in mat4 iModel;
            
            out vec2 TexCoord;
            
            uniform float tilingX = 1.0f;
            uniform float tilingY = 1.0f;
            uniform mat4 projection = mat4(1.0f);
            uniform mat4 view;
            
            void main()
            {
                gl_Position =  projection * view * iModel * vec4(aPosition, 1.0f);
                TexCoord = vec2(aTexCoord.x * tilingX, aTexCoord.y * tilingY);
            }
            """;

        public const string skybox_vert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 2) in vec2 aTexCoord;
            out vec2 TexCoord;
            
            layout (std140) uniform Camera
            {
                mat4 projection;
                mat4 model;
                mat4 view;
            };
            
            void main()
            {
                gl_Position =  projection * view * model * vec4(aPosition, 1.0f);
                gl_Position.z = gl_Position.w;
                TexCoord = aTexCoord;
            }
            """;
    }
}
