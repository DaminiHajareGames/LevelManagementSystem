using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CommanOfDamini.Enum;
using CommanOfDamini;

namespace CommanOfDamini.GenericClasses
{
    [Serializable]
    public class ListOfGenericClass<W, T> : MonoBehaviour where W : AvaiblityClass<T>
    {
        public T _currentObject;

        [SerializeField] public List<W> _entries;

        string _id;

        private int _currentIndex = -1;
        int currenetIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                if (CheckAvaiblity(value))
                {
                    _currentIndex = Mathf.Clamp(value, 0, _entries.Count - 1);
                    _currentObject = _entries[_currentIndex].obj;
                    _entries[_currentIndex].Opened();
                    SetValuesOnCurrentIndexChanged();
                }
            }
        }

        public virtual void Awake()
        {
            currenetIndex = 0;
            LoadFromSavedData();
        }

        public virtual void Start()
        {
        }

        //<Id And Save Data Related.......
        public virtual void ResetData()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                _entries[i].isLocked.ResetData();
            }
        }
    
        public void LoadFromSavedData()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                _entries[i].isLocked.LoadFromSavedData();
            }
        }

        public virtual void SetIds(string passedId)
        {
            _id = passedId;
            for (int i = 0; i < _entries.Count; i++)
            {
                _entries[i].isLocked.SetId(_id + i + _entries[i].isLocked.ToString());
            }
        }
        //>Id And Save Data Related.......

        public virtual void SetValuesOnCurrentIndexChanged()
        {
        }

        public virtual void SetCurrentIndexAccordingToGivenObject(W obj)
        {
           // Debug.Log("........... = " + obj);
            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Equals(obj))
                {
                    currenetIndex = i;
                    break;
                }
            }
        }

        bool CheckAvaiblity(int index)
        {
            if (index >= 0 && index < _entries.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckIsUnLocked(int index)
        {
            if (CheckAvaiblity(index))
            {
                if (_entries[index].isLocked.IsOpened())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public bool UnlockFirstElement()
        {
            return UnLockGiven(0);
        }

        public virtual bool UnLockGiven(int index)
        {
            if (CheckAvaiblity(index))
            {
                _entries[index].isLocked.SetToggleValue(Toggle.yes);
                return true;
            }
            return false;
        }

        public bool UnlockAndOpenFirstElement()
        {
            return UnLockAndOpenGiven(0);
        }

        public virtual bool UnlockAndOpenLastElement()
        {
            return UnLockAndOpenGiven(_entries.Count - 1);
        }

        public virtual bool UnLockAndOpenGiven(int givenIndex)
        {
            if (UnLockGiven(givenIndex))
            {
                if (Open(givenIndex))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool UnlockAndOpenNext()
        {
            return UnLockAndOpenGiven(currenetIndex + 1);
        }

        public virtual bool UnlockAndOpenPrevious()
        {
            return UnLockAndOpenGiven(currenetIndex - 1);
        }

        public virtual bool OpenFirstElement()
        {
            return Open(0);
        }

        public virtual void OpenWithoutAcknowledgment(int index)
        {
            Open(index);
        }

        public virtual bool Open(int givenIndex)
        {
            if (CheckAvaiblity(givenIndex))
            {
                if (CheckIsUnLocked(givenIndex))
                {
                    currenetIndex = givenIndex;
                    return true;
                }
            }
            return false;
        }

        bool OpenNext()
        {
            return Open(currenetIndex + 1);
        }

        bool OpenPrevious()
        {
            return Open(currenetIndex - 1);
        }
    }

    [Serializable]
    public class AvaiblityClass<T> 
    {
        public T obj;
       // public string avaiblityTypeReason;
        public ToggleClass isLocked;
        public delegate void OpenedAction();//AvaiblityClass<T> t);
        public event OpenedAction onOpenEvent = null;
        public string tempString;

        public void Opened()
        {
            if (onOpenEvent != null)
            {
                onOpenEvent();
            }
        }
    }

    [Serializable]
    public class ToggleClass
    {
        [SerializeField] string _id;
        public Toggle _toggle;
        public Toggle toggle
        {
            get
            {
                _toggle = PlayerPrefs.GetInt(_id, 0) == 0 ? Toggle.no : Toggle.yes;
                return _toggle;
            }
            set
            {
                _toggle = value;
                PlayerPrefs.SetInt(_id, _toggle == Toggle.no ? 0 : 1);
            }
        }

        public void SetId(string passedId)
        {
            _id = passedId;
        }

        public void LoadFromSavedData()
        {
            toggle = toggle;
        }

        public void ResetData()
        {
            toggle = Toggle.no;
        }

        public virtual bool IsOpened()
        {
            if (toggle == Toggle.yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetToggleValue(Toggle toggleValue)
        {
            toggle = toggleValue;
        }
    }

    public class LevelGenericClass : MonoBehaviour
    {
        #region USSGAE_OF_LISTOFGENERIC_CLASS
        [Serializable]
        public class Type1Placement
        {
            [Serializable] public class Type1Entry : AvaiblityClass<int> { }
            [Serializable] public class Type1List : ListOfGenericClass<Type1Entry, int> { }

            [SerializeField] Type1List _list;
        }

        [Serializable]
        public class Type2Placement
        {
            [Serializable] public class Type2Entry : AvaiblityClass<Type1Placement> { }
            [Serializable] public class Type2List : ListOfGenericClass<Type2Entry, Type1Placement> { }

            [SerializeField] Type2List _list;
        }

        [Serializable]
        public class Round
        {
            [SerializeField] Type1Placement[] _list1;
            [SerializeField] Type2Placement levels;
        }

        [SerializeField]
        public Round round;

        //Another Example
        [Serializable] public class SceneAvaiblityClass : AvaiblityClass<int> { }
        [Serializable] public class Scenes : ListOfGenericClass<SceneAvaiblityClass, int> { }
        [SerializeField] public Scenes scenes;

        #endregion
    }
}
