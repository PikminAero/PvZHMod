﻿using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using Microsoft.SqlServer.Server;
using NexPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace PvZHMod
{
    public class PvZHModCards : WildfrostMod
    {
        public PvZHModCards(string modDirectory) : base(modDirectory)
        {
            Instance = this;
        }

        public static PvZHModCards Instance;

        public override string GUID => "somethinggaming.wildfrost.grassandghouls";
        public override string[] Depends => new string[] { };
        public override string Title => "Grass and Ghouls";
        public override string Description => "Plants vs. Zombies Heroes in Wildfrost!";


        public static List<object> assets = new List<object>();    //The list of builders that will build your CardData/StatusEffectData


        private bool preLoaded = false;                            //Used to prevent redundantly reconstructing our data. Not truly necessary.


        private void CreateModAssets()
        {
            //Code for status effects

            //Code for cards

            preLoaded = true;
        }

        public T TryGet<T>(string name) where T : DataFile
        {
            T data;
            if (typeof(StatusEffectData).IsAssignableFrom(typeof(T)))
                data = base.Get<StatusEffectData>(name) as T;
            else
                data = base.Get<T>(name);

            if (data == null)
                throw new Exception($"TryGet Error: Could not find a [{typeof(T).Name}] with the name [{name}] or [{Extensions.PrefixGUID(name, this)}]");

            return data;
        }

        public CardData.StatusEffectStacks SStack(string name, int amount) => new CardData.StatusEffectStacks(Get<StatusEffectData>(name), amount);

        public StatusEffectDataBuilder StatusCopy(string oldName, string newName)
        {
            StatusEffectData data = TryGet<StatusEffectData>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            StatusEffectDataBuilder builder = data.Edit<StatusEffectData, StatusEffectDataBuilder>();
            builder.Mod = this;
            return builder;
        }

        public CardDataBuilder CardCopy(string oldName, string newName)
        {
            CardData data = TryGet<CardData>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            CardDataBuilder builder = data.Edit<CardData, CardDataBuilder>();
            builder.Mod = this;
            return builder;
        }

        public ClassDataBuilder TribeCopy(string oldName, string newName)
        {
            ClassData data = TryGet<ClassData>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            ClassDataBuilder builder = data.Edit<ClassData, ClassDataBuilder>();
            builder.Mod = this;
            return builder;
        }
        internal T[] RemoveNulls<T>(T[] data) where T : DataFile
        {
            List<T> list = data.ToList();
            list.RemoveAll(x => x == null || x.ModAdded == this);
            return list.ToArray();
        }

        private T[] DataList<T>(params string[] names) where T : DataFile => names.Select((s) => TryGet<T>(s)).ToArray();

        public override void Load()
        {
            if (!preLoaded) { CreateModAssets(); } //preLoaded makes sure that the builders are not made again on the 2nd load.
            base.Load();                          //Actual loading
        }
       

        public void UnloadFromClasses()
        {
            List<ClassData> tribes = AddressableLoader.GetGroup<ClassData>("ClassData");
            foreach (ClassData tribe in tribes)
            {
                if (tribe == null || tribe.rewardPools == null) { continue; } //This isn't even a tribe; skip it.

                foreach (RewardPool pool in tribe.rewardPools)
                {
                    if (pool == null) { continue; }; //This isn't even a reward pool; skip it.

                    pool.list.RemoveAllWhere((item) => item == null || item.ModAdded == this); //Find and remove everything that needs to be removed.
                }
            }
        }

        public CardData.TraitStacks TStack(string name, int amount) => new CardData.TraitStacks(TryGet<TraitData>(name), amount);

        public override void Unload()
        {
            UnloadFromClasses();
            base.Unload();
        }

        //Credits to Hopeful for this AddAssets code.
        public override List<T> AddAssets<T, Y>()
        {
            if (assets.OfType<T>().Any())
                Debug.LogWarning($"[{Title}] adding {typeof(Y).Name}s: {assets.OfType<T>().Select(a => a._data.name).Join()}");

            ///////////////////////////////////////////////////////////////////////////////
            /// CUSTOM CARDS
            ///////////////////////////////////////////////////////////////////////////////


            // Disco Zombie
            assets.Add(
                new CardDataBuilder(this).CreateUnit("DiscoZombie", "Disco Zombie")
                .SetSprites("disco_zombie.png", "crazy_zombie_bg.png")
                .SetStats(3, 1, 2)
                .WithCardType("Friendly")
                .WithFlavour("The Star of the show!")
                .AddPool("BasicUnitPool")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1]
                    {
                        SStack("When Deployed Summon Backup Dancer",1)
                    };
                })
                );
            

            // Backup Dancer
            assets.Add(
                new CardDataBuilder(this).CreateUnit("BackupDancer", "Backup Dancer")
                .SetSprites("backup_dancer.png", "crazy_zombie_bg.png")
                .SetStats(1, 1, 2)
                .WithCardType("Friendly")
                .WithFlavour("What a great mustachio'd dancer! His moves are nothing without Disco Zombie's though...")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1]
                    {
                        SStack("Trigger When Disco Zombie In Row Attacks",1)
                    };
                })
                );


            // Discotron 3000
            assets.Add(
                new CardDataBuilder(this).CreateUnit("Discobot", "Disco-Tron 3000")
                .SetSprites("Discobot.png", "crazy_zombie_bg.png")
                .SetStats(4, 4, 6)
                .WithCardType("Friendly")
                .WithFlavour("Get ready to boogie down!")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1]
                    {
                        SStack("When Deployed Summon Disco Zombie",1)
                    };
                }));

            // Cuckoo Zombie
            assets.Add(
                new CardDataBuilder(this).CreateUnit("CuckooZombie", "Cuckoo Zombie")
                .SetSprites("CuckooZombie.png", "crazy_zombie_bg.png")
                .SetStats(1, 4, 2)
                .WithCardType("Friendly")
                .WithFlavour("He always knows what time it is. CRAZY TIME!"));

            assets.Add(
                new CardDataBuilder(this).CreateUnit("UnlifeParty", "Unlife of the Party")
                .SetSprites("UnlifeParty.png", "crazy_zombie_bg.png")
                .SetStats(1, 1, 3)
                .WithCardType("Friendly")
                .WithFlavour("His archenemy is the Wall-Flower.")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1]
                    {
                        SStack("When Deployed Gain Damage And Health Per Ally",1)
                    };
                }));

            // Jester Zombie
            assets.Add(
                new CardDataBuilder(this).CreateUnit("JesterZombie", "Jester Zombie")
                .SetSprites("JesterZombie.png", "crazy_zombie_bg.png")
                .SetStats(5, 1, 3)
                .WithCardType("Friendly")
                .SetTraits(new CardData.TraitStacks[1]
                {
                    TStack("Smackback",1)
                }));


            // Flamenco Zombie
            assets.Add(
                new CardDataBuilder(this).CreateUnit("FlamencoZombie", "Flamenco Zombie")
                .SetSprites("FlamencoZombie.png", "crazy_zombie_bg.png")
                .SetStats(2, 3, 5)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1]
                    {
                        SStack("When Deployed Gain Spice Per Dancing Zombie",2)
                    };
                }));

            // Bungee Plumber
            assets.Add(
                new CardDataBuilder(this).CreateItem("BungeePlumber", "Bungee Plumber")
                .SetSprites("BungeePlumber.png", "crazy_zombie_bg.png")
                .SetDamage(2)
                .WithCardType("Item")
                .WithFlavour("Likes: Plumbing and adventure sports. Dislikes: Belts.")
                );

            // Trapper Territory
            assets.Add(
                new CardDataBuilder(this).CreateUnit("EnvTrapper", "Trapper Territory")
                .SetSprites("EnvTrapper.png","crazy_zombie_bg.png")
                .WithCardType("Clunker")
                .SetStats(null, 1, 2)
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.traits = new List<CardData.TraitStacks>()
                    {
                        TStack("Barrage",1),
                        TStack("Backline",1)
                    };
                    data.startWithEffects = new CardData.StatusEffectStacks[1]
                    {
                        SStack("Scrap",1)
                    };
                }));

            // Mystery Egg
            assets.Add(
                new CardDataBuilder(this).CreateUnit("MysteryEgg", "Mystery Egg")
                .SetSprites("MysteryEgg.png", "crazy_zombie_bg.png")
                .WithCardType("Friendly")
                .SetStats(2, null, 0)
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1]
                    {
                        SStack("When Destroyed Summon Random Zombie",1)
                    };
                }));

            // Tennis Champ
            assets.Add(
                new CardDataBuilder(this).CreateUnit("TennisChamp", "Tennis Champ")
                .SetSprites("TennisChamp.png", "crazy_zombie_bg.png")
                .WithCardType("Friendly")
                .SetStats(1, 1, 2)
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1]
                    {
                        SStack("When Deployed Gain Spice",3)
                    };
                }));

            // Meteor Z
            assets.Add(
                new CardDataBuilder(this).CreateUnit("MeteorZ", "Meteor Z")
                .SetSprites("MeteorZ.png", "crazy_zombie_bg.png")
                .WithCardType("Clunker")
                .SetStats(null, null, 0)
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.traits = new List<CardData.TraitStacks>()
                    {
                        TStack("Backline",1)
                    };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("While Active Increase Attack To AlliesInRow",2),
                        SStack("Scrap",1)
                    };
                }));

            // Sugary Treat
            assets.Add(
                new CardDataBuilder(this).CreateItem("SugaryTreat", "Sugary Treat")
                .SetSprites("SugaryTreat.png","crazy_zombie_bg.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.attackEffects =
                    [
                        SStack("Increase Attack",3),
                        SStack("Increase Max Health",1)
                    ];
                }));

            // Cakesplosion
            assets.Add(
                new CardDataBuilder(this).CreateItem("Cakesplosion", "Cakesplosion")
                .SetSprites("Cakesplosion.png", "crazy_zombie_bg.png")
                .SetDamage(4)
                .WithFlavour("4 eggs. 2 cups flour. 10 sticks TNT.")
                );

            // Orchestra Conductor
            assets.Add(
                new CardDataBuilder(this).CreateUnit("OrchestraConductor", "Orchestra Conductor")
                .SetSprites("OrchestraConductor.png", "crazy_zombie_bg.png")
                .SetStats(2, 0, 4)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("When Deployed Increase All Allies Attack",2),
                        SStack("When Deployed Increase Self Attack",2)
                    ];
                }));

            // Gravestone (Conga Zombie)
            assets.Add(
                new CardDataBuilder(this).CreateUnit("GraveConga", "Gravestone: Conga Zombie")
                .SetSprites("GraveConga.png", "crazy_zombie_bg.png")
                .SetStats(1, null, 3)
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("Ignore Damage",1),
                        SStack("Grave Reveal Conga",1),
                        SStack("Grave Open",1)
                    ];
                }));

            // Conga Zombie
            assets.Add(
                new CardDataBuilder(this).CreateUnit("CongaZombie", "Conga Zombie")
                .SetSprites("CongaZombie.png", "crazy_zombie_bg.png")
                .SetStats(2, 2, 2)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("Trigger When Revealed",1)
                    ];
                }));

            // Gravestone (Newspaper Zombie)
            assets.Add(
                new CardDataBuilder(this).CreateUnit("GraveNewspaper", "Gravestone: Newspaper Zombie")
                .SetSprites("GraveNewspaper.png", "crazy_zombie_bg.png")
                .SetStats(1, null, 3)
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("Ignore Damage",1),
                        SStack("Grave Reveal Newspaper",1),
                        SStack("Grave Open",1)
                    ];
                }));

            // Newspaper Zombie
            assets.Add(
                new CardDataBuilder(this).CreateUnit("NewspaperZombie", "Newspaper Zombie")
                .SetSprites("NewspaperZombie.png", "crazy_zombie_bg.png")
                .SetStats(4, 1, 3)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("When Hit Increase Attack",4)
                    ];
                }));

            // Foot Soldier Zombie
            assets.Add(
                new CardDataBuilder(this).CreateUnit("FootSoldier", "Foot Soldier Zombie")
                .SetSprites("FootSoldier.png", "crazy_zombie_bg.png")
                .SetStats(4, 3, 5)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.traits =
                    [
                        TStack("Backline",1),
                        TStack("Spark",1)
                    ];
                }));

            // Loose Cannon (eye forgive you :3)
            
            assets.Add(
                new CardDataBuilder(this).CreateUnit("LooseCannon", "Loose Cannon")
                .SetSprites("LooseCannon.png", "crazy_zombie_bg.png")
                .SetStats(1, 1, 2)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("MultiHit",1),
                        SStack("Overshoot",1)
                    ];
                   
                }));

            //////////////////////////////////////////////////////////////
            /// TESTING
            //////////////////////////////////////////////////////////////

            assets.Add(
                new CardDataBuilder(this).CreateUnit("WinCannon", "Win Cannon")
                .SetSprites("LooseCannon.png", "crazy_zombie_bg.png")
                .SetStats(9, 1, 2)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("MultiHit",1),
                        SStack("When Hit Apply Removeable Longshot To Self",1)
                    ];

                }));

            //////////////////////////////////////////////////////////////
            /// TESTING DONE
            //////////////////////////////////////////////////////////////

            // Gravestone (Exploding Imp)
            assets.Add(
                new CardDataBuilder(this).CreateUnit("GraveExplodingImp", "Gravestone: Exploding Imp")
                .SetSprites("GraveExplodingImp.png", "crazy_zombie_bg.png")
                .SetStats(1, null, 3)
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("Ignore Damage",1),
                        SStack("Grave Reveal Exploding Imp",1),
                        SStack("Grave Open",1)
                    ];
                }));

            // Exploding Imp
            assets.Add(
                new CardDataBuilder(this).CreateUnit("ExplodingImp", "Exploding Imp")
                .SetSprites("ExplodingImp.png", "crazy_zombie_bg.png")
                .SetStats(1, 6, 2)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("Destroy Self After Turn",1)
                    ];
                }));

            // Gizzard Lizzard (Item)
            assets.Add(
                new CardDataBuilder(this).CreateItem("LizardItem","Lizard Treat")
                .SetSprites("LizardTreat.png","crazy_zombie_bg.png")
                .WithCardType("Item")
                .CanPlayOnBoard(true)
                .CanPlayOnEnemy(false)
                .CanPlayOnFriendly(true)
                .CanPlayOnHand(false)
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.attackEffects =
                    [
                        SStack("Sacrifice Ally",1),
                        SStack("Instant Summon Gizzard Lizard",1)
                    ];
                    data.startWithEffects =
                    [
                        SStack("When Destroyed Apply Damage To Enemies",2)
                    ];
                    data.traits =
                    [
                        TStack("Consume",1)
                    ];
                }));

            // Gizzard Lizard (Unit)
            assets.Add(
                new CardDataBuilder(this).CreateUnit("GizzardLizard", "Gizzard Lizard")
                .SetSprites("GizzardLizard.png", "crazy_zombie_bg.png")
                .SetStats(3, 3, 3)
                .WithCardType("Friendly")
                .WithFlavour("He is the missing skink.")
                );


            // The Chickening
            assets.Add(
                new CardDataBuilder(this).CreateItem("ChickenAttack", "The Chickening")
                .SetSprites("ChickenAttack.png", "crazy_zombie_bg.png")
                .SetDamage(2)
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("Hit All Enemies",2)
                    ];
                    data.traits =
                    [
                        TStack("Consume",1)
                    ];
                }));

            // Imp-Throwing Gargantuar
            assets.Add(
                new CardDataBuilder(this).CreateUnit("ImpThrowGarg", "Imp-Throwing Gargantuar")
                .SetSprites("ImpThrowGarg.png", "crazy_zombie_bg.png")
                .SetStats(5, 5, 5)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("When Hit Summon Random Imp",1)
                    ];
                }));

            // Final Mission
            assets.Add(
                new CardDataBuilder(this).CreateItem("FinalMission", "Final Mission")
                .SetSprites("FinalMission.png", "crazy_zombie_bg.png")
                .WithCardType("Item")
                .CanPlayOnBoard(true)
                .CanPlayOnEnemy(false)
                .CanPlayOnFriendly(true)
                .CanPlayOnHand(false)
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.attackEffects =
                    [
                        SStack("Sacrifice Ally",1),
                    ];
                    data.startWithEffects =
                    [
                        SStack("When Destroyed Apply Damage To Enemies",4)
                    ];
                    data.traits =
                    [
                        TStack("Consume",1)
                    ];
                }));

            // Cosmic Dancer
            assets.Add(
                new CardDataBuilder(this).CreateUnit("CosmicDancer", "Cosmic Dancer")
                .SetSprites("CosmicDancer.png", "crazy_zombie_bg.png")
                .SetStats(3, 2, 4)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("When Deployed Summon Random Dancing With Overshoot",1)
                    ];
                }));

            // Zombot's Wrath
            assets.Add(
                new CardDataBuilder(this).CreateItem("ZombotWrath", "Zombot's Wrath")
                .SetSprites("ZombotWrath.png", "crazy_zombie_bg.png")
                .SetDamage(3)
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("Bonus Damage If Row Full",3)
                    ];
                }));

            // Aerobics Instructor
            assets.Add(
                new CardDataBuilder(this).CreateUnit("Aerobics", "Aerobics Instructor")
                .SetSprites("Aerobics.png", "crazy_zombie_bg.png")
                .SetStats(3, 2, 4)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("On Card Played Increase Attack To Dancings",2),
                        SStack("On Card Played Apply Attack To Self",2)
                    ];
                }));

            // Disco Dance Floor: [AWAIT FUSION]

            // Abracadver: Trigger when Leader is hurt, Aimless, Regular Countdown
            assets.Add(
                new CardDataBuilder(this).CreateUnit("Abracadaver","Abracadaver")
                .SetSprites("Abracadaver.png","crazy_zombie_bg.png")
                .SetStats(2,3,3)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("Trigger When Leader Is Hit",1)
                    ];
                    data.traits =
                    [
                        TStack("Aimless",1)
                    ];
                }));

            // Fireworks Zombie: When deployed, deal 1 damage to everything (ApplyToFlags.Allies + ApplyToFlags.Enemies)

            // Gas Giant: When hurt, deal 1 damage to everything. Explode 5

            // Zombie's Best Friend: Summon a random Zombie with Sun <= 2 or 3 (or check stats?)    

            // Moon Base Z: While Active, Apply Overshoot (Temp Longshot + MultiHit 1)

            // Disco-Naut: While active, cards with <= 2 Attack have Bullseye

            // Quickdraw Con-Man: Bullseye, No Sun, 1 damage, Hits All Enemies, Trigger when the Redraw Bell is hit

            // Barrel o' Deadbeards: When destroyed, do 1 damage to everything. Summon Captain Deadbeard

            // Captain Deadbeard: 3,4,3

            // Quazar Wizard: When deployed: if there is an ally in the row, Conjure a Superpower

            // Valkyrie: While in hand, get +2 atk when an ally is destroyed

            // Tankylosaurus: Trigger when a card is drawn (Legendary)



            // Headhunter's Hat: Sacrifice an ally, summon Headhunter (Legendary)

            // Headhunter: Bullseye, 6,5,4, Trigger when a Dancing card is played (Legendary)

            // Binary Stars: All allies deal double damage (Legendary)
            
            assets.Add(
                new CardDataBuilder(this).CreateUnit("BinaryStars", "Binary Stars")
                .SetSprites("BinaryStars.png", "crazy_zombie_bg.png")
                .SetStats(3, 3, 3)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects =
                    [
                        SStack("While Active Double Allies Attack",1)
                    ];
                    data.traits =
                    [
                        TStack("Legendary",1)
                    ];
                }));

            // Garg Feast: Consume, MultiHit 3, Summon a random Gargantuar (Legendary)

            // Fruitcake: 7 damage, summon a Plum, Earth Berry or Beeberry on the enemy side

            ///////////////////////////////////////////////////////////////////////////////
            /// CUSTOM STATUS EFFECTS
            ///////////////////////////////////////////////////////////////////////////////

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Backup Dancer")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectSummon)data).gainTrait = null;
                    ((StatusEffectSummon)data).summonCard = TryGet<CardData>("BackupDancer");
                }));

            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Backup Dancer")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummon)data).targetSummon = TryGet<StatusEffectSummon>("Summon Backup Dancer");
                }));

            assets.Add(
                StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Backup Dancer")
                .WithText("When deployed, summon a {0}.")
                .WithTextInsert($"<card={GUID}.BackupDancer>")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectData>("Instant Summon Backup Dancer");
                }));

            assets.Add(
                StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Disco Zombie")
                .WithText("When deployed, summon a {0}.")
                .WithTextInsert($"<card={GUID}.DiscoZombie>")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectData>("Instant Summon Disco Zombie");
                }));

            assets.Add(
                new StatusEffectDataBuilder(this)
                .Create<StatusEffectTriggerWhenCertainAllyAttacks>("Trigger When Disco Zombie In Row Attacks")
                .WithCanBeBoosted(false)
                .WithText("Trigger when a {0} in the row attacks.")
                .WithTextInsert($"<card={GUID}.DiscoZombie>")
                .WithType("")
                .FreeModify(
                    delegate (StatusEffectData data)
                    {
                        data.isReaction = true;
                        data.stackable = false;
                    })
                .SubscribeToAfterAllBuildEvent(
                    delegate (StatusEffectData data)
                    {
                        ((StatusEffectTriggerWhenCertainAllyAttacks)data).ally = TryGet<CardData>("DiscoZombie");
                    }));

            assets.Add(
                StatusCopy("When Deployed Apply Frenzy To Self", "When Deployed Gain Damage And Health Per Ally")
                .WithCanBeBoosted(true)
                .WithText("When deployed, gain + <{a}> <keyword=attack> and <keyword=health> per ally on the field.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDeployed)data).scriptableAmount = ScriptableObject.CreateInstance<ScriptableAllyNbr>();
                    ((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectData>("Increase Attack & Health");
                }));

            assets.Add(
                StatusCopy("On Kill Draw", "On Kill Trigger")
                .WithText($"<keyword={GUID}.frenzied>")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXOnKill)data).effectToApply = TryGet<StatusEffectData>("Trigger");
                }));

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Disco Zombie")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectSummon)data).summonCard = TryGet<CardData>("DiscoZombie");
                    ((StatusEffectSummon)data).gainTrait = null;
                }));

            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Disco Zombie")
                .WithText($"Summon a <card={GUID}.DiscoZombie>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummon)data).targetSummon = TryGet<StatusEffectSummon>("Summon Disco Zombie");
                }));

            assets.Add(
                StatusCopy("When Deployed Apply Frenzy To Self", "When Deployed Gain Spice Per Dancing Zombie")
                .WithCanBeBoosted(true)
                .WithText("When deployed, gain <{a}> <keyword=spice> per Dancing Zombie on the field.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDeployed)data).scriptableAmount = ScriptableObject.CreateInstance<ScriptableFlamencoEffect>();
                    ((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectData>("Spice");
                }));

            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectInstantSummonRandomZombieFromTribe>("Instant Summon Random Zombie")
                .WithCanBeBoosted(false)
                .WithType("")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummonRandomZombieFromTribe)data).targetSummon = TryGet<StatusEffectSummon>("Summon Plep");
                    ((StatusEffectInstantSummonRandomZombieFromTribe)data).zombies = ConjureZombie.All.zombies;
                }));

            assets.Add(
                StatusCopy("When Destroyed Summon Dregg", "When Destroyed Summon Random Zombie")
                .WithCanBeBoosted(false)
                .WithText("When destroyed, summon a random zombie.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDestroyed)data).effectToApply = TryGet<StatusEffectData>("Instant Summon Random Zombie");
                }));

            assets.Add(
                StatusCopy("When Deployed Apply Frenzy To Self", "When Deployed Gain Spice")
                .WithCanBeBoosted(true)
                .WithText("When deployed, gain <{a}> <keyword=spice>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectData>("Spice");
                }));

            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectApplyXWhenDeployed>("When Deployed Increase All Allies Attack")
                .WithCanBeBoosted(true)
                .WithType("")
                .WithText("When deployed, all allies get +<{a}> <keyword=attack>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDeployed)data).applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    ((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectData>("Increase Attack");
                }));

            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectApplyXWhenDeployed>("When Deployed Increase Self Attack")
                .WithCanBeBoosted(true)
                .WithType("")
                .WithText("When deployed, get +<{a}> <keyword=attack>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDeployed)data).applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    ((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectData>("Increase Attack");
                }));

            assets.Add(
                StatusCopy("When Destroyed Summon Dregg", "Grave Reveal Conga")
                .WithCanBeBoosted(false)
                .WithText($"<keyword={GUID}.gravestone>. When revealed, summon a <card={GUID}.CongaZombie>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDestroyed)data).effectToApply = TryGet<StatusEffectData>("Instant Summon Conga Zombie");
                }));

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Conga Zombie")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectSummon)data).summonCard = TryGet<CardData>("CongaZombie");
                    ((StatusEffectSummon)data).gainTrait = null;
                }));

            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Conga Zombie")
                .WithText($"Summon a <card={GUID}.CongaZombie>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummon)data).targetSummon = TryGet<StatusEffectSummon>("Summon Conga Zombie");
                    ((StatusEffectInstantSummon)data).summonPosition = StatusEffectInstantSummon.Position.AppliersPosition;
                }));

            assets.Add(
                StatusCopy("Trigger When Deployed","Trigger When Revealed")
                .WithText("Trigger when revealed."));

            assets.Add(
                StatusCopy("Destroy Self After Turn", "Grave Open")
                .WithText(""));

            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectIgnoreDamageOnHit>("Ignore Damage")
                .WithCanBeBoosted(false)
                .WithType("")
                .WithText(""));

            assets.Add(
                StatusCopy("When Hit Heal Self", "When Hit Increase Attack")
                .WithCanBeBoosted(true)
                .WithType("")
                .WithText("When hit, get +<{a}> <keyword=attack>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenHit)data).effectToApply = TryGet<StatusEffectData>("Increase Attack");
                }));

            assets.Add(
                StatusCopy("When Destroyed Summon Dregg", "Grave Reveal Newspaper")
                .WithCanBeBoosted(false)
                .WithText($"<keyword={GUID}.gravestone>. When revealed, summon a <card={GUID}.NewspaperZombie>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDestroyed)data).effectToApply = TryGet<StatusEffectData>("Instant Summon Newspaper Zombie");
                }));

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Newspaper Zombie")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectSummon)data).summonCard = TryGet<CardData>("NewspaperZombie");
                    ((StatusEffectSummon)data).gainTrait = null;
                }));

            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Newspaper Zombie")
                .WithText($"Summon a <card={GUID}.CongaZombie>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummon)data).targetSummon = TryGet<StatusEffectSummon>("Summon Newspaper Zombie");
                    ((StatusEffectInstantSummon)data).summonPosition = StatusEffectInstantSummon.Position.AppliersPosition;
                }));

            /*
            assets.Add(
                StatusCopy("Pre Trigger Gain Frenzy Equal To Scrap", "Pre Trigger Apply Overshoot")
                .WithText("{0} <{a}>")
                .WithTextInsert($"<keyword={GUID}.overshoot>")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXPreTrigger)data).effectToApply = TryGet<StatusEffectData>("Overshoot");
                }));

            
            assets.Add(
                StatusCopy("Temporary Barrage", "Temporary Longshot")
                .WithText("")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectTemporaryTrait)data).trait = TryGet<TraitData>("Longshot");
                }));
            */

            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectSporadicTrait>("Overshoot")
                .WithType("")
                .WithText($"<keyword={GUID}.overshoot>")
                .WithCanBeBoosted(false)
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectSporadicTrait)data).trait = TryGet<TraitData>("Longshot");
                }));


            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectTemporaryTraitRemove>("Removeable Longshot")
                .WithText("When triggered for the first time, has {0}")
                .WithTextInsert($"<keyword={GUID}.harpoon>")
                .WithType("")
                .WithCanBeBoosted(false)
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectTemporaryTraitRemove)data).trait = TryGet<TraitData>("Harpoon Strike");
                }));

            assets.Add(
                StatusCopy("When Hit Apply Ink To Self", "When Hit Apply Removeable Longshot To Self")
                .WithText($"When hit, gain <keyword={GUID}.harpoon> and trigger.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenHit)data).effectToApply = TryGet<StatusEffectData>("Removeable Longshot");
                }));
           
            /*
            assets.Add(
                StatusCopy("Apply Haze", "Set Attack To Overshoot Stacks")
                .WithCanBeBoosted(true)
                .WithText("When deployed, gain <{a}> <keyword=spice> per Dancing Zombie on the field.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantApplyEffect)data).scriptableAmount = ScriptableObject.CreateInstance<ScriptableOvershootStacks>();
                    ((StatusEffectInstantApplyEffect)data).effectToApply = TryGet<StatusEffectData>("Set Attack");
                }));
            */

            assets.Add(
                StatusCopy("When Destroyed Summon Dregg", "Grave Reveal Exploding Imp")
                .WithCanBeBoosted(false)
                .WithText($"<keyword={GUID}.gravestone>. When revealed, summon a <card={GUID}.ExplodingImp>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDestroyed)data).effectToApply = TryGet<StatusEffectData>("Instant Summon Exploding Imp");
                }));

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Exploding Imp")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectSummon)data).summonCard = TryGet<CardData>("ExplodingImp");
                    ((StatusEffectSummon)data).gainTrait = null;
                }));

            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Exploding Imp")
                .WithText($"Summon a <card={GUID}.CongaZombie>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummon)data).targetSummon = TryGet<StatusEffectSummon>("Summon Exploding Imp");
                    ((StatusEffectInstantSummon)data).summonPosition = StatusEffectInstantSummon.Position.AppliersPosition;
                }));

            assets.Add(
                StatusCopy("When Destroyed Apply Damage To Allies", "When Destroyed Apply Damage To Enemies")
                .WithText("When destroyed, deal <{a}> damage to all enemies.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDestroyed)data).applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                }));


            assets.Add(
                StatusCopy("Summon Fallow", "Summon Gizzard Lizard")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectSummon)data).summonCard = TryGet<CardData>("GizzardLizard");
                    ((StatusEffectSummon)data).gainTrait = null;
                }));

            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Gizzard Lizard")
                .WithText($"Summon a <card={GUID}.DiscoZombie>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummon)data).targetSummon = TryGet<StatusEffectSummon>("Summon Gizzard Lizard");
                }));

            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectInstantSummonRandomZombieFromTribe>("Instant Summon Random Imp")
                .WithCanBeBoosted(false)
                .WithType("")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummonRandomZombieFromTribe)data).targetSummon = TryGet<StatusEffectSummon>("Summon Plep");
                    ((StatusEffectInstantSummonRandomZombieFromTribe)data).zombies = ConjureZombie.Imp.zombies;
                }));

            assets.Add(
                StatusCopy("When Hit Draw", "When Hit Summon Random Imp")
                .WithText($"When hit, summon a random Imp.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenHit)data).effectToApply = TryGet<StatusEffectInstantSummonRandomZombieFromTribe>("Instant Summon Random Imp");
                }));

            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectInstantSummonRandomZombieFromTribe>("Instant Summon Random Dancing With Overshoot")
                .WithCanBeBoosted(false)
                .WithType("")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectInstantSummonRandomZombieFromTribe)data).targetSummon = TryGet<StatusEffectSummon>("Summon Plep");
                    ((StatusEffectInstantSummonRandomZombieFromTribe)data).withEffects = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Overshoot"),
                        TryGet<StatusEffectData>("MultiHit")
                    };
                    ((StatusEffectInstantSummonRandomZombieFromTribe)data).zombies = ConjureZombie.Dancing.zombies;
                }));

            assets.Add(
                StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Random Dancing With Overshoot")
                .WithText($"When deployed, summon a random Dancing zombie, and it gets <keyword={GUID}.overshoot>.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectInstantSummonRandomZombieFromTribe>("Instant Summon Random Dancing With Overshoot");
                }));

            assets.Add(
                StatusCopy("Bonus Damage Equal To Scrap", "Bonus Damage If Row Full")
                .WithText("Deal <{a}> additional damage if the target's row is full.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectBonusDamageEqualToX)data).scriptableAmount = ScriptableObject.CreateInstance<ScriptableBonusDamageIfRowFull>();
                    ((StatusEffectBonusDamageEqualToX)data).on = StatusEffectBonusDamageEqualToX.On.ScriptableAmount;
                    ((StatusEffectBonusDamageEqualToX)data).health = false;
                }));

            assets.Add(
                StatusCopy("On Card Played Apply Attack To Self", "On Card Played Increase Attack To Dancings")
                .WithText("Give + <{a}> <keyword=attack> to all Dancing Zombies.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    var constraint = ScriptableObject.CreateInstance<TargetConstraintInTribe>();
                    constraint.tribe = ConjureZombie.Dancing.zombies;
                    ((StatusEffectApplyXOnCardPlayed)data).applyConstraints = new TargetConstraint[] { constraint };
                    ((StatusEffectApplyXOnCardPlayed)data).applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    ((StatusEffectApplyXOnCardPlayed)data).effectToApply = TryGet<StatusEffectData>("Increase Attack");
                }));

            /*
            assets.Add(
                StatusCopy("Trigger When Ally Is Hit", "Trigger When Leader Is Hit")
                .WithText("Trigger when your leader is hit.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    
                }));
            */

            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectApplyXWhenLeaderIsHit>("Trigger When Leader Is Hit")
                .WithCanBeBoosted(false)
                .WithIsReaction(true)
                .WithText("Trigger when your leader is hit.")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectApplyXWhenLeaderIsHit)data).effectToApply = TryGet<StatusEffectData>("Trigger");
                }));

            assets.Add(
                StatusCopy("While Active Barrage To Allies", "While Active Double Allies Attack")
                .WithText("While active, all allies have double <keyword=attack>.")
                .WithCanBeBoosted(false)
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectWhileActiveX)data).effectToApply = TryGet<StatusEffectData>("Double Attack");
                }));
            
            ///////////////////////////////////////////////////////////////////////////////
            /// CUSTOM TRAITS
            ///////////////////////////////////////////////////////////////////////////////

            /*
            assets.Add(
                new TraitDataBuilder(this).Create("Set Attack To Overshoot Stacks(No Desc)")
                .SubscribeToAfterAllBuildEvent(
                    (trait) =>
                    {
                        trait.keyword = TryGet<KeywordData>("none");
                        trait.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Set Attack To Overshoot Stacks") };
                    }));
            */

            // Bullseye: Ignore on hit effects? Backup: ignore Shell/Block

            // Legendary: The card has Fragile and Hogheaded
            assets.Add(
                new TraitDataBuilder(this).Create("Legendary")
                .SubscribeToAfterAllBuildEvent(
                    (trait) =>
                    {
                        trait.keyword = TryGet<KeywordData>("legendary");
                        trait.effects = new StatusEffectData[]
                        {
                            TryGet<StatusEffectData>("Cannot Increase Max Health"),
                            TryGet<StatusEffectData>("Cannot Recall"),
                        };
                    }));

            // Fusion: ????????

            assets.Add(
                new TraitDataBuilder(this).Create("Harpoon Strike")
                .SubscribeToAfterAllBuildEvent(
                    (trait) =>
                    {
                        trait.keyword = TryGet<KeywordData>("harpoon");
                        trait.effects = new StatusEffectData[]
                        {
                            TryGet<StatusEffectData>("On Hit Pull Target"),
                            TryGet<StatusEffectData>("Hit Furthest Target"),
                            TryGet<StatusEffectData>("Trigger")
                        };
                    }));

            ///////////////////////////////////////////////////////////////////////////////
            /// CUSTOM KEYWORDS
            ///////////////////////////////////////////////////////////////////////////////

            assets.Add(
                new KeywordDataBuilder(this)
                .Create("frenzied")
                .WithTitle("Frenzied")
                .WithTitleColour(new UnityEngine.Color(0.85f, 0.85f, 0.45f))
                .WithShowName(true)
                .WithDescription("Trigger when this creature kills an enemy.|Can lead to chain reactions!")
                .WithNoteColour(new Color(0.85f, 0.85f, 0.45f))
                .WithBodyColour(new Color(0.55f, 0.55f, 0.15f))
                .WithCanStack(false)
                );

            assets.Add(
                new KeywordDataBuilder(this)
                .Create("gravestone")
                .WithTitle("Gravestone")
                .WithTitleColour(new UnityEngine.Color(0.75f, 0.75f, 0.75f))
                .WithShowName(true)
                .WithDescription("Gravestones reveal their zombie after the counter reaches 0.|They are immune to all damage!")
                .WithNoteColour(new Color(0.75f, 0.75f, 0.75f))
                .WithBodyColour(new Color(0.50f, 0.50f, 0.50f))
                .WithCanStack(false)
                );

            assets.Add(
                new KeywordDataBuilder(this)
                .Create("overshoot")
                .WithTitle("Overshoot")
                .WithTitleColour(new UnityEngine.Color(0.85f, 0.85f, 0.45f))
                .WithShowName(true)
                .WithDescription("Deal damage to the enemy in the back of the row alongside the regular target.|If there's only one enemy, it will take that damage and the card's normal damage!")
                .WithNoteColour(new Color(0.85f, 0.85f, 0.45f))
                .WithBodyColour(new Color(0.50f, 0.50f, 0.50f))
                .WithCanStack(true)
                );

            assets.Add(
                new KeywordDataBuilder(this)
                .Create("harpoon")
                .WithTitle("Harpoon Strike")
                .WithDescription("Have <keyword=longshot> and <keyword=pull>")
                .WithShowName(true)
                .WithCanStack(false));

            assets.Add(
                new KeywordDataBuilder(this)
                .Create("legendary")
                .WithTitle("Legendary")
                .WithDescription("Have <keyword=pigheaded> and <keyword=fragile>.|Legendary cards have strong effects!")
                .WithShowName(true)
                .WithTitleColour(new UnityEngine.Color(0.85f, 0.45f, 0.45f))
                .WithNoteColour(new Color(0.85f, 0.45f, 0.45f))
                .WithBodyColour(new Color(0.50f, 0.50f, 0.50f))
                .WithCanStack(false));

            /*
            assets.Add(
                new KeywordDataBuilder(this)
                .Create("none")
                .WithTitle("None")
                .WithShowName(false)
                .WithShow(false)
                .WithDescription("No desc")
                .WithCanStack(false)
                );
            */

            ///////////////////////////////////////////////////////////////////////////////
            /// CUSTOM CHARMS
            ///////////////////////////////////////////////////////////////////////////////

            assets.Add(
                new CardUpgradeDataBuilder(this)
                .CreateCharm("CardUpgradeManiacalLaughter")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("ManiacalCharm.png")
                .WithTitle("Maniacal Laughter Charm")
                .WithText($"Gain <keyword={GUID}.frenzied>, +4 <keyword=attack> and +4 <keyword=health>")
                .WithTier(3)
                .ChangeHP(4)
                .WithSetHP(false)
                .ChangeDamage(4)
                .WithSetDamage(false)
                .SubscribeToAfterAllBuildEvent(delegate (CardUpgradeData data)
                {
                    data.effects = new CardData.StatusEffectStacks[1] { SStack("On Kill Trigger", 1) };
                }));

            ///////////////////////////////////////////////////////////////////////////////
            /// TESTING AREA
            ///////////////////////////////////////////////////////////////////////////////


            return assets.OfType<T>().ToList();     //Return the correct builders.
        }

        

    }
}
