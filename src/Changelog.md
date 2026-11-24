### Additions
- Added a ability to throw exceptions directly in `SEDebug` instead of doing ones separately
- Added a `UniformBuffer` class
  - `Resize`
  - `BufferBase`
  - `BufferRange`
  - `Data`
  - `Init`
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
- Added a IBufferObject interface
- Added temporary buffer classes

### Changes
- Inlined a lot of methods
- Changed how shaders are created
- Renamed `SteelEngine.Base` to `SteelEngine.Core`
- Renamed `SteelEngine.Elements` to `SteelEngine.Objects`
- Moved `SteelEngine.Structs` to `SteelEngine.Elements`
- Removed the usage of `EngineScript` in core scripts
- Reduced needed ammount of calls to update `EngineScript` parameters and functions
- Changed the default `aTexCoord` layout location to `2`
- Changed the default `iModel` layout location to `9`
- Added extra bind checks to all core objects in order to reduce driver calls
- Renamed Buffer classes to their _fuller_ names

### Fixes
- Fixed `SEDebug` not being able to write to a file
- Fixed `SEDebug` crashing when writing a lot of lines to a file

### Deletions
- `SEObject`
- `Material`

other small possibly undocumented changes