using System.Collections;

namespace TTC.Core;

public sealed class TimeTable : IEnumerable<List<Course>>
{
    private readonly List<Course>[,] _table;

    public TimeTable(int slots, int days)
    {
        _table = new List<Course>[slots, days];

        foreach (var day in ..days)
        {
            foreach (var slot in ..slots)
            {
                _table[slot, day] = [];
            }
        }
    }

    public List<Course> this[int slot, Day day] => _table[slot, (int) day];

    public IEnumerator<List<Course>> GetEnumerator() => _table.Cast<List<Course>>().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _table.GetEnumerator();
}
