# FlowKit
[![Version: 2.0.0](https://img.shields.io/badge/Version-2.0.0-blue.svg)](https://github.com/Ho11ow1/FlowKit)
[![License: Apache-2-0](https://img.shields.io/badge/License-Apache%202.0-green.svg)](https://opensource.org/license/apache-2-0)<br/>
**A lightweight, high-performance animation and effects toolkit for Unity TMPro.**<br/>
FlowKit provides a clean, flexible API for driving UI animations, text effects, and visual feedback — 
with support for both component-based and monolith-style workflows.

---

## Features
- **Dual Workflow Support**
  - **Component mode** — attach `FKText`, `FKVisibility`, etc. directly to your GameObjects
  - **Monolith mode** — drive animations on any `RectTransform` reference from a single controller, safe against dynamically created UI

- **Core Animation Modules**
  - **Movement**: UI positioning and directional transitions
  - **Rotation**: Rotation animations
  - **Scale**: Size and scale animations
  - **Visibility**: Fade and display state control
  - **Text**: Text-specific animations and effects

- **Text Effects**
  - Typewriter animation — configurable by characters per second or total duration
  - Color cycling with a smooth lerp between any number of colors
  - Wave and shake per-character vertex effects

- **Animation Handles**
  - Fire-and-forget or handle-based control over every animation
  - Play, stop, and chain animations via `FKHandle`

- **Event System**
  - Subscribe to start and end events per animation instance
  - Custom `FKEventData` carries target, duration, and identity per invocation

- **Custom Logging**
  - Internal `FKLogger` with contextual null and missing component warnings

- **Editor Utilities**
  - Menu item to instantly scaffold a `FlowKitController` object in your scene

---

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

---

## Usage

### Monolith Mode — via FKEngine
Drive all animations through a single `FKEngine` controller object:
```csharp
using UnityEngine;

using FlowKit;

public class Sample : MonoBehaviour
{
    [SerializeField] private RectTransform dialogueBox;
    [SerializeField] private RectTransform scoreLabel;

    [SerializeField] private FKEngine engine;

    void Start()
    {
        engine.Text.TypeWrite(dialogueBox, 2f);
        engine.Movement.Move(dialogueBox, Direction.FromLeft, 300f, 3f);
        engine.Text.Wave(scoreLabel, 0.3f, 3f, 2f);
    }
}

```

### Monolith Mode — via Component Reference
Call directly on an `FK*` component, passing any `RectTransform` as the target:
```csharp
using UnityEngine;

using FlowKit;

public class Sample : MonoBehaviour
{
    [SerializeField] private RectTransform dialogueBox;
    [SerializeField] private RectTransform scoreLabel;

    [SerializeField] private FKText fkText;

    void Start()
    {
        fkText.TypeWrite(dialogueBox, 2f);
        fkText.Wave(scoreLabel, 0.3f, 3f, 2f);
    }
}

```

### Component Mode
Attach an `FK*` component to a GameObject and animate it directly — no target reference needed:
```csharp
using UnityEngine;

using FlowKit;
using FlowKit.Events;

public class Sample : MonoBehaviour
{
    [SerializeField] private FKText fkText;
    [SerializeField] private FKVisibility fkVisibility;

    void Start()
    {
        FlowKitEvents.OnAnimationStart += OnAnimationStart;
        FlowKitEvents.OnAnimationEnd += OnAnimationEnd;

        fkText.TypeWrite(2f);
        fkText.ColorCycle(Color.red, Color.blue, 0.5f, 4f);
        fkVisibility.Fade(0f, 1f, 1f);
    }

    void OnAnimationStart(FKEventData data) => Debug.Log($"{data.Target.name} started.");
    void OnAnimationEnd(FKEventData data) => Debug.Log($"{data.Target.name} ended.");

    void OnDestroy()
    {
        FlowKitEvents.OnAnimationStart -= OnAnimationStart;
        FlowKitEvents.OnAnimationEnd -= OnAnimationEnd;
    }
}

```

### Handle-Based Control
Every animation has a `Handle` variant that lets you control playback after the fact:
```csharp
using UnityEngine;

using FlowKit;

public class Sample : MonoBehaviour
{
    [SerializeField] private FKText fkText;

    private FKHandle titleHandle;

    void Awake()
    {
        titleHandle = fkText.WaveHandle(amplitude: 0.4f, frequency: 2f);    
    }

    void Start()
    {
        titleHandle.Play();
    }

    void OnDisable()
    {
        titleHandle.Stop();
    }
}

```

---

## Requirements
- Unity 6 or higher
- TextMeshPro package

---

## License
APACHE-2.0 License - see [LICENSE](LICENSE) 

---

If you find any issues during usage, please create a github Issue [Here](https://github.com/Ho11ow1/FlowKit/issues)
