# Architecture Overview

The codebase follows a **clean architecture** with MVVM (Model-View-ViewModel) pattern and platform-specific implementations.

## Layering Structure

```
Views + ViewModels (Presentation)
        ↓
Services (Business Logic - cross-platform)
        ↓
Repositories (Data Access - platform-specific)
        ↓
Platform APIs (Android MediaStore / iOS MediaPlayer)
```

## Key Architectural Patterns

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

## Directory Structure

- **`Views/`**: UI components organized by type (Page/, Row/, GridItem/, FirstView/, etc.)
- **`ViewModels/`**: MVVM view models for each page
- **`Models/`**: Shared cross-platform data models
- **`Contracts/`**: Shared interfaces for services, models, and utilities
- **`Platforms/Android/`**: Android-specific implementations
- **`Platforms/iOS/`**: iOS-specific implementations
- **`Resources/`**: Shared app resources (fonts, images, splash, styles)

## Navigation & Routing

Navigation is configured in `AppShell.cs` with Shell-based routing:
- All pages registered via `Routing.RegisterRoute()`
- Main tab has: Home (MainPage), plus navigation to ArtistsPage, AlbumsPage, SongsPage
- Detail pages: AlbumDetailPage, ArtistDetailPage

## Data Flow Example

```
AlbumsPage → AlbumViewModel → IAlbumService → IAlbumRepository (Android/iOS) → MediaStore/MediaPlayer API
```
