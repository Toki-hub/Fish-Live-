# 🐟 Magic Aquarium — AR Fishing App

An augmented reality mobile app built with **Unity 6** and **Vuforia 11.4.4** that brings fish cards to life. Scan a fish card and watch it swim in your own virtual aquarium!

---

## 📱 Features

### 🔍 AR Image Tracking
- Scan one of 3 fish cards to add that fish to your aquarium
- Powered by Vuforia image targets — works in real time via the device camera
- Supports: **Betta Splendens**, **Flying Fish**, **Japanese Snapper**

### 🐠 Living Aquarium
- Each fish card can be scanned up to **3 times** — adding up to 3 of the same fish
- All fish swim freely inside the aquarium panel using animated 2D sprites
- Fish bounce off the panel edges and tilt naturally as they swim

### 📖 Fish Info Panel
- Tap any fish to open a detailed info panel
- Shows fish name, latin name, habitat, size, conservation status, and description
- Displays a **rotating 3D model** of the fish rendered via RenderTexture
- Panel slides in/out with smooth animation

### 🌐 Bilingual Support (ID / EN)
- Toggle between **Indonesian** and **English** with a single button
- All fish info updates instantly when language is switched
- Language preference is saved between sessions

### 🗑️ Trash System
- Trash bags spawn and fall into the aquarium at random intervals
- **Tap to remove** trash and keep your aquarium clean
- Adds an interactive element and environmental awareness theme

---

## 🛠️ Tech Stack

| Component | Version |
|-----------|---------|
| Unity | 6000.4.11f1 |
| Vuforia Engine | 11.4.4 |
| Render Pipeline | URP (Universal Render Pipeline) |
| Platform | Android (ARM64, Min API 29) |
| Scripting Backend | IL2CPP |
| Orientation | Landscape Left |

---

## 📁 Project Structure

```
Assets/
├── Scenes/
│   ├── fish/
│   │   ├── Models/       # 3D fish models
│   │   ├── Prefabs/      # Fish 2D prefabs + TrashBag prefab
│   │   └── Trash/        # Trash sprite assets
│   └── scripts/
│       ├── FishTargetHandler.cs    # Vuforia tracking → spawn fish
│       ├── FishSwim2D.cs           # 2D swimming animation
│       ├── FishClickHandler.cs     # Tap fish → open info panel
│       ├── FishInfoUI.cs           # Info panel UI + slide animation
│       ├── FishModelDisplay.cs     # 3D model via RenderTexture
│       ├── FishData.cs             # ScriptableObject fish data (ID + EN)
│       ├── TrashItem.cs            # Falling trash + tap to remove
│       ├── TrashSpawner.cs         # Spawns trash at intervals
│       ├── LanguageManager.cs      # ID/EN language toggle + persistence
│       └── LanguageButtonUI.cs     # UI button for language switch
```

---

## 🚀 Getting Started

### Requirements
- Unity 6 (6000.4.11f1)
- Android Build Support module installed
- Vuforia Engine 11.4.4 (installed via `.unitypackage`)
- Android device with API 29+

### Setup
1. Clone this repository
2. Open in Unity 6
3. Import Vuforia Engine package if not auto-detected
4. Set build target to **Android** in Build Settings
5. Add your Vuforia license key in `Resources/VuforiaConfiguration`
6. Build and run on device

---

## 🎮 How to Use

1. Launch the app on your Android device
2. Point camera at a **fish card** (Betta, Flying Fish, or Snapper)
3. The fish appears and swims in the aquarium
4. Scan the same card again (up to 3×) to add more of that fish
5. **Tap a fish** to see its info and 3D model
6. Use the **EN/ID button** to switch language
7. **Tap trash bags** that appear to clean your aquarium

---
