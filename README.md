# AdoBuddy

> A simplified, cross-platform alternative to the Azure DevOps web interface - available on Windows and mobile.

---

## Overview

**AdoBuddy** is a .NET MAUI application designed to offer a cleaner and more accessible experience for Azure DevOps users.  
Instead of navigating the full Azure DevOps website, AdoBuddy brings the essential features directly into a streamlined native app, available on **Windows**, **Android**, and **iOS**.

The goal is to reduce friction for developers and teams who interact daily with Azure DevOps - boards, pipelines, work items, and more - without the overhead of a full browser-based interface.

---

## Features (planned)

- View and manage Work Items (Boards, Backlogs)
- Monitor CI/CD Pipelines
- Browse Pull Requests and Repositories
- Receive notifications on pipeline runs and PR updates
- Native Windows app experience
- Mobile-first UI for Android and iOS

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | [.NET MAUI](https://learn.microsoft.com/dotnet/maui/) |
| Language | C# / XAML |
| Runtime | .NET 10 |
| UI Engine | MAUI Controls (XAML Source Generation) |
| Dependency Injection | Microsoft.Extensions.DependencyInjection (built-in MAUI) |
| Logging | Microsoft.Extensions.Logging.Debug |
| Fonts | OpenSans (Regular & SemiBold) |

---

## Target Platforms

| Platform | Minimum Version |
|---|---|
| Windows | Windows 10 (10.0.17763.0) |
| Android | API 21 (Android 5.0) |
| iOS | 15.0 |
| macOS (Catalyst) | 15.0 |

---

## Getting Started

### Prerequisites

- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with the **.NET MAUI** workload installed
- .NET 10 SDK
- An Azure DevOps account (Personal Access Token required)

### Run the project

Clone the repository and open `AdoBuddy.slnx` in Visual Studio, then select your target platform (Windows, Android or iOS).

---

## Project Structure

```
AdoBuddy/
├── Platforms/          # Platform-specific entry points (Windows, Android, iOS, macOS)
├── Resources/
│   ├── AppIcon/        # App icons
│   ├── Fonts/          # Custom fonts (OpenSans)
│   ├── Images/         # Shared images
│   ├── Splash/         # Splash screen
│   └── Styles/         # Global XAML styles and colors
├── App.xaml            # Application root
├── AppShell.xaml       # Shell navigation
├── MainPage.xaml       # Main entry page
└── MauiProgram.cs      # App builder and DI configuration
```

---

## License

This project is licensed under the MIT License.
