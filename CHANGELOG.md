# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic
Versioning](http://semver.org/spec/v2.0.0.html).

## [0.4.1] – 2023-03-05

Bug fix release

### Fixed
- Fix SmartBaking systems being created both on the runtime world and Latios World
- Fix Burst incompatibility on EntityCommands

## [0.4.0] – 2023-02-17

### Changed
- Upgraded DMotion to Entities 1.0
- All DMotion systems know use ISystem instead of SystemBase

### Added
- State Machine Visual Debug (click entity with StateMachineComponent in hierarchy)
- Simpler APIs for playing SingleClips and OneShot clips
- 9 new samples

### Fixed
- Normalize rotations when blending between multiple clips (optimized and non-optimized)
- Fix Safety Exceptions on ClipSamplingSystem and UpdateAnimationStatesSystem
 
## [0.3.4] – 2022-10-30

### Added
- State Machine Visual Debug (click entity with StateMachineComponent in hierarchy)
- Simpler APIs for playing SingleClips and OneShot clips
- 9 new samples

### Fixed
- Normalize rotations when blending between multiple clips (optimized and non-optimized)
- Fix Safety Exceptions on ClipSamplingSystem and UpdateAnimationStatesSystem


## [0.3.3] – 2022-10-16

### Added
- Int Transitions
- Enum Transitions

### Fixed
- Fix AnimationParameterAsset hash calculation when using strings
- Fixed bug where StateMachine would not update during AnimationState transition
- Fix bug where LinearBlend and SingleStates would reference wrong AnimationState in StateMachine (fix at UpdateStateMachineJob.CreateState)

## [0.3.2] – 2022-9-18

### Added

- Gracefully blend between multiple One Shot transitions
- Improvements to AnimationClip editor
  - Timeline
  - Camera orbit
- Unit and Performance Tests

### Changed
- Internal refactors and cleanups:
  - Handling of Animation States now separate from State Machine (see UpdateAnimationStates)
  - Clip Sampling now separate from State Machine (see ClipSamplingSystem)
  - Generalize Blending of States (see BlendAnimationStatesSystem)
  - Other bug fixes

## [0.3.1] – 2022-8-13

- Netcode sample + bug fixes

### Changed

- Netcode supported example
- Fix End time Transitions not working for clips greater than 1 sec
- Fix conceptual errors between Time and Normalized Time
- Make sure events are called when clip loops
- Fix build compilation error for Blend Parameters

## [0.3.0] – 2022-7-28

First version with a complete State Machine Visual Editor, some small BlobAsset changes

### Added

- State Machines visual editor
- Event Editor
- Clip Previews

### Changed

- StateMachineBlobAsset has more nested arrays now, making it easier to use (performance shouldn't be impacted since nested arrays are allocated inline in BlobAssets)

## [0.2.0] – 2022-7-24

This marks the first version ready for public use and with somewhat stable API.

### Added

- State Machines can now be built with Scriptable Objects
- Root Motion Modes
    - Disabled: No root motion
    - Enabled Automatic: Root motion is automatically applied to the root object
    - Enabled Manual: Root motion deltas are calculated and stored to be used by external systems for custom use cases
- Enable/Disable animation events
- True support for one shots: Any clip can be played at any time, without needing to be added to the state machine before hand. The system handles blending out of the state machine, and back in when the one shot animation finishes

### Changed

- Moved most StateMachine data to Blob Assets
- AnimationEvents implementation now use DynamicBuffers
- API cleanup pass (implementation details made internal)

## [0.1.0] – 2022-6-25

Very first version for community feedback

### Added

- Animation State Machine System and Authoring functions
- Support for optimized and non-optimized bones
- Single Clip States and 1D Blend Tree States
- Blending between states during transitions
- Boolean based transitions between states (OR/AND)
- End time transitions between states
- Root Motion (with WriteGroup support)
- Animation Events
- One Shot animations (playing a single animation that is not on the state machine)
- Object attachment
