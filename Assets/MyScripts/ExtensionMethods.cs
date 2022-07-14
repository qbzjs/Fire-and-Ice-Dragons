using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�i��k
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// �j�M�l����
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="SearchObj">�j�M����</param>
    /// <param name="searchName">�j�M����W��</param>
    /// <returns></returns>
    public static T FindAnyChild<T>(this Transform SearchObj, string searchName ) where T : Component
    {
        for (int i = 0; i < SearchObj.childCount; i++)
        {
            if(SearchObj.GetChild(i).childCount > 0)//�l����U�٦��l����
            {
                var child = SearchObj.GetChild(i).FindAnyChild<Transform>(searchName);                
                if (child != null)
                    return child.GetComponent<T>();
            }
            if (SearchObj.GetChild(i).name == searchName)//��쪫��
            {
                return SearchObj.GetChild(i).GetComponent<T>();
            }
        }

        return default;
    }
}
