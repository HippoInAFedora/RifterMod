# ChangeLog

ver 0.3.0
 - Description changes
 - Specials reworked! Recursion is now a ranged tool, and Chained Worlds acts as god intended (or so I hope).
 - Enemy healthbar now shows the amount of health shattered.
 - May or may not have fixed shatter being applied by non-rifter characters. This needs to be playtested.

ver 0.2.3
 - forgot dll whoops

ver 0.2.2
 - description fixes and changes. (Special thanks to TRYAGAIN211 for the constant help with this!)
 - many a nerf, to compensate for shatter's strengths.
 - secondaries are all at 300%. Changes to percentage will be based on which feels the weakest from here on out.
 - specials were lowered to 280% base damage.
 - this should be the last update for a hot minute. Will be working on VFX, getting a model, etc. Have fun, and let me know how the damage %s feel!


ver 0.2.1
 - Networked!
 - Some slight fixes, Rapidfire is reverted to base stocks of 3, not 4 (it was a mistake)
 - Cooldown increases were made, this was from the last update, forgot to mention
 - Chained Worlds logic is pretty screwy at times. I will work on this, but in the meantime, either avoid it or let me know what situations make it worse.

ver 0.2.0
 - Bug Fix: Rift Rider teleports enemies again.
 - Some Networking has been done. Yay! Not fully networked.
 > Major Changes
 - Base damage has been increased from 12 to 14. This means ALL ability percentages have been tweaked.
 - Rift radius has been reduced for primary and secondary.
 - Added a new experimental passive: Shatter. Currently a hidden debuff (will not count toawrds Deathmark rn).
 - Shatter reduces armor and total health of enemies. Stacks up to 20 times, for a max of 60 armor and 50% health reduction.
 - Armor reduction for shatter is NOT linear; this is used to assist with bosses and spongy enemies.
 - health reduction IS linear. Not that you needed to know either of these.
 - Shatter is not networked currently




ver 0.1.0
 - Bug Fix: Added R2API_DamageType as a dependency. LMK if this isn't working correctly still
 - Bug Fix: Rifts no longer permanently Friendly Fire
 - Bug Fix: All Refraction Rifts now teleport
 - primary rift size slightly increased. Distance slightly decreased.
 - overcharge now stacks (I actually tested it and it works now)
 - Chained Worlds might be buggy, but I think I reverted it
 - issues: Teleporting enemies does not activate if you are not the host player. Working on it.
 - future changes: vfx overhaul, model, anims, you know, the pretty stuff.
 - I only changed it to 0.1.0 cuz I felt like it.

ver 0.0.6
 - slight change. Maybe it worked, maybe it didn't.



ver 0.0.5
 - Yippee! Blacklisting works (lunar is not blacklisted as of currently - for research purposes)
 - Slipstream stacks overcharges (I was lazy, I did not double check, let me know if I'm lying)
 >buffs/nerfs
 - Refraction cooldown changed to 3s.
 - Refraction damage reduced from 375% to 325%. It seems fairly balanced now.
 - Refraction side rifts might not teleport enemies? This is a bug if so. Will change. Let me know.


ver 0.0.4
Eventual Changes:
- blacklisting of lunar enemies, void barnacles, and minor constructs from being teleported
- possible slipstream changes
 >Changes:
- Added another special: Chained Worlds
- Added basic vfx. Ideas or help welcome. I don't like it.
 >buffs/nerfs
- primary buffed from 550% to 600%
- refraction now always teleports
- recursion buffed to 350% base damage



ver 0.0.3
- Got rid of my lazy implementation of passives that aren't showing up anyways (they were breaking things, apparently (hopefully))


ver 0.0.2
- I intended to add basic vfx for visual clarity. I am leaving that for later (if you want to do this, let me know!)
- Made it so other skills cannot be used during Recursion.
- Fixed Rift Rider getting cancelled by everything.
  >buffs/nerfs
- primary buffed from 450% to 550%
- Refraction buffed from 375% to 475% (I'm too lazy to change the logic right now, so I'm testing this first)
- Recursion charge-up time has been increased from .5s to .8s.


ver 0.0.1
- Initial build