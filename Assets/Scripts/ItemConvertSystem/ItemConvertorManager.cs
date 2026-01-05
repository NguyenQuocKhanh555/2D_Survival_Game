using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemConvertorManager : MonoBehaviour
{
    private SortedList<float, List<ItemConvertorInteract>> _processSortedList = new SortedList<float, List<ItemConvertorInteract>>();

    private void Start()
    {
        TimeController.instance.onTimeTick += ProcessCheck;
    }

    private void OnDestroy()
    {
        TimeController.instance.onTimeTick -= ProcessCheck;
    }

    private void ProcessCheck()
    {
        float currentPhase = TimeController.instance.GetTime();

        while (_processSortedList.Count > 0)
        {
            float firstEntry = _processSortedList.Keys[0];
            if (firstEntry > currentPhase) break;

            List<ItemConvertorInteract> convertors = _processSortedList[firstEntry];
            _processSortedList.RemoveAt(0);

            foreach (ItemConvertorInteract convertor in convertors)
            {
                convertor.StopAnimation();
            }
        }
    }

    public void ScheduleConvertor(ItemConvertorInteract convertor, float stopTime)
    {
        if (!_processSortedList.ContainsKey(stopTime))
        {
            _processSortedList[stopTime] = new List<ItemConvertorInteract>();
        }

        _processSortedList[stopTime].Add(convertor);
    }

    public void UnscheduleConvertor(ItemConvertorInteract convertor, int lastScheduledTime)
    {
        if (!_processSortedList.ContainsKey(lastScheduledTime)) return;
        if (!_processSortedList[lastScheduledTime].Contains(convertor)) return;
        _processSortedList[lastScheduledTime].Remove(convertor);
    }
}
