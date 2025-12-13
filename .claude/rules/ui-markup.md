---
paths: Views/**/*
---

# C# Markup UI Guidelines

## Overview

- **No XAML**: All UI is built using C# markup via `CommunityToolkit.Maui.Markup`
- Uses fluent C# UI construction pattern

## Property vs Collection Initialization

- **プロパティ設定 → メソッドチェーン**（`.Width()`, `.Height()`, `.Text()`など）
- **コレクション初期化 → `{}`のまま**（Children, ItemsSource, RowDefinitions, ColumnDefinitionsなど）
- **1プロパティでもメソッドチェーンを使用**
- **継承したコンポーネント内では、`{}`でプロパティ初期化する**（`this`からのメソッドチェーンは使わない）

```csharp
// Good - メソッドチェーン
new ArtworkImage()
    .Width(40)
    .Height(40)

// Good - コレクション初期化
new Grid
{
    Children = { ... }
}

// Bad - プロパティに{}を使用
new ArtworkImage
{
    WidthRequest = 40,
    HeightRequest = 40
}
```

## Method Chain Formatting

- **1つのメソッドチェーン**: 1行で記述
- **2つ以上のメソッドチェーン**: 改行してインデントを入れる

```csharp
// 1つの場合 - 1行
new Label().Text("Hello")
new AlbumList().Bind(ItemsView.ItemsSourceProperty, nameof(vm.Albums))

// 2つ以上の場合 - 改行
new Label()
    .Text("Hello")
    .FontSize(16)
    .Bind(Label.TextProperty, "Name")
```

## Multi-line Method Call Formatting

- **複数行のメソッド呼び出し**: 閉じ括弧`);`は独立した行に配置

```csharp
// Good - 閉じ括弧が独立した行
BindableProperty.Create(
    nameof(Title),
    typeof(string),
    typeof(DetailHeader),
    default(string)
);

// Bad - 閉じ括弧が最後の引数と同じ行
BindableProperty.Create(
    nameof(Title),
    typeof(string),
    typeof(DetailHeader),
    default(string));
```

## Adding New Pages

1. Create Page in `Views/Page/` using C# markup pattern
2. Register in `MauiProgram.cs` as Transient
3. Add route in `AppShell.cs`

## View Organization

- `Views/Page/` - Full page components
- `Views/Row/` - List row items
- `Views/GridItem/` - Grid item components
- `Views/FirstView/` - Header/hero sections
- `Views/Header/` - Navigation headers
- `Views/List/` - Reusable list components
- `Views/Text/` - Text components
- `Views/Img/` - Image components
