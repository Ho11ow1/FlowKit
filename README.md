# FlowKit
[![Status: Work In Progress](https://img.shields.io/badge/Status-Work%20In%20Progress-yellow.svg)](https://github.com/Ho11ow1/FlowKit)
[![Version: 1.2.0](https://img.shields.io/badge/Version-1.2.0-blue.svg)](https://github.com/Ho11ow1/FlowKit/releases)
[![License: Apache-2-0](https://img.shields.io/badge/License-Apache%202.0-green.svg)](https://opensource.org/license/apache-2-0)<br/>
[![Unity](https://img.shields.io/badge/Unity-2022.3.10f1%2B-black.svg?logo=unity&logoColor=white)](#)<br/>
**A lightweight and flexible animation toolkit for Unity UI and visual effects.**<br/>
Supports smooth fade, transition, scale, rotate, and typewriter animations, along with custom shaders and editor tools.

## Features
- **Unified FlowKit API**
  - Animate UI with a single entry point (`FlowKitEngine.cs`)
  - Modular backend, internal-only animation components
- **Core Animation Module split**
  - **Visibility**: Handles opacity and display state via
  - **Transition**: Controls UI positioning and movement
  - **Text**: Manages text specific animations and effects
  - **Scale**: Handles size animations
  - **Rotate**: Controls rotation animations
- **Text Effects**
  - Typewriter animation with configurable speed and character delay
  - Easy to use colorCycling effect
- **Editor Utilities**
  - Visual animation preview window
- **Shader Support (W.I.P)**
  - Expandable 2D/3D shader folders included

## Installation

### Option 1: Unity Package Manager (via Git)
1. Open **Unity** and go to **Window > Package Manager**
2. Click the **+** button and choose **"Add package from Git URL..."**
3. Paste in:
```text
https://github.com/Ho11ow1/FlowKit.git
```

### Option 2: Manual Installation
1. Download or clone this repository
2. Drag the `FlowKit/` folder into your `Assets/` directory

## Usage

```csharp
using FlowKit.Core;
using FlowKit.Common;

public class PopupController : MonoBehaviour
{
    [SerializeField] private FlowKitEngine popupFK;

    void Start()
    {
        // Hide panel on load
        popupFK.Visibility.SetPanelVisibility(false);

        // Subscribe to animation events
        FlowKitEvents.FadeStart += () => Debug.Log("Fade started.");
        FlowKitEvents.FadeEnd += () => Debug.Log("Fade ended.");
    }

    public void ShowPopup()
    {
        popupFK.Visibility.FadeIn(AnimationTarget.Panel, 1, 0.5f);
        popupFK.Transition.FromLeft(AnimationTarget.Image, 1, 100f, EasingType.EaseInOut, 0.75f);
    }

    public void HidePopup()
    {
        popupFK.Visibility.FadeOut(AnimationTarget.Panel, 1, 0.5f, 0.25f);
    }

    void OnDestroy()
    {
        // Clean up
        FlowKitEvents.FadeStart -= () => Debug.Log("Fade started.");
        FlowKitEvents.FadeEnd -= () => Debug.Log("Fade ended.");
    }
}

```

## Requirements

- Unity 2022.3.10f1 or higher
- TextMeshPro package

## Roadmap
- Take a look at planned & up coming features [Here](ROADMAP.md)

## License

APACHE-2.0 License - see [LICENSE](LICENSE) 
