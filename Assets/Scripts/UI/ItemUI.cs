using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Pool;
using VirTest.Utility;
using System.Linq;
using VirTest.Event;
using System;

namespace VirTest.UI
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private int addBtnCount = 10;
        [SerializeField] private Button addBtn;
        [SerializeField] private Button reorderBtn;
        [SerializeField] private Button insertBtn;
        [SerializeField] private Button closeBtn;
        [SerializeField] private LeanGameObjectPool leanPool;
        [SerializeField] private GameObject itemParent;

        private List<ItemView> itemViews = new List<ItemView>();

        private void Awake()
        {
            addBtn.onClick.AddListener(()=>AddItems(addBtnCount));
            reorderBtn.onClick.AddListener(ReorderItems);
            insertBtn.onClick.AddListener(AddItem);
            closeBtn.onClick.AddListener(ClosePanel);

            EventManager.AddListener<ItemClick>(OnItemClick);
        }

        private void Start()
        {
            Deactivate();
        }

        private void AddItems(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddItem();
            }
        }

        private void AddItem()
        {
            GameObject spawnedObject = leanPool.Spawn(itemParent.transform);
            ItemView itemView = spawnedObject.GetComponent<ItemView>();
            string randString = RandomUtility.GenerateAlphaNumericString();

            itemView.SetIndex(randString);

            if (itemViews != null)
                itemViews.Add(itemView);
        }

        private void ReorderItems()
        {
            // Ordering the value instead of the UI object because it performs better
            List<string> indexList = itemViews.Select(i => i.Index).ToList();
            indexList.Sort();
            for (int i = 0; i < itemViews.Count; i++)
            {
                itemViews[i].SetIndex(indexList[i]);
            }
        }

        private void ClosePanel()
        {
            ClearData();
            Deactivate();
        }

        private void ClearData()
        {
            for (int i = 0; i < itemViews.Count; i++)
            {
                itemViews[i].ResetIndex();
            }
            itemViews.Clear();
            leanPool.DespawnAll();
        }

        private void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void OnItemClick(ItemClick evt)
        {
            Activate();
        }

        private void OnDestroy()
        {
            addBtn.onClick.RemoveAllListeners();
            reorderBtn.onClick.RemoveAllListeners();
            insertBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.RemoveAllListeners();

            EventManager.RemoveListener<ItemClick>(OnItemClick);
        }
    }
}
