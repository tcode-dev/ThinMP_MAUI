---
paths: Platforms/Android/**/*
---

# Android Platform Guidelines

## Directory Structure

- `Platforms/Android/Repositories/` - Data access implementations
- `Platforms/Android/Services/` - Platform-specific services
- `Platforms/Android/Audio/` - Audio playback (ExoPlayer)
- `Platforms/Android/Handlers/` - Custom control handlers
- `Platforms/Android/Models/` - Platform-specific models
- `Platforms/Android/Notifications/` - Notification handling

## Key Implementation Details

### Repository Contracts

- Android repositories use **string IDs** for entity identification
- Contracts are in `Platforms/Android/Repositories/Contracts/`
- Access data via Android `MediaStore` API

### Music Playback

- Uses **ExoPlayer** via `MusicPlayer` class in `Platforms/Android/Audio/`
- Wrapped by `IPlayerService` interface for cross-platform access

### Custom Controls

- `BlurredImageViewHandler.cs` in `Platforms/Android/Handlers/`
- Registered in `MauiProgram.cs` via `ConfigureMauiHandlers()`

## Minimum Requirements

- Android 13 (API 33)
