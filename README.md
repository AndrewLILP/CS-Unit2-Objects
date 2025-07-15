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