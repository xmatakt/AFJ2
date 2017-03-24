using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LA.Classes
{
    class State
    {
        private bool isInitialState;
        private bool isAcceptabelState;
        private int code;
        private string name;
        private Dictionary<char, State> transitions;

        public State(string name, bool isInitialState = false, bool isFinalState = false, int code = -1)
        {
            transitions = new Dictionary<char, State>();
            this.name = name;
            this.isAcceptabelState = isFinalState;
            this.isInitialState = isInitialState;
            this.code = code;
        }

        public void AddTrasition(char c, State state)
        {
            if (!transitions.ContainsKey(c))
                transitions.Add(c, state);
        }

        public State GetState(char c)
        {
            if (transitions.ContainsKey(c))
                return transitions[c];

            return null;
        }

        public int GetStateCode()
        {
            return code;
        }

        public bool GetStateAcceptability()
        {
            return isAcceptabelState;
        }

        public void ModifyState(int code)
        {
            isAcceptabelState = true;
            this.code = code;
        }
    }
}
