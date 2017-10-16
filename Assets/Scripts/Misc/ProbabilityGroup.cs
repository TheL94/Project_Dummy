using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityGroup<T> {

    List<Element<T>> Elements = new List<Element<T>>();

    #region API
    public ProbabilityGroup(List<Element<T>> _elements)
    {
        Elements = _elements;
    }

    public void Add(List<Element<T>> _elements)
    {
        Elements.AddRange(_elements);
        UpdateElementsPercentage();
    }

    public void Add(Element<T> _element)
    {
        Elements.Add(_element);
        UpdateElementsPercentage();
    }

    public T PickRandom()
    {
        if (Elements.Count > 0)
        {
            float randNum = Random.Range(0, 100);
            float tempMinValue = 0f;

            for (int i = 0; i < Elements.Count; i++)
            {
                if (randNum < (Elements[i].Percentage + tempMinValue) && randNum >= tempMinValue)
                    return Elements[i].Obj;

                tempMinValue += Elements[i].Percentage;
            }
        }
        return Elements[0].Obj;
    }
    #endregion

    void UpdateElementsPercentage()
    {
        if (Elements.Count <= 0)
            return;

        float weigthSum = 0;
        foreach (var element in Elements)
            weigthSum += element.Weigth;

        foreach (var element in Elements)
            element.Percentage = element.Weigth / weigthSum / 100;
    }

    public class Element<T>
    {
        public T Obj;
        public float Weigth;
        public float Percentage;

        public Element(T _obj, float _weigth)
        {
            Obj = _obj;
            Weigth = _weigth;
        }
    }
}
