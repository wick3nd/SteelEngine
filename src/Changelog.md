### Additions
- Added a ability to throw exceptions directly in `SEDebug` instead of doing ones separately
- Added a `UBO` class
- Added new variables to `GLControl`:
  - `MaxUniformBufferBindings`
  - `MaxVertexUniformBlocks`
  - `MaxGeometryUniformBlocks`
  - `MaxFragmentUniformBlocks`
  - `MaxUniformBlockSize`
  - `UniformBufferOffsetAlignment`
  - `UniformOffsetAlignment`
- Added new variables to `Shader`:
  - `SetTexture2D`
  - `SetProgramTexture2D`
- Added a `IBufferObject` interface

### Changes
- Inlined a lot of methods
- Changed how shaders are created
- Renamed `SteelEngine.Base` to `SteelEngine.Core`
- Renamed `SteelEngine.Elements` to `SteelEngine.Objects`
- Moved `SteelEngine.Structs` to `SteelEngine.Elements`
- Removed the usage of `EngineScript` in core scripts

### Fixes
- Fixed `SEDebug` not being able to write to a file
- Fixed `SEDebug` crashing when writing a lot of lines to a file
- 

### Deletions
- `SEObject`
- `Material`
