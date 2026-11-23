namespace SteelEngine
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

        public const string gBuffer_frag = """
            #version 330 core

            layout (location = 0) out vec3 gPosition;
            layout (location = 1) out vec3 gNormal;
            layout (location = 2) out vec4 gAlbedoSpec;

            in vec2 TexCoords;
            in vec3 FragPos;
            in vec3 Normal;

            uniform sampler2D texture_diffuse1;
            uniform sampler2D texture_specular1;

            void main()
            {    
                gPosition = FragPos;

                gNormal = normalize(Normal);

                gAlbedoSpec.rgb = texture(texture_diffuse1, TexCoords).rgb;
                gAlbedoSpec.a = texture(texture_specular1, TexCoords).r;
            }  
            """;

        public const string default_vert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            layout (location = 9) in mat4 iModel;
            
            out vec3 FragPos;
            out vec3 Normal;
            out vec2 TexCoord;

            layout (std140) uniform Camera
            {
                mat4 projection;
                mat4 view;
            };
            
            void main()
            {
                vec4 worldPos = iModel * vec4(aPosition, 1.0f);
                FragPos = worldPos.xyz;
                Normal = mat3(transpose(inverse(iModel))) * Normal;
                gl_Position =  projection * view * worldPos;
                TexCoord = aTexCoord;
            }
            """;

        public const string skybox_vert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            layout (location = 9) in mat4 iModel;

            out vec3 FragPos;
            out vec3 Normal;
            out vec2 TexCoord;
            
            layout (std140) uniform Camera
            {
                mat4 projection;
                mat4 view;
            };
            
            void main()
            {
                vec4 worldPos = iModel * vec4(aPosition, 1.0f);
                FragPos = worldPos.xyz;
                Normal = mat3(transpose(inverse(iModel))) * Normal;
                gl_Position =  projection * view * worldPos;
                gl_Position.z = gl_Position.w;
                TexCoord = aTexCoord;
            }
            """;
    }
}