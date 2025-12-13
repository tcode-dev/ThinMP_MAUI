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

## Conditional Compilation

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
