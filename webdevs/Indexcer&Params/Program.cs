using System;
using System.Collections.Generic;
using System.Linq;

namespace Indexcer_Params {
    class Program {
        static void Main (string[] args) {
            var webDevs = new WebDevs ();

            var firstMemberOfInternalColeection = webDevs[0];

            Console.WriteLine ($"first member value is : {firstMemberOfInternalColeection}");

            var getPropsValue = webDevs["Admin1", "Admin2", "Admin3"];

            foreach (var propValue in getPropsValue) {
                Console.WriteLine ($"Args value in order : {propValue}");
            }

            Console.ReadLine ();
        }
    }

    public class WebDevs {

        private List<string> _members;

        public WebDevs () {
            _members = new List<string> {
                "Ms1",
                "Miss2",
                "Mrs3",
                "Mr1",
                "Mr2",
                "Mr3"
            };
        }
        public string Admin1 { get; set; } = "Arman";
        public string Admin2 { get; set; } = "Hesam";
        public string Admin3 { get; set; } = "Mehdi";

        public string this [int key] {
            get { return _members[key]; }
        }
        public IEnumerable<object> this [params string[] keys] {
            get {
                var properties = this.GetType ().GetProperties ();
                return properties.Where (prop => keys.Contains (prop.Name)).Select (prop => prop.GetValue (this));
                //var keys = this.GetType().GetProperty()
                //return keys.Select (key => _members[key]).AsEnumerable ();
            }
        }
    }
}