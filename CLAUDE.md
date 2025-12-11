# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ThinMP_MAUI is a cross-platform music player application built with .NET MAUI (Multi-platform App UI) for Android 13+ and iOS 16+. The app reads from the device's native music library and provides browsing by artists, albums, and songs with full playback controls.

## Build Commands

```bash
# Build for Android
dotnet build -f net10.0-android

# Build for iOS
dotnet build -f net10.0-ios

# Clean build artifacts
dotnet clean

# Restore dependencies
dotnet restore
```

**Note:** This project currently has no test suite.

## Architecture Overview

The codebase follows a **clean architecture** with MVVM (Model-View-ViewModel) pattern and platform-specific implementations:

### Layering Structure

```
Views + ViewModels (Presentation)
        ↓
Services (Business Logic - cross-platform)
        ↓
Repositories (Data Access - platform-specific)
        ↓
Platform APIs (Android MediaStore / iOS MediaPlayer)
```

### Key Architectural Patterns

1. **Platform-Specific Repositories**: Each platform implements its own version of `ISongRepository`, `IAlbumRepository`, and `IArtistRepository` with different contracts
   - Android: Uses string IDs, accesses Android `MediaStore` API
   - iOS: Uses `Id` value objects, accesses iOS `MediaPlayer` framework
   - **Important**: Android and iOS repository contracts are NOT compatible - they live in `Platforms/{Android|iOS}/Repositories/Contracts/`

2. **Shared Services**: Business logic services are cross-platform and registered in `MauiProgram.cs`
   - Services use conditional compilation (`#if ANDROID` / `#elif IOS`) to call the correct platform repository
   - Service interfaces are in `Contracts/Services/`

3. **Dependency Injection** (`MauiProgram.cs`):
   - Repositories/Services: **Singleton** (shared state)
   - ViewModels/Pages: **Transient** (new instance per navigation)

4. **MVVM with CommunityToolkit**: ViewModels use `[ObservableProperty]` attributes for reactive bindings

5. **C# Markup UI**: Uses `CommunityToolkit.Maui.Markup` for fluent C# UI construction (no XAML)
   - Example: `new Label().Text("Hello").FontSize(16).Bind(Label.TextProperty, "Property")`

### Directory Structure

- **`Views/`**: UI components organized by type (Page/, Row/, GridItem/, FirstView/, etc.)
- **`ViewModels/`**: MVVM view models for each page
- **`Models/`**: Shared cross-platform data models
- **`Contracts/`**: Shared interfaces for services, models, and utilities
- **`Platforms/Android/`**: Android-specific implementations
  - Repositories/, Services/, Audio/, Handlers/, Models/, Notifications/, etc.
  - Uses ExoPlayer (Xamarin.AndroidX.Media3) for audio playback
- **`Platforms/iOS/`**: iOS-specific implementations
  - Repositories/, Services/, Player/, Handlers/, Models/, ValueObjects/, etc.
  - Uses native iOS MediaPlayer framework
- **`Resources/`**: Shared app resources (fonts, images, splash, styles)

### Navigation & Routing

Navigation is configured in `AppShell.cs` with Shell-based routing:
- All pages registered via `Routing.RegisterRoute()`
- Main tab has: Home (MainPage), plus navigation to ArtistsPage, AlbumsPage, SongsPage
- Detail pages: AlbumDetailPage, ArtistDetailPage

## Development Guidelines

### Adding New Features

1. **New Page/Screen**:
   - Create ViewModel in `ViewModels/` (use `[ObservableProperty]` from CommunityToolkit.Mvvm)
   - Create Page in `Views/Page/` using C# markup pattern
   - Register both in `MauiProgram.cs` (ViewModel as Transient, Page as Transient)
   - Add route in `AppShell.cs`

2. **Platform-Specific Code**:
   - **If adding data access**: Implement repository in both `Platforms/Android/Repositories/` and `Platforms/iOS/Repositories/`
   - **If adding business logic**: Create service in `Platforms/Android/Services/` and `Platforms/iOS/Services/`
   - Create interface in `Contracts/Services/` for services that should be cross-platform
   - Register in `MauiProgram.cs` with conditional compilation blocks

3. **Shared vs Platform Code**:
   - Keep business logic in Services when possible (use `#if` directives to call platform-specific repositories)
   - Platform models in `Platforms/{Android|iOS}/Models/` implement different contracts
   - Shared models in `Models/` are for cross-platform data transfer

### Key Implementation Details

**Music Playback**:
- Android: Uses ExoPlayer via `MusicPlayer` class in `Platforms/Android/Audio/`
- iOS: Uses `MusicPlayer.Shared` in `Platforms/iOS/Player/`
- Both wrapped by `IPlayerService` interface for cross-platform access

**Custom Controls**:
- `BlurredImageView`: Custom MAUI control with platform-specific handlers
- Handlers in `Platforms/{Android|iOS}/Handlers/BlurredImageViewHandler.cs`
- Registered in `MauiProgram.cs` via `ConfigureMauiHandlers()`

**Data Flow Example**:
```
AlbumsPage → AlbumViewModel → IAlbumService → IAlbumRepository (Android/iOS) → MediaStore/MediaPlayer API
```

### Conditional Compilation

Use platform directives for platform-specific code:
```csharp
#if ANDROID
    // Android-specific code
#elif IOS
    // iOS-specific code
#endif
```

This is heavily used in `MauiProgram.cs` and service implementations.

## Important Notes

- **No XAML**: All UI is built using C# markup via `CommunityToolkit.Maui.Markup`
- **Platform Contracts Differ**: Android and iOS repositories have different method signatures (string vs Id types)
- **.NET 10.0**: Project uses the latest .NET runtime
- **Minimum OS Versions**: Android 13 (API 33), iOS 16.0
- **No Tests**: There is currently no test project in this repository
