using System.Collections.Generic;
public static class Utility
{
    public static bool AreListsEqualContent<T>(List<T> list1, List<T> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        HashSet<T> set1 = new HashSet<T>(list1);
        HashSet<T> set2 = new HashSet<T>(list2);

        return set1.SetEquals(set2);
    }
}
