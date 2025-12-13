---
paths: Platforms/iOS/**/*
---

# iOS Platform Guidelines

## Directory Structure

- `Platforms/iOS/Repositories/` - Data access implementations
- `Platforms/iOS/Services/` - Platform-specific services
- `Platforms/iOS/Player/` - Audio playback (native MediaPlayer)
- `Platforms/iOS/Handlers/` - Custom control handlers
- `Platforms/iOS/Models/` - Platform-specific models
- `Platforms/iOS/ValueObjects/` - Value object types (Id, etc.)

## Key Implementation Details

### Repository Contracts

- iOS repositories use **`Id` value objects** for entity identification
- Contracts are in `Platforms/iOS/Repositories/Contracts/`
- Access data via iOS `MediaPlayer` framework

### Music Playback

- Uses native iOS **MediaPlayer** framework via `MusicPlayer.Shared` in `Platforms/iOS/Player/`
- Wrapped by `IPlayerService` interface for cross-platform access

### Custom Controls

- `BlurredImageViewHandler.cs` in `Platforms/iOS/Handlers/`
- Registered in `MauiProgram.cs` via `ConfigureMauiHandlers()`

## Minimum Requirements

- iOS 16.0
