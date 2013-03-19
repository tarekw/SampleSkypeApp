using System.Collections.ObjectModel;
using SkypeTest.Engine.Models;

namespace SkypeTest.Data
{
    public class ContactsInGroup : ObservableCollection<Contact>
    {
        public ContactsInGroup(string category)
        {
            Key = category;
        }

        public string Key { get; set; }

        public bool HasItems { get { return Count > 0; } }
    }
}
