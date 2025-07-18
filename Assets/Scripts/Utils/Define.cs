using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public class SlotData
    {
        public static int slotSize = 80; // 여기에서 슬롯 사이즈를 관리함. 자주 사용하므로 전역처리
    }
    public class ScoreData
    {
        public string player_ID;
        public int maxScore;
    }
    public class VolumeData
    {
        public float masterVolume;
        public float bgmVolume;
        public float sfxVolume;
        public VolumeData()
        {
            masterVolume = 0.7f;
            bgmVolume = 0.7f;
            sfxVolume = 0.7f;
        }
    }
    [System.Serializable]
    public class WholeGameData
    {
        public bool firstPlay;
        public int maxScore;

        public WholeGameData()
        {
            firstPlay = false;
            maxScore = 0;
        }
    }

    [System.Serializable]
    public class Blesses
    {
        public List<BlessData> bless; // BlessData의 List를 만들어주고 이에 접근하는 것이다.
    }
    [System.Serializable]
    public class BlessData
    {
        public int Index; // 인덱스
        public string Name; // 이름
        public string Effect; // 효과. -> exel의 큰축을 써준다. 이에 접근한다.

        public BlessData(int _index,string _name, string _effect)
        {
            this.Index = _index;
            this.Name = _name;
            this.Effect = _effect;
        } // BlessData를 호출할때 값을 설정할 수 있게해주자.
    }
    [System.Serializable]
    public class Items
    {
        public List<ItemData> items;
    }
    [System.Serializable]
    public class ItemData
    {
        public int Column1;
        public string Item;
        public string Slot;
        public string Ability;
        public Rarity Rarity;
        public UseType UseType;
        public string SpecialTag;
        public ItemData(int _index, string _name, string _slotsize,string _itemText,Rarity _rarity, UseType _useType, string _specialTag)
        {
            this.Column1 = _index;
            this.Item = _name;
            this.Slot = _slotsize;
            this.Ability = _itemText;
            this.Rarity = _rarity;
            this.UseType = _useType;
            this.SpecialTag = _specialTag;

        }
    }

  
    public enum ItemType
    {
        Riffle,
        FireFlame,
        Launcher,
        CartridgeBelt
    }
    public enum Rarity
    {
        Normal,
        Rare,
        Epic,
        Legend
    }

    public enum UseType
    {
        Active,
        Passive
    }
    public enum WorldObject
    {
        Unknown,
        Player,
        Enemy,
    }
    public enum State
    {
        Idle,
        Walk,
        Crouch
    }
    public enum UIEvent
    {
        Click,
        BeginDrag,
        Drag,
        DragEnd,
        PointerDown,
        PointerUP
    }
    public enum MouseEvent
    {
        Press,
        Click,
        End
    }
    public enum Scene
    {
        Unknown,
        TitleScene,
        MainGame,
        MainTitle
    }
    public enum Sound
    {
        Master,
        BGM,
        SFX,
        MaxCount
    }
}