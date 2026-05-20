namespace SteelEngine.Core
{
    public partial class Shader
    {
        public const string default_frag = """
            #version 330 core

            in vec2 TexCoord;

            uniform sampler2D texture0;

            out vec4 FragColor;

            void main() {
                vec4 image0 = texture(texture0, TexCoord);

                FragColor = image0;
            }
            """;

        public const string depthPass_frag = """
            #version 150 core

            void main() { }
            """;

        public const string default_vert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            layout (location = 9) in mat4 iModel;
            
            out vec2 TexCoord;

            layout (std140) uniform Camera
            {
                mat4 projection;
                mat4 view;
            };
            
            void main()
            {
                gl_Position =  projection * view * iModel * vec4(aPosition, 1.0f);
                TexCoord = aTexCoord;
            }
            """;

        public const string skybox_vert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            layout (location = 9) in mat4 iModel;
            out vec2 TexCoord;
            
            layout (std140) uniform Camera
            {
                mat4 projection;
                mat4 view;
            };
            
            void main()
            {
                gl_Position =  projection * view * iModel * vec4(aPosition, 1.0f);
                gl_Position.z = gl_Position.w;
                TexCoord = aTexCoord;
            }
            """;
    }
}