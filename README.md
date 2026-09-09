# Unity Game Development Template (C#)

A batteries-included Unity project template for building state-driven mobile games. It wires together dependency injection, a typed event bus, an async state machine, Addressables-driven UI, and a versioned save system so a new game can start from a working vertical slice instead of an empty scene.

## Architecture: Model – View – State

```
┌────────────────────────────────────────────────────────────┐
│ GameLifetimeScope (VContainer root)  ── DontDestroyOnLoad  │
│   ├─ GameEntry                    (di bootstrap)           │
│   ├─ SoundService / LoadingCurtain (serialized instances)  │
│   ├─ all IGlobalService services   (reflection-registered) │
│   └─ all IState states             (reflection-registered) │
└────────────────────────────────────────────────────────────┘
        │  StateFactory.CreateAllStates() → StateMachine
        ▼
   StateMachine (async, UniTask)  ◄── enter/exit between states
        │
        └── UIState<TModel, TView>  ── creates View via UIFactory
              │                        (Addressables, keyed by type name)
              ├── TView  : BaseView<TModel>   (presentation)
              └── TModel : BaseModel          (state data, publishes events)
```

**Flow:** `GameEntry` → `LoadDataState` (load save, construct sound, load *Menu* scene) →
`MainState` → `GameState` ⇄ `MainState`. Boot order and lifecycle are described in
`Assets/Game/Scripts/Structure/`.

### Core systems

| System | Where | Purpose |
|---|---|---|
| **VContainer** | `Assets/Plugins/VContainer` | DI container. Global services and all states auto-registered via reflection (`TypeExtensions`), no manual registration per class. |
| **State machine** | `Assets/Game/Scripts/Structure/StateMachine` | Async (`UniTask`) states with cancellation tokens, payloaded transitions (up to 3 payloads), guard against re-entering the active state. |
| **Event bus** | `Assets/Game/Scripts/Core/EventBus` | Typed publish/subscribe (`Event<T>` / `Event` classes implementing `IEvent`). Thread-safe; error-isolated subscribers. |
| **UI factory** | `Assets/Game/Scripts/Services/Factories` | Creates views from Addressables keyed by `typeof(View).Name`, injects dependencies, recursively initializes child views. |
| **Assets** | `Assets/Game/Scripts/Services/Assets` | Addressables wrapper: cached async loads + instantiates that auto-release when the GameObject is destroyed (no leaks). |
| **SaveLoad** | `Assets/Game/Scripts/Services/SaveLoad` | Versioned, fault-tolerant JSON (Newtonsoft) persistence in `PlayerPrefs`; auto-saves on any change. |
| **StaticData** | `Assets/Game/Scripts/Services/StaticData` | `Resources.Load` configs (sounds, game settings). |
| **Sound** | `Assets/Game/Scripts/Services/Sound` | Music + one-shot effects keyed by `SoundId` enum, persisted mute state. |

## Adding a new screen (State)

1. **View** — `Assets/Game/Scripts/Game/UI/<Name>` : `BaseView<TModel>` (implements `IViewInitializer`).
   Add the prefab to an Addressables group; keyed by class name.
2. **Model** — `Assets/Game/Scripts/Game/Logic/<Name>` : `BaseModel` (gets `IEventBus` injected).
3. **State** — `Assets/Game/Scripts/Structure/StateMachine/States/<Name>` : `UIState<TModel, TView>`
   (screen) or `SimpleState`/`IState` (logic-only). Auto-registered and instantiated by `StateFactory`.

Register new global services by implementing the `IGlobalService` marker; add sounds by extending
the `SoundId` enum. See the full checklist in `docs/` / the Obsidian project notes.

## Plugins

**Custom:** `MultiGraphicButton` (multi-graphic animated buttons), `EventBus`, `TweenEasePreviewWindow`,
`PreviewSprite` inspector drawer, `ForceSingleSpriteImporter`.
**Community:** DOTween, NativeGallery, UniTask, VContainer, Addressables, Newtonsoft JSON.

---

## Known limitations & design decisions

Deliberate trade-offs in this template — reviewed so a reader knows they are intentional, not bugs.

- **Save storage is plaintext `PlayerPrefs`.** Simple and zero-dependency. For a shipped mobile title with real economy, replace with an encrypted/cloud save (or `Application.persistentDataPath` + a signing/encryption layer). The `ISaveLoad` abstraction isolates that swap to one class.
- **Save schema is forward-tolerant, not backward-migrating.** A version bump (`SaveLoad.SaveVersion`) resets the save rather than migrating old data. Add migration logic when a live install base exists.
- **Views are created and destroyed on every state entry/exit.** Trade-off favoring memory predictability over reuse (each screen is rebuilt fresh). Addressables instances self-release on destroy, so this is memory-safe.
- **Addressables assets are keyed by C# type name** (e.g. `typeof(MainView).Name`). Fast for a solo/AR team, but brittle if you rename a class — keep prefab addresses in sync. Consider centralized address constants for larger teams.
- **`CleanUp()` releases all cached loads at a scene boundary.** Correct for scene switching, but flushes the load cache on every `SceneManager.LoadSceneAsync`. If a game reuses many assets across scenes, add ref-counting.
- **`StateFactory` keys states by concrete type via reflection.** Adding a state costs nothing; the trade-off is a scan on registration. `TypeExtensions` caches the reflection result; call `ClearCache()` if you add code in play mode.
- **UI adaptation targets iPhone/iPad** (`Scripts/Game/DeviceAdaptation`). Add Android-safe-area or tablet variants for other targets.
- **No addressables per-preference asset packing / build optimization** is preconfigured. Set group layouts per platform before release.

## Requirements

- Unity 6 (6000.5; developed against `com.unity.addressables 2.9.1`, UGUI 2.5.0)
- A `StaticData/GameSettings` ScriptableObject at `Assets/Game/Resources/StaticData/` (optional — falls back to default framerate/V-sync)
