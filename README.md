# **[Super Brick Breaker]**

## 소개
> 본 프로젝트는 1개월 (25.10.01 ~ 25.11.01)간 1인 개발로 완성한 2D 벽돌깨기 기반 기술 데모(Vertical Slice)입니다.
> 
> 프로젝트의 목적은 단순히 플레이 가능한 게임을 만드는 것을 넘어, 유지보수 및 확장이 용이한 코드와 시스템을 구현 및 검증하는데 있습니다.
>
> 자세한 설계 의도와 문제 해결 과정은 아래 본문을 참조 바랍니다.

<br>



## 기술 스택 (Tech Stack)

| 구분 | 내용 |
| :--- | :--- |
| **Engine** | ![Unity](https://img.shields.io/badge/Unity-6.0LTS-black?style=for-the-badge&logo=unity) |
| **Language** | ![C#](https://img.shields.io/badge/C%23-8.0-blue?style=for-the-badge&logo=csharp) |
| **Tools** | `GitHub Desktop`, `VisualStudio 2022`, `Aseprite`|

<br>

## 시각 자료 (Visuals)
(시각 자료 준비중)
<br>

 [ [🎮 itch.io에서 플레이](https://medorpond.itch.io/super-brick-breaker)]

<br>



## 1. 핵심 설계 (Core Architecture & Design)

본 프로젝트는 **'책임 영역 분리(SRP)'**, **'유지보수'** 및 **'확장성'** 을 고려한 시스템을 설계하였습니다.

### 1-1. 데이터 분리 (ScriptableObject, SO)
**밸런싱**과 **콘텐츠 관리**를 위해 프리팹과 데이터를 분리했습니다.
* **`LevelDatabase`:** 스테이지별로 별도의 Scene(Level)을 만들지 않고, **`Level Data`** 구조체로 각 스테이지의 정보를 저장한 뒤 이를 SO 에셋에서 리스트로 관리하여, 각 스테이지의 수정 및 추가가 용이하도록 설계했습니다.
* **`ItemData`:** 아이템의 아이콘 및 효과를 SO로 데이터화 한 뒤 `ItemShell` 프리팹에 주입하는 방식을 사용해, 아이템의 밸런싱 및 유사한 효과의 아이템 추가를 별도의 코드 수정 없이 가능하도록 했습니다.

### 1-2. 모듈형 디자인 (Modular Design)
**`Brick`** 프리팹의 확장성을 극대화하기 위해 **기능**을 모듈로 분리했습니다.
* `Brick.cs`는 `OnHit`, `OnBreak`와 같은 Brick에서 발생하는 상호작용에 대한 '이벤트'만 발행하는 뼈대로써 작용합니다.
* `HPModule`, `ItemModule` 등 **기능**을 담당하는 각 모듈은 Brick이 발행하는 이벤트 중 원하는 이벤트를 구독하여 작동합니다.
* 위 모듈 시스템을 통해 다양한 기믹을 가지는 Brick의 종류를 유연하게 확장할 수 있었습니다.

### 1-3. 풀링 (Pooling)
**`메모리 풀`** 패턴을 통해 자주 생성 및 파괴되는 Object에 대한 메모리 효율을 최적화했습니다.
* **`Pool`**: `Stack`자료구조를 통해 Item을 요청받은 위치에 발행 및 회수합니다. 생성 시 초기 개수만큼 Item을 미리 인스턴스화하며, 최대 개수를 초과한 아이템은 Stack에 회수하지 않고 파괴합니다.
* **`PoolItem`**: 메모리 풀로 관리되는 객체로, 초기화를 위한 `SetPool` 메서드와 회수를 위한 `Retrieve`메서드를 제공합니다.

### 1-4. 중앙 관리자(Managers)
**SRP**를 고려해 책임 영역을 분할하고, 코드 응집도를 높였습니다.
* **`GameManager`:** **Scene**전환 로직, `LevelDB`원본 등을 보유하며 전체 게임 흐름 및 공용 데이터를 관리합니다.
* **`SoundManager`:** **`Master`**, **`BGM`**, **`SFX`** 세 가지 사운드에 대한 음량 및 AudioSource를 관리하며, 음량 설정, AudioClip 설정, Audio Muffle등 파생되는 메서드를 외부에 제공합니다.
* **`StageManager`:** **`LevelDB`**에 설정된 스테이지 데이터를 **Scene**에 로드하고, **Combo System**에 따라 콤보 점수를 연산하며, **Player**의 Life 변화 또는 **Brick**의 개수 변화에 따른 **GameOver** 분기를 판단합니다.
* **`BallManager`:** 메모리 풀을 보유하며 스테이지에 존재하는 모든 **`Ball`**에 대한 발행, 관리, 회수를 담당합니다. **`Ball`**과 관련된 아이템을 획득하면 모든 Ball에게 이를 전파하며, 모든 공을 잃은 경우 **`OnAllBallLost`** 이벤트를 발행합니다.
* **`ItemManager`:** **`ItemData`**에 대한 목록을 보유하여 요청에 따라 무작위 데이터가 적용된 **`ItemShell`:**을 발급하며, **`Player`**가 아이템을 획득하면 아이템의 적용 Scope에 따라 올바른 이벤트를 발행합니다.
* **`VFXManager`:** **`VFX`** 모델을 메모리 풀로 보유하며, 요청에 따라 올바른 위치에 VFX를 발급해줍니다. VFX 타입을 **`Enum`**으로 관리하여, 요청 효과를 쉽게 바꿀 수 있도록 설계했습니다.

### 1-5. Generic 타입 스크립팅
Generic(`<T>`) 타입을 사용한 범용 코드 구현으로 코드 응집도 및 재사용성을 높였습니다.
* **`BaseManager<T>`**에 기반해 **Singleton**패턴을 구현하고, DontDestroyOnLoad를 직렬화를 통해 인스펙터에 노출시켜 다양한 **Manager**클래스를 편리하게 구현 및 수정했습니다.
* **`Pool<T>`** 및 **`PoolItem<T>`**를 통해 범용**메모리 풀 패턴**을 구현하여, 풀링이 필요한 리소스에게 손쉽게 확장하여 적용하였습니다.

<br>


## 2. 주요 문제 해결 (Key Challenges & Solutions)

본 프로젝트에서는 SRP에 따라 책임 영역을 분리하는 과정에서 다양한 **참조** 및 **데이터 관리** 문제가 발생했습니다. </br> 
아래 항목에서는 발생한 다양한 오류 중 치명적인 오류 및 이슈와, 기타 게임 플레이 경험을 저해하는 이슈를 나누어 그 원인과 해결 방법을 제시합니다.

### [버그 1] 씬 로드시 발생하는 치명적 널(Null) 오류

* **[문제]** 씬을 로드하는 과정에서 핵심 이벤트 구독을 위해 Singleton Manager에 접근할 시, 널 참조(Null Reference) 오류가 발생했습니다.
* **[원인]** Unity 엔진에서 각 객체의 초기화 순서가 보장되지 않아 레이스 컨디션(Race Condition)에 의해 발생한 널 오류였습니다.
* **[해결]** `BaseManager<T>`의 Instance에 Getter를 설정하여, Manager가 초기화되기 전에 참조가 요청될 경우 Scene의 계층 트리를 탐색하여 Manager의 참조를 반환하도록 보장하였습니다.

<br>

### [버그 2] 씬 리로드(Retry) 시 발생하는 치명적 널(Null) 오류

* **[문제]** 씬을 리로드하는 과정에서 널 참조(Null Reference) 오류가 발생했으며, 단순 Null Guard의 도입으로는 문제가 해결되지 않았습니다.
* **[원인]** Singleton으로 구현된 Manager의 Instance는 Manager가 파괴될 경우 참조 자체가 불가하여, Null 비교연산 자체에서 Null 오류를 발생시켰습니다.
* **[해결]** 객체 초기화 시점 (`Awake` 생명주기)에 Manager Instance를 캐싱하여, 캐시된 필드에 Null Guard를 적용하여 널 참조 오류를 해결할 수 있었습니다.

<br>

### [버그 3] 콜라이더 터널링 이슈

* **[문제]** `Ball`이 Wall 또는 Deadzone의 콜라이더에서 충돌 또는 감지되지 않고 뚫고나가는 문제가 발생했습니다.
* **[원인]** `Ball`의 속도가 지나치게 빨라, 물리 연산 주기 내에 충돌 영역을 지나쳐 발생하는 **터널링** 이슈였습니다.
* **[해결]** 물리 연산 주기를 더 짧게 설정하고, `Ball`의 충돌 연산 방식을 `Continuous`로 바꿔, 보다 정확한 충돌 연산을 보장하므로써 문제를 해결할 수 있었습니다.

<br>

### [게임플레이 1] 축 정지(Axis Stall) 이슈

* **[문제]** `Ball`이 특정 궤적을 무한히 왕복하며, 게임 플레이가 불가능해지는 문제가 발생했습니다.
* **[원인]** `Ball`의 특정 축 속력이 `0`이 되어 발생하는 문제였습니다.
* **[해결]** `Ball`이 매 충돌 시 각 축의 속력을 검사하여 Epsilon 속력 이하인 경우, 지정된 최소 속력을 해당 축에 보장하는 방식으로 이를 해결할 수 있었습니다.

</br>

## 3. 회고 및 향후 개선점 (Retrospective)

* **[기술 부채 1] UI SRP 위반:** </br>
**(문제점)**: 마감 기한을 준수하기 위해 UI 스크립트의 책임 범위를 명확하게 나누지 못하여, `TogglePanel`등의 메서드가 여러 스크립트에 분산되는 등의 기술 부채가 발생했습니다.</br>
**(개선안)**: 차후 프로젝트에서는 각 UI의 책임 영역을 명확하게 나누고, 베이스 코드 사용 또는 모듈화를 통해 SRP를 준수하고 기술 부채를 최소화하겠습니다.
* **[향후 계획] 콘텐츠 확장:** </br>
`Scriptable Object`에 기반한 레벨 데이터 및 **모듈 방식**으로 구현된 Brick 시스템을 바탕으로, 추후 다양한 기믹과 아이템, 스테이지를 확장해 나가겠습니다.

---
<br>

| **ENGLISH TRANSLATION** |
|:---:|

<br>

> *The English translation was provided by Gemini 2.5 pro. If there are any errors or potential misunderstandings, the original Korean text is the authoritative version.*

<br>

# **[Super Brick Breaker]**

## Introduction
> This project is a 2D brick-breaker-based technical demo (Vertical Slice) completed as a solo development over 1 month (25.10.01 ~ 25.11.01).
> 
> The purpose of the project goes beyond simply making a playable game, and lies in implementing and verifying code and systems that are easy to maintain and expand.
>
> For detailed design intentions and problem-solving processes, please refer to the main text below.

<br>



## Tech Stack (Tech Stack)

| Category | Content |
| :--- | :--- |
| **Engine** | ![Unity](https://img.shields.io/badge/Unity-6.0LTS-black?style=for-the-badge&logo=unity) |
| **Language** | ![C#](https://img.shields.io/badge/C%23-8.0-blue?style=for-the-badge&logo=csharp) |
| **Tools** | `GitHub Desktop`, `VisualStudio 2022`, `Aseprite`|

<br>

## Visuals (Visuals)
(No Visuals Not Yet Set)
<br>

 [ [🎮 Play on itch.io](https://medorpond.itch.io/super-brick-breaker)]

<br>



## 1. Core Design (Core Architecture & Design)

This project designed a system that considers **'Separation of Responsibility (SRP)'**, **'Maintainability'**, and **'Expandability'**.

### 1-1. Data Separation (ScriptableObject, SO)
For **Balancing** and **Content Management**, prefabs and data were separated.
* **`LevelDatabase`:** Instead of creating a separate Scene (Level) for each stage, each stage's information is saved as a **`Level Data`** struct and managed as a list in an SO asset, designed to make modification and addition of each stage easy.
* **`ItemData`:** By using a method where the item's icon and effects are turned into data via SO and then injected into the `ItemShell` prefab, it was made possible to balance items and add items with similar effects without separate code modifications.

### 1-2. Modular Design (Modular Design)
To maximize the expandability of the **`Brick`** prefab, **functionality** was separated into modules.
* `Brick.cs` acts as a skeleton that only publishes 'events' for interactions occurring in the Brick, such as `OnHit` and `OnBreak`.
* Each module responsible for a **functionality**, such as `HPModule` and `ItemModule`, operates by subscribing to the desired event among those published by the Brick.
* Through this module system, the types of Bricks with various gimmicks could be flexibly expanded.

### 1-3. Pooling (Pooling)
Through the **`Memory Pool`** pattern, memory efficiency for Objects that are frequently created and destroyed was optimized.
* **`Pool`**: Issues and retrieves Items to the requested location via a `Stack` data structure. Upon creation, Items are pre-instantiated up to the initial count, and items exceeding the maximum count are destroyed instead of being retrieved to the Stack.
* **`PoolItem`**: As an object managed by the memory pool, it provides a `SetPool` method for initialization and a `Retrieve` method for retrieval.

### 1-4. Central Managers (Managers)
Considering **SRP**, responsibility areas were divided, and code cohesion was increased.
* **`GameManager`:** Holds **Scene** transition logic, the original `LevelDB`, etc., and manages the overall game flow and common data.
* **`SoundManager`:** Manages volume and AudioSource for the three sound types: **`Master`**, **`BGM`**, and **`SFX`**, and provides derived methods externally, such as volume setting, AudioClip setting, and Audio Muffle.
* **`StageManager`:** Loads the stage data set in **`LevelDB`** into the **Scene**, calculates combo scores according to the **Combo System**, and determines the **GameOver** branch based on changes in the **Player**'s Life or the number of **Brick**s.
* **`BallManager`:** Holds a memory pool and is responsible for the issuance, management, and retrieval of all **`Ball`**s existing in the stage. When an item related to the **`Ball`** is acquired, it propagates this to all Balls, and publishes the **`OnAllBallLost`** event if all balls are lost.
* **`ItemManager`:** Holds a list of **`ItemData`** and issues an **`ItemShell`:** with random data applied upon request, and when the **`Player`** acquires an item, it publishes the correct event according to the item's application Scope.
* **`VFXManager`:** Holds **`VFX`** models as a memory pool and issues VFX at the correct location upon request. It was designed so that VFX types are managed as an **`Enum`**, allowing the requested effect to be easily changed.

### 1-5. Generic Type Scripting
Through general-purpose code implementation using Generic (`<T>`) types, code cohesion and reusability were increased.
* Based on **`BaseManager<T>`**, the **Singleton** pattern was implemented, and DontDestroyOnLoad was exposed to the inspector via serialization, allowing various **Manager** classes to be conveniently implemented and modified.
* Through **`Pool<T>`** and **`PoolItem<T>`**, a general-purpose **Memory Pool Pattern** was implemented, and easily extended and applied to resources that need pooling.

<br>


## 2. Key Problem Solving (Key Challenges & Solutions)

In this project, various **reference** and **data management** problems occurred in the process of separating responsibility areas according to SRP. </br> 
The items below divide the various errors that occurred into critical errors and issues, and other issues that hinder the gameplay experience, presenting their causes and solutions.

### [Bug 1] Critical Null Error Occurring During Scene Load

* **[Problem]** In the process of loading a scene, a Null Reference error occurred when accessing a Singleton Manager to subscribe to core events.
* **[Cause]** It was a null error that occurred due to a Race Condition because the initialization order of each object is not guaranteed in the Unity engine.
* **[Solution]** A Getter was set on the Instance of `BaseManager<T>` to ensure that if a reference is requested before the Manager is initialized, it searches the Scene's hierarchy tree and returns the Manager's reference.

<br>

### [Bug 2] Critical Null Error Occurring During Scene Reload (Retry)

* **[Problem]** In the process of reloading a scene, a Null Reference error occurred, and the problem was not solved by the introduction of a simple Null Guard.
* **[Cause]** The Instance of a Manager implemented as a Singleton becomes unreferenceable itself when the Manager is destroyed, causing a Null error in the Null comparison operation itself.
* **[Solution]** By caching the Manager Instance at the time of object initialization (`Awake` lifecycle), the null reference error could be resolved by applying a Null Guard to the cached field.

<br>

### [Bug 3] Collider Tunneling Issue

* **[Problem]** A problem occurred where the `Ball` would pass through the colliders of the Wall or Deadzone without colliding or being detected.
* **[Cause]** The `Ball`'s speed was excessively fast, so it was a **Tunneling** issue that occurred by passing through the collision area within a physics calculation cycle.
* **[Solution]** The problem could be solved by setting the physics calculation cycle to be shorter and changing the `Ball`'s collision calculation method to `Continuous`, thereby ensuring more accurate collision calculations.

<br>

### [Gameplay 1] Axis Stall Issue

* **[Problem]** A problem occurred where the `Ball` infinitely travels back and forth on a specific trajectory, making gameplay impossible.
* **[Cause]** It was a problem caused by the `Ball`'s velocity on a specific axis becoming `0`.
* **[Solution]** This could be resolved by having the `Ball` check the velocity of each axis at every collision, and if it is below Epsilon velocity, ensuring a designated minimum velocity on that axis.

</br>

## 3. Retrospective and Future Improvements (Retrospective)

* **[Technical Debt 1] UI SRP Violation:** </br>
**(Problem):** To meet the deadline, the responsibility scope of UI scripts could not be clearly divided, leading to technical debt such as methods like `TogglePanel` being distributed across multiple scripts.</br>
**(Improvement Plan):** In future projects, each UI's area of responsibility will be clearly divided, and SRP will be adhered to and technical debt minimized through the use of base code or modularization.</br>
* **[Future Plans] Content Expansion:** </br>
Based on the level data based on `Scriptable Object` and the Brick system implemented in a **modular fashion**, various gimmicks, items, and stages will be expanded in the future.