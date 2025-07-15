# CS-Unit2-Objects
 Version 3

 # Development Log
 
 # DevLog 1
- Game Playable, 
- Build 101 done
- Video made (local file)
- Commit: https://github.com/AndrewLILP/CS-Unit2-Objects/commit/b27df909ae01762a95bc82d697b7e1eb4e79083a

 # Devlog 2
- commit: https://github.com/AndrewLILP/CS-Unit2-Objects/commit/6d44247865393972e9d4edc4cccf2c0185a9b70a
- Sprint set up for Assignment 3
- Epic 1: Core Game Enhancement (4.5h)

- Story 1.1: Difficulty Progression System (1.5h)
- Role: Game Designer + Programmer
- Tasks:
    - Design difficulty curve (spawn rate, enemy health, player challenges)
    - Implement dynamic enemy spawn rate scaling
    - Add wave-based progression system
    - Balance testing and tweaks
    - Acceptance Criteria: Game gets progressively harder over time with max spawn rate cap
- Working:
    - Level 1 (0-19 pts): Only melee enemies, slow spawn rate
    - Level 2 (20-39 pts): Melee + Exploder enemies
    - Level 3 (40-59 pts): Melee + Exploder + Shooter enemies
    - Level 4 (60-79 pts): All enemies, but Machine Gun is nerfed
    - Level 5+ (80+ pts): All enemies at full power, spawn rate capped
- commit: https://github.com/AndrewLILP/CS-Unit2-Objects/commit/f8b7f34f232f236282557a0fbde337f8ac164b43

- Story 1.2: Enhanced Enemy Behaviors (1h)
- Role: Game Designer + Programmer
- Tasks:
    - Fine-tune existing enemy types (MeleeEnemy, ShooterEnemy, etc.)
    - Add enemy spawn variety based on difficulty
    - Implement enemy group spawning patterns
    - Acceptance Criteria: Enemies provide varied, balanced challenge
- commmit: Enemy enhancements - https://github.com/AndrewLILP/CS-Unit2-Objects/commit/f503e0c70fffd62f969766963150df7d48af40ac
- Key Improvements (Gameplay testing needed):
    - Melee Enemies → Unpredictable zigzag movement + speed bursts
    - xploder Enemies → New kamikaze enemy with fuse timers and chain reactions
    - Sniper Enemies → Repositioning behavior + panic mode
    - Group Tactics → 4 coordinated spawn patterns that scale with difficulty
- Gameplay Impact:
    - Early levels: Learn individual enemy types
    - Mid levels: Simple tactical decisions (which enemy first?)
    - Late levels: Complex positioning and prioritization challenges
    
- Story 1.3: Player Feedback Systems (2h)
- Role: Programmer + VFX Artist
- Tasks:
    - Add damage feedback (screen shake, hit effects)
    - Implement bullet impact particles
    - Add enemy death effects
    - Create muzzle flash particles for weapons
    - Acceptance Criteria: Clear visual feedback for all game actions
- Key Improvements
    - Camera Shakes
    - Visual effects

- commit: Camera shake and Visual Effects - https://github.com/AndrewLILP/CS-Unit2-Objects/commit/fa4ad28f1dab5d3985ef4f2be3e943bd16aeee2b

- Epic 2: Menu & UI Systems (2h)
- Story 2.1: Main Menu (1h)
- Role: UI/UX Designer + Programmer
- Tasks:
    - Create main menu scene - yes
    - Add Start, Instructions, Quit buttons - yes
    - Implement scene transitions - NO
    - Create instructions panel/screen - yes
- Acceptance Criteria: Professional main menu with clear navigation - yes

- Story 2.2: Game Over & High Score (1h)
- Role: UI/UX Designer + Programmer
- Tasks:
    - Design game over screen
    - Display final score and high score
    - Add restart and main menu options
    - Ensure high score persistence works
- Acceptance Criteria: Game over screen shows scores and allows restart

- Epic 3: Audio & Polish (2.5h)
- Story 3.1: Sound Implementation (1.5h)
- Role: Audio Designer + Programmer
- Tasks:
    - Find/create basic sound effects (shooting, explosions, damage)
    - Add background music (simple loop)
    - Implement AudioManager system
    - Add volume controls
- Acceptance Criteria: All major actions have audio feedback

- Story 3.2: Visual Polish (1h)
- Role: VFX Artist + Programmer
- Tasks:
    - Add post-processing effects (basic color grading)
    - Implement simple particle systems
    - Add UI animations/transitions
    - Polish visual consistency
- Acceptance Criteria: Game has professional visual polish

- Epic 4: Testing & Deployment (1h)
- Story 4.1: Quality Assurance (0.5h)
- Role: QA Tester
- Tasks:
    - Gameplay testing session
    - Bug identification and priority list
    - Performance testing
    - User experience evaluation
- Acceptance Criteria: Game is stable and enjoyable

- Story 4.2: Build & Deployment (0.5h)
- Role: Producer + Programmer
- Tasks:
    - Create final build for Unity Play
    - Update GitHub repository
    - Document final features
    - Prepare submission materials
- Acceptance Criteria: Game successfully deployed to Unity Play