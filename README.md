# ⚔️ SAMT ALL-STARS

> A 2D pixel-art platform fighter inspired by Brawlhalla and Super Smash Bros, built in Unity.

---

## 👥 Team

| Name | Role |
|------|------|
| **Yasser** | Developer / Character |
| **Ivan** | Developer / Character |
| **Quan** | Developer / Character |
| **Sidon** | Developer / Character |

---

## 🎮 Gameplay

SAMT ALL-STARS is a local 2-player fighting game where each player selects a character and a map before battling. Each character has unique stats and a special ability with its own cooldown.

### Controls

| Action | Keyboard |
|--------|----------|
| Move | `←` `→` |
| Jump | `Space` |
| Dash | `Left Shift` |
| Kick | `K` |

Both **keyboard** and **gamepad** are supported and configurable in the Settings screen.

---

## 🧑‍🤝‍🧑 Characters

| Character | HP | Speed | Special Ability | Cooldown |
|-----------|-----|-------|-----------------|----------|
| **Yasser** | 300 | 1.0 | LilBoom | 11s |
| **Ivan** | 250 | 1.0 | Pierce Thought | 11s |
| **Quan** | 200 | 1.1 | Ghosting | 7s |
| **Sidon** | 250 | 1.25 | Roberto Carlos | 13s |

---

## 🗺️ Maps

Each map was AI-generated and themed around a character:

- **Pincode** *(Yasser)*
- **Bab Al-Hara** *(Sidon)*
- **Xiaohongshu** *(Quan)*
- **Arebaetaotisetik** *(Ivan)*

---

## 🖥️ Scenes

The game is composed of the following Unity scenes:

```
MainMenu           → Title screen with Play / Settings / Wiki / Exit
CharacterSelection → P1 and P2 each pick a character
MapSelection       → Players vote on the map to fight on
Game               → Main fight scene
Settings           → Audio and control configuration
Wiki               → Character stats and map info
```

---

## 🏗️ Project Structure

```
SAMT-ALL-STARS/
├── 1_QdC/                          # Quadro di competenze
├── 2_Abstract/                     # Project abstract
├── 3_Documentazione/               # Full project documentation
├── 4_Diari/                        # Work diaries
├── 5_Applicativo/
│   └── SAMT ALL-STARS/             # Unity project root
│       ├── Assets/
│       │   ├── Animation/          # Animation controllers (per character)
│       │   │   └── Yasser/
│       │   ├── Resources/
│       │   │   ├── Animations/     # Scene animation assets
│       │   │   ├── Materials/      # Physics materials
│       │   │   ├── Scripts/        # All C# game logic
│       │   │   │   ├── Ability.cs
│       │   │   │   ├── Calci.cs
│       │   │   │   ├── Character.cs
│       │   │   │   ├── CharacterData.cs
│       │   │   │   ├── CharacterManager.cs
│       │   │   │   ├── InputType.cs
│       │   │   │   ├── LogManager.cs
│       │   │   │   ├── Map.cs
│       │   │   │   ├── MapManager.cs
│       │   │   │   ├── Player.cs
│       │   │   │   ├── PlayerController.cs
│       │   │   │   └── UIManager.cs
│       │   │   └── Sprites/
│       │   │       ├── Game/       # Map backgrounds & tiles
│       │   │       ├── Main Menu/  # Menu buttons & background
│       │   │       ├── Player/     # Character sprite sheets
│       │   │       │   ├── Ivan/
│       │   │       │   ├── Quan/
│       │   │       │   ├── Sidon/
│       │   │       │   └── Yasser/
│       │   │       ├── Select/     # Character & map selection UI
│       │   │       ├── Settings/   # Keyboard & gamepad UI assets
│       │   │       ├── Wiki/       # Wiki page assets
│       │   │       └── WinPage/    # Victory screen assets
│       │   ├── Scenes/
│       │   │   ├── MainMenu.unity
│       │   │   ├── CharacterSelection.unity
│       │   │   ├── MapSelection.unity
│       │   │   ├── Game.unity
│       │   │   ├── Settings.unity
│       │   │   └── Wiki.unity
│       │   └── Settings/           # URP render pipeline config
│       ├── Packages/
│       └── ProjectSettings/
└── 7_Allegati/                     # Diagrams & project planning
    ├── Diagramma delle classi.png
    ├── Swimlane.png
    └── Use Case.png
```

---

## 🧱 Architecture

The codebase follows an object-oriented design:

- **`Player`** — holds a player's ID, color, input type, and assigned character
- **`Character`** — wraps `CharacterData` with runtime state (current HP, ability cooldown)
- **`CharacterData`** — static stats: name, max HP, speed, description, win/game history
- **`Ability`** — name, damage, duration, cooldown
- **`PlayerController`** — MonoBehaviour handling movement, jumping, dashing, and kick animation
- **`CharacterManager`** / **`MapManager`** — manage collections of characters and maps
- **`UIManager`** — static navigation helpers for scene transitions
- **`LogManager`** — writes timestamped logs to `%AppData%/SamtAllStars/Logs/`
- **`InputType`** — enum: `Keyboard` | `Controller`

See [`7_Allegati/Diagramma delle classi.png`](7_Allegati/Diagramma%20delle%20classi.png) for the full class diagram.

---

## 🛠️ Tech Stack

| Tool | Version / Detail |
|------|-----------------|
| **Unity** | 6000.1.4f1 |
| **Render Pipeline** | Universal Render Pipeline (URP) 2D |
| **Language** | C# |
| **Sprite Generation** | AI-generated via ChatGPT / DALL·E |
| **Physics** | Unity Rigidbody2D with custom friction material |
| **Input** | Unity Legacy Input + New Input System |

---

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/IvanOstroumov/SAMT-ALL-STARS.git
   ```

2. Open Unity Hub and add the project located at:
   ```
   5_Applicativo/SAMT ALL-STARS/
   ```

3. Make sure Unity version **6000.1.4f1** (or compatible) is installed with the **2D URP** module.

4. Open the `MainMenu` scene and press **Play**.

---

## 📄 Documentation

| Document | Location |
|----------|----------|
| Project documentation | `3_Documentazione/SamtAllStars.docx` |
| Competency framework | `1_QdC/QdC_SAMT ALL-STARS.docx` |
| Work diaries | `4_Diari/Diario di Lavoro.docx` |
| Class diagram | `7_Allegati/Diagramma delle classi.png` |
| Use case diagram | `7_Allegati/Use Case.png` |
| Swimlane diagram | `7_Allegati/Swimlane.png` |
| Sprite generation prompt | `7_Allegati/Sprite_Generation_Prompt.docx` |
