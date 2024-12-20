using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIF1006_tp1
{
    /// <summary>
    /// Un état a un nom (p.ex. "s0", "s1", ou peu importe), peut être un état final, et peut transiter vers d'autres états
    /// selon un input lu (on a donc une liste de transitions, où chaque transition est un tuple (input, nouvel état) 
    /// </summary>
    public class State
    {
        public bool IsFinal {get; private set;}
        public string Name { get; private set; }
        public List<Transition> Transitions { get; private set; }

        public State (string name, bool isFinal)
        {
            Name = name;
            IsFinal = isFinal;
            Transitions = new List<Transition>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (IsFinal)
            {
                sb.Append($"({Name})");
            }
            else 
            {
                sb.Append(Name);
            }
            sb.Append("\n");
            foreach (var transition in Transitions)
            {
                sb.Append("\t");
                sb.Append(transition.ToString() + "\n");
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            State st = obj as State;
            return Name.Equals(st.Name);
        }
    }
}
