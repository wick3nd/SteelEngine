namespace SteelMotion.SteelMotion_Data.assets.shaders
{
    class CustomShaders
    {
        public const string oneColor = """
            #version 330 core
            out vec4 FragColor;
            uniform float time = 1.0f;

            void main()
            {
                float blinking = abs(sin(time));

                vec4 red = vec4(1.0f, 0.0f, 0.0f, 1.0f);
                vec4 green = vec4(0.0f, 1.0f, 0.0f, 1.0f);

                FragColor = mix(red, green, blinking);
            }
            """;
    }
}