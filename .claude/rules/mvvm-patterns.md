---
paths: ViewModels/**/*
---

# MVVM Pattern Guidelines

## Overview

Uses **MVVM with CommunityToolkit.Mvvm** for reactive bindings.

## ViewModel Implementation

- Use `[ObservableProperty]` attributes for reactive properties
- Place ViewModels in `ViewModels/` directory

## Example

```csharp
public partial class AlbumViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<IAlbumModel> albums = new();

    public void Load()
    {
        // Load data from service
    }
}
```

## Adding New ViewModels

1. Create ViewModel in `ViewModels/`
2. Use `[ObservableProperty]` from CommunityToolkit.Mvvm
3. Register in `MauiProgram.cs` as Transient

## Dependency Injection

- ViewModels: **Transient** (new instance per navigation)
- Services: **Singleton** (shared state)
