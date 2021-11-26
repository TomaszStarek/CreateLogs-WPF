using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transponowanko
{
    public static class SumStringsList
    {
        public static IEnumerable<string> SumStringsLists(
            this IEnumerable<string> first,
            IEnumerable<string> second)
        {
            using (var enumeratorA = first.GetEnumerator())
            using (var enumeratorB = second.GetEnumerator())
            {
                while (enumeratorA.MoveNext())
                {
                    if (enumeratorB.MoveNext())
                        yield return enumeratorA.Current + enumeratorB.Current;
                    else
                        yield return enumeratorA.Current;
                }
                // should it continue iterating the second list?
                while (enumeratorB.MoveNext())
                    yield return enumeratorB.Current;
            }
        }


    }
}
